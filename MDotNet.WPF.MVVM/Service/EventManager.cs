namespace MDotNet.WPF.MVVM.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Contracts;
	using Extensions;

	public class EventManager : IEventHandler
	{
		private readonly List<Handler> _handlers = new List<Handler>();

		#region IEventHandler Members
		/// <summary>
		///   Registers an instance to handlel events.
		/// </summary>
		/// <param name = "instance">The instance.</param>
		public void Register( object instance )
		{
			lock ( _handlers )
			{
				if ( _handlers.Any( x => x.Matches( instance ) ) )
					return;
				_handlers.Add( new Handler( instance ) );
			}
		}
		/// <summary>
		///   Unregisters the specified instance.
		/// </summary>
		/// <param name = "instance">The instance.</param>
		public void Unregister( object instance )
		{
			lock ( _handlers )
			{
				var found = _handlers.FirstOrDefault( x => x.Matches( instance ) );
				if ( found != null )
					_handlers.Remove( found );
			}
		}
		/// <summary>
		///   Fires an event for the specified message.
		/// </summary>
		/// <param name = "message">The message.</param>
		public void Fire( object message )
		{
			Handler[] toTrigger;
			lock ( _handlers )
				toTrigger = _handlers.ToArray();

			var messageType = message.GetType();
			var dead = toTrigger.Where( handler => !handler.Handle( messageType, message ) ).ToList();

			if ( dead.Any() )
				lock ( _handlers )
					dead.Apply( x => _handlers.Remove( x ) );
		}
		#endregion

		#region Nested type: Handler
		protected class Handler
		{
			private readonly WeakReference _reference;
			private readonly Dictionary<Type, MethodInfo> _suppoertedEvents = new Dictionary<Type, MethodInfo>();

			public Handler( object handler )
			{
				_reference = new WeakReference( handler );

				var interfaces = handler.GetType().GetInterfaces()
					.Where( i => typeof( IHandleEvent ).IsAssignableFrom( i ) && i.IsGenericParameter );

				foreach ( var @interface in interfaces )
				{
					var type = @interface.GetGenericArguments().First();
					var method = @interface.GetMethod( "Handle" );
					_suppoertedEvents[ type ] = method;
				}
			}

			public bool Matches( object instance )
			{
				return _reference.Target == instance;
			}

			public bool Handle( Type eventType, object message )
			{
				var target = _reference.Target;
				if ( target == null ) return false;

				foreach ( var pair in _suppoertedEvents )
				{
					if ( pair.Key.IsAssignableFrom( eventType ) )
					{
						pair.Value.Invoke( target, new[] { message } );
						return true;
					}
				}

				return true;
			}
		}
		#endregion
	}
}