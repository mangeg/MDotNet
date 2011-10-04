namespace MDotNet.WPF.MVVM.Action
{
	using System;
	using System.Collections.Generic;
	using Logging;

	/// <summary>
	///   An implementation of <see cref = "IResult" /> that enables sequential execution of multiple results.
	/// </summary>
	public class SequentialResult : IResult
	{
		private readonly IEnumerator<IResult> enumerator;
		private ActionExecutionContext context;

		/// <summary>
		///   Initializes a new instance of the <see cref = "SequentialResult" /> class.
		/// </summary>
		/// <param name = "enumerator">The enumerator.</param>
		public SequentialResult( IEnumerator<IResult> enumerator )
		{
			this.enumerator = enumerator;
		}

		#region IResult Members
		/// <summary>
		///   Occurs when execution has completed.
		/// </summary>
		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		/// <summary>
		///   Executes the result using the specified context.
		/// </summary>
		/// <param name = "context">The context.</param>
		public void Execute( ActionExecutionContext context )
		{
			this.context = context;
			ChildCompleted( null, new ResultCompletionEventArgs() );
		}
		#endregion

		private void ChildCompleted( object sender, ResultCompletionEventArgs args )
		{
			if ( args.Error != null || args.WasCancelled )
			{
				OnComplete( args.Error, args.WasCancelled );
				return;
			}

			var previous = sender as IResult;

			if ( previous != null )
				previous.Completed -= ChildCompleted;

			var moveNextSucceeded = false;
			try
			{
				moveNextSucceeded = enumerator.MoveNext();
			}
			catch ( Exception ex )
			{
				OnComplete( ex, false );
				return;
			}

			if ( moveNextSucceeded )
			{
				try
				{
					var next = enumerator.Current;
					IoC.Compose( next );
					next.Completed += ChildCompleted;
					next.Execute( context );
				}
				catch ( Exception ex )
				{
					OnComplete( ex, false );
					return;
				}
			}
			else OnComplete( null, false );
		}

		private void OnComplete( Exception error, bool wasCancelled )
		{
			enumerator.Dispose();
			Completed( this, new ResultCompletionEventArgs { Error = error, WasCancelled = wasCancelled } );
		}
	}

	/// <summary>
	///   The event args for the Completed event of an <see cref = "IResult" />.
	/// </summary>
	public class ResultCompletionEventArgs : EventArgs
	{
		/// <summary>
		///   Gets or sets the error if one occurred.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error;

		/// <summary>
		///   Gets or sets a value indicating whether the result was cancelled.
		/// </summary>
		/// <value><c>true</c> if cancelled; otherwise, <c>false</c>.</value>
		public bool WasCancelled;
	}

	/// <summary>
	///   Allows custom code to execute after the return of a action.
	/// </summary>
	public interface IResult
	{
		/// <summary>
		///   Executes the result using the specified context.
		/// </summary>
		/// <param name = "context">The context.</param>
		void Execute( ActionExecutionContext context );

		/// <summary>
		///   Occurs when execution has completed.
		/// </summary>
		event EventHandler<ResultCompletionEventArgs> Completed;
	}

	/// <summary>
	///   Manages coroutine execution.
	/// </summary>
	public static class Coroutine
	{
		private static readonly ILog Log = LogManager.GetLog( typeof( Coroutine ) );

		/// <summary>
		///   Creates the parent enumerator.
		/// </summary>
		public static Func<IEnumerator<IResult>, IResult> CreateParentEnumerator = inner => new SequentialResult( inner );

		/// <summary>
		///   Executes a coroutine.
		/// </summary>
		/// <param name = "coroutine">The coroutine to execute.</param>
		/// <param name = "context">The context to execute the coroutine within.</param>
		/// ///
		/// <param name = "callback">The completion callback for the coroutine.</param>
		public static void BeginExecute( IEnumerator<IResult> coroutine, ActionExecutionContext context = null,
		                                 EventHandler<ResultCompletionEventArgs> callback = null )
		{
			Log.Info( "Executing coroutine." );

			var enumerator = CreateParentEnumerator( coroutine );
			IoC.Compose( enumerator );

			if ( callback != null )
				enumerator.Completed += callback;
			enumerator.Completed += Completed;

			enumerator.Execute( context ?? new ActionExecutionContext() );
		}

		/// <summary>
		///   Called upon completion of a coroutine.
		/// </summary>
		public static event EventHandler<ResultCompletionEventArgs> Completed = ( s, e ) => {
			var enumerator = ( IResult )s;
			enumerator.Completed -= Completed;

			if ( e.Error != null )
				Log.Error( e.Error );
			else if ( e.WasCancelled )
				Log.Info( "Coroutine execution cancelled." );
			else
				Log.Info( "Coroutine execution completed." );
		};
	}
}