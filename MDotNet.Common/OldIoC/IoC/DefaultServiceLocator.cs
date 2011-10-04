namespace MDotNet.Common.OldIoC.IoC
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class DefaultServiceLocator : ContainerBase
	{
		private readonly Dictionary<string, Func<object>> _typeToHanlder = new Dictionary<string, Func<object>>();

		public DefaultServiceLocator()
		{
			AddHandler( typeof( IServiceLocator ), () => this );
			AddHandler( typeof( IRegistry ), () => this );
			AddHandler( typeof( IBuilder ), () => this );
			AddHandler( typeof( IContainer ), () => this );
		}

		public bool IsRegistered( Type type ) { return _typeToHanlder.ContainsKey( type.FullName ); }

		public bool IsRegistered( string fullName ) { return _typeToHanlder.ContainsKey( fullName ); }

		public void AddHandler( Type type, Func<object> handler ) { AddHandler( type.FullName, handler ); }

		public void AddHandler( string key, Func<object> handler )
		{
			if ( IsRegistered( key ) )
			{
				var exception = new ArgumentException( key + " was already registered in the container." );
				Log.Error( "Exceotion", exception );
				Log.Error( "Exceotion", exception );
				throw exception;
			}

			_typeToHanlder.Add( key, handler );
		}

		public Func<object> GetHandler( Type type )
		{
			if ( type.IsGenericType && !IsRegistered( type ) )
			{
				var genericType = type.GetGenericTypeDefinition();

				if ( !IsRegistered( genericType ) )
				{
					var exception = new ArgumentException( type + " is not a registered component." );
					Log.Error( "Exception", exception );
					throw exception;
				}

				return () => CreateInstance( InternalGetHandler( genericType )() as Type, type.GetGenericArguments() );
			}

			return InternalGetHandler( type );
		}

		private Func<Object> InternalGetHandler( Type type )
		{
			if ( type == null )
			{
				Log.Error( "InternalGetHanlder was called with null as type" );
				return () => null;
			}

			Func<object> handler;
			if ( !_typeToHanlder.TryGetValue( type.FullName, out handler ) )
			{
				if ( !type.IsAbstract )
				{
					AddHandler( type, () => CreateInstance( type ) );
					return GetHandler( type );
				}

				var exception = new ArgumentException( type + " is not a registered component." );
				Log.Error( exception );
				throw exception;
			}

			return handler;
		}

		private object CreateInstance( Type type, params Type[] typeArguments )
		{
			if ( typeArguments != null && typeArguments.Length > 0 )
				type = type.MakeGenericType( typeArguments );


			var args = DetermineConstructorArgs( type );

			object instance = args.Count() > 0 ? Activator.CreateInstance( type, args ) : Activator.CreateInstance( type );

			return instance;
		}

		public override object GetInstance( Type serviceType, string key )
		{
			return key == null
			       	? GetHandler( serviceType )()
			       	: ( serviceType == null )
			       	  	? GetInstance( key )
			       	  	: GetInstance( key, serviceType.GetGenericArguments() );
		}

		public object GetInstance( string key, params Type[] typeArguments )
		{
			Func<object> handler;

			if ( !_typeToHanlder.TryGetValue( key, out handler ) )
			{
				var exception = new ArgumentException( key + " is not a registered component key." );
				Log.Error( exception );
				throw exception;
			}

			var instance = handler();

			// TODO: That was hacky, must have some clever way to do this.
			var type = instance as Type;
			return type == null ? instance : CreateInstance( type, typeArguments );
		}

		public override IEnumerable<object> GetAllInstances( Type serviceType ) { return new[] { Activator.CreateInstance( serviceType ) }; }

		public override void BuildUp( object instance ) { }
	}
}