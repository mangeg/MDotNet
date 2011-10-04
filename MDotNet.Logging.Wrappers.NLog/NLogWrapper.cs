namespace MDotNet.Logging.Wrappers.NLog
{
	using System;
	using global::NLog;

	/// <summary>
	/// NLog wrapper for ILog
	/// </summary>
	internal class NLogWrapper : ILog
	{
		private Logger _log;

		/// <summary>
		/// Initializes a new instance of the <see cref="NLogWrapper"/> class.
		/// </summary>
		/// <param name="log">The log.</param>
		public NLogWrapper( Logger log ) { _log = log; }

		#region ILog Members

		/// <summary>
		/// Log a information message.
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Info( string format, params object[] args )
		{
			if ( IsException( args ) )
			{
				_log.InfoException( format, args[ 0 ] as Exception );
				return;
			}
			_log.Info( format, args );
		}

		/// <summary>
		/// Log a warning message.
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Warn( string format, params object[] args )
		{
			if ( IsException( args ) )
			{
				_log.WarnException( format, args[ 0 ] as Exception );
				return;
			}
			_log.Warn( format, args );
		}

		/// <summary>
		/// Log a trace message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Trace( string format, params object[] args )
		{
			if ( IsException( args ) )
			{
				_log.TraceException( format, args[ 0 ] as Exception );
				return;
			}
			_log.Trace( format, args );
		}

		/// <summary>
		/// Log a fatal message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Fatal( string format, params object[] args )
		{
			if ( IsException( args ) )
			{
				_log.FatalException( format, args[ 0 ] as Exception );
				return;
			}
			_log.Fatal( format, args );
		}

		/// <summary>
		/// Log a error message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Error( string format, params object[] args )
		{
			if ( IsException( args ) )
			{
				_log.ErrorException( format, args[ 0 ] as Exception );
				return;
			}
			_log.Error( format, args );
		}

		/// <summary>
		/// Log specified exception as an error.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void Error( Exception exception ) { _log.Error( exception ); }

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		public void Shutdown() { _log.Factory.Configuration = null; }

		#endregion

		private static bool IsException( params object[] args )
		{
			if ( args.Length == 1 && args[ 0 ] is Exception )
				return true;

			return false;
		}
	}
}