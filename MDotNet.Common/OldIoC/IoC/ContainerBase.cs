namespace MDotNet.Common.OldIoC.IoC
{
	using System;
	using System.Collections.Generic;
	using Logging;

	public abstract class ContainerBase : IContainer
	{
		private readonly Dictionary<Type, Delegate> _handlers = new Dictionary<Type, Delegate>();
		private ILog _log = LogManager.GetLog();

		public ILog Log
		{
			get { return _log; }
			set { _log = value; }
		}

		#region IContainer Members

		public abstract object GetInstance( Type serviceType, string key );

		public abstract IEnumerable<object> GetAllInstances( Type serviceType );

		public abstract void BuildUp( object instance );

		public virtual void Register( IEnumerable<ICoponentRegistration> registrations )
		{
			foreach ( var registration in registrations )
			{
				var key = registrations.GetType();
				Delegate handler;

				if ( _handlers.TryGetValue( key, out handler ) )
					handler.DynamicInvoke( registrations );
				else
				{
				}
			}
		}

		#endregion

		protected IEnumerable<object> DetermineConstructorArgs( Type implmentation )
		{
			var args = new List<object>();
			var constructor = implmentation.SelectEligibleConstructor();

			if ( constructor != null )
			{
				foreach ( var info in constructor.GetParameters() )
				{
					var arg = GetInstance( info.ParameterType, null );
					args.Add( arg );
				}
			}

			return args;
		}

		public void AddRegistrationHanlder<T>( Action<T> handler ) where T : ICoponentRegistration { _handlers[ typeof( T ) ] = handler; }
	}
}