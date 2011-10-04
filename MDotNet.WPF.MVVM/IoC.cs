namespace MDotNet.WPF.MVVM
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ApplicationModel;
	using Extensions;
	using MEF.Attributes;
	using Service.Contracts;

	public static class IoC
	{
		private static IServiceLocator _sLocator;

		/// <summary>
		///   Gets the service locator.
		/// </summary>
		public static IServiceLocator Locator
		{
			get { return _sLocator; }
		}

		/// <summary>
		///   Initializes the IoC.
		/// </summary>
		/// <param name = "locator">The service locator.</param>
		public static void Initialize( IServiceLocator locator )
		{
			_sLocator = locator;
		}

		/// <summary>
		///   Gets a instance of the specified interface or concrete type.
		/// </summary>
		/// <param name = "serviceType">Type of the service.</param>
		/// <returns>A instance of the requested type.</returns>
		public static object GetInstance( Type serviceType )
		{
			return GetInstance( serviceType, true );
		}
		/// <summary>
		///   Gets a instance of the specified interface or concrete type,
		///   and applies default values for DesignTime.
		/// </summary>
		/// <param name = "serviceType">Type of the service.</param>
		/// <param name = "designTimeDefaultValues">if set to <c>true</c> [design time default values].</param>
		/// <returns>A instance of the requested type.</returns>
		public static object GetInstance( Type serviceType, bool designTimeDefaultValues )
		{
			var ret = _sLocator.GetInstance( serviceType );
			if ( designTimeDefaultValues )
				InsertDesignDefaults( ret );
			return ret;
		}
		/// <summary>
		///   Gets a instance of the specified interface or concrete type.
		/// </summary>
		/// <typeparam name = "T">The interface or concrete type</typeparam>
		/// <returns>A instance of the requested type.</returns>
		public static T GetInstance<T>()
		{
			return GetInstance<T>( true );
		}
		/// <summary>
		///   Gets a instance of the specified interface or concrete type,
		///   and applies default values for DesignTime.
		/// </summary>
		/// <typeparam name = "T">The interface or concrete type</typeparam>
		/// <param name = "designTimeDefaultValues">if set to <c>true</c> [design time default values].</param>
		/// <returns>A instance of the requested type.</returns>
		public static T GetInstance<T>( bool designTimeDefaultValues )
		{
			var ret = GetInstance( typeof( T ), designTimeDefaultValues );
			return ( T )ret;
		}

		/// <summary>
		///   Gets a instance of the specified contract.
		/// </summary>
		/// <param name = "contractName">Name of the contract.</param>
		/// <returns>A instance of the requested contract</returns>
		public static object GetInstance( String contractName )
		{
			return GetInstance( contractName, true );
		}
		/// <summary>
		///   Gets a instance of the specified contract,
		///   and applies default values for DesignTime.
		/// </summary>
		/// <param name = "contractName">Name of the contract.</param>
		/// <param name = "designTimeDefaultValues">if set to <c>true</c> [design time default values].</param>
		/// <returns>A instance of the requested contract</returns>
		public static object GetInstance( String contractName, bool designTimeDefaultValues )
		{
			var ret = _sLocator.GetInstance( contractName );
			if ( designTimeDefaultValues )
				InsertDesignDefaults( ret );
			return ret;
		}

		/// <summary>
		///   Gets all instances of the specified service type.
		/// </summary>
		/// <param name = "serviceType">Type of the service.</param>
		/// <returns>A <see cref = "IEnumerable{T}" /> of all the instances of the specified type</returns>
		public static IEnumerable<object> GetAllInstances( Type serviceType )
		{
			return GetAllInstances( serviceType, true );
		}
		/// <summary>
		///   Gets all instances of the specified service type,
		///   and applies default values for DesignTime.
		/// </summary>
		/// <param name = "serviceType">Type of the service.</param>
		/// <param name = "designTimeDefaultValues">if set to <c>true</c> [design time default values].</param>
		/// <returns>A <see cref = "IEnumerable{T}" /> of all the instances of the specified type</returns>
		public static IEnumerable<object> GetAllInstances( Type serviceType, bool designTimeDefaultValues )
		{
			var ret = _sLocator.GetAllInstances( serviceType );
			if ( designTimeDefaultValues )
				ret.Apply( InsertDesignDefaults );
			return ret;
		}
		/// <summary>
		///   Gets all instances of the specified service type,
		/// </summary>
		/// <typeparam name = "T">The type of the service.</typeparam>
		/// <returns>A <see cref = "IEnumerable{T}" /> of all the instances of the specified type</returns>
		public static IEnumerable<T> GetAllInstances<T>()
		{
			return GetAllInstances<T>( true );
		}
		/// <summary>
		///   Gets all instances of the specified service type,
		/// </summary>
		/// <typeparam name = "T">The type of the service.</typeparam>
		/// <param name = "designTimeDefaultValues">if set to <c>true</c> [design time default values].</param>
		/// <returns>A <see cref = "IEnumerable{T}" /> of all the instances of the specified type</returns>
		public static IEnumerable<T> GetAllInstances<T>( bool designTimeDefaultValues )
		{
			return GetAllInstances( typeof( T ), true ).Cast<T>();
		}

		/// <summary>
		///   Gets all instances if the specified contract.
		/// </summary>
		/// <param name = "contractName">Name of the contract.</param>
		/// <returns>A <see cref = "IEnumerable{T}" /> of all the instances of the specified contract</returns>
		public static IEnumerable<object> GetAllInstances( String contractName )
		{
			return GetAllInstances( contractName, true );
		}
		/// <summary>
		///   Gets all instances if the specified contract,
		///   and applies default values for DesignTime.
		/// </summary>
		/// <param name = "contractName">Name of the contract.</param>
		/// <param name = "designTimeDefaultValues">if set to <c>true</c> [design time default values].</param>
		/// <returns>A <see cref = "IEnumerable{T}" /> of all the instances of the specified contract</returns>
		public static IEnumerable<object> GetAllInstances( String contractName, bool designTimeDefaultValues )
		{
			var ret = _sLocator.GetAllInstances( contractName );
			if ( designTimeDefaultValues )
				ret.Apply( InsertDesignDefaults );
			return ret;
		}

		/// <summary>
		///   Composes the specified instance and try to satisfy imports.
		/// </summary>
		/// <param name = "instance">The instance to compose.</param>
		public static void Compose( object instance )
		{
			Compose( instance, true );
		}
		/// <summary>
		///   Composes the specified instance and try to satisfy imports,
		///   and applies default design time values if in design time.
		/// </summary>
		/// <param name = "instance">The instance to compose.</param>
		/// <param name = "designTimeDefaultValues">if set to <c>true</c> [design time default values].</param>
		public static void Compose( object instance, bool designTimeDefaultValues )
		{
			_sLocator.Compose( instance );

			if ( designTimeDefaultValues )
			{
				InsertDesignDefaults( instance );
			}
		}

		private static void InsertDesignDefaults( object instance )
		{
			if ( !FrameworkConfiguration.IsInDesignMode ) return;

			var properties = instance.GetType().GetProperties();
			foreach ( var propertyInfo in properties )
			{
				var attribs = propertyInfo.GetCustomAttributes( typeof( DesignTimeValueAttribute ), false );
				if ( attribs.Length > 0 )
				{
					var attrib = ( DesignTimeValueAttribute )attribs.First();
					propertyInfo.SetValue( instance, attrib.DefaultValue, null );
				}
			}
		}
	}
}