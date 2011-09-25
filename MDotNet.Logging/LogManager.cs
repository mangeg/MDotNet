namespace MDotNet.Logging
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	/// <summary>
	/// LogManager for ILog
	/// </summary>
	public static class LogManager
	{
		private static ILogLocator _sDefaultLocator = new NullLocator();
		private static Dictionary<String, ILogLocator> _sLocators = new Dictionary<string, ILogLocator>();

		/// <summary>
		/// Adds the keyed locator.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="locator">The locator.</param>
		public static void AddKeyedLocator( String key, ILogLocator locator )
		{
			_sLocators[ key ] = locator;
		}

		/// <summary>
		/// Removes the keyed locator.
		/// </summary>
		/// <param name="key">The key.</param>
		public static void RemoveKeyedLocator( String key )
		{
			if ( _sLocators.ContainsKey( key ) )
				_sLocators.Remove( key );
		}

		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static ILog GetLog( Type type )
		{
			return _sDefaultLocator.TypedLocator( type );
		}

		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static ILog GetLog<T>()
		{
			return GetLog( typeof( T ) );
		}

		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <returns></returns>
		public static ILog GetLog()
		{
#if SILVERLIGHT
			var frame = new StackFrame(1);
#else
			var frame = new StackFrame( 1, false );
#endif
			return GetLog( frame.GetMethod().DeclaringType );
		}

		/// <summary>
		/// Gets a named log.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The named <see cref="ILog"/></returns>
		public static ILog GetNamedLog( String name )
		{
			return _sDefaultLocator.NamedLocator( name );
		}

		/// <summary>
		/// Gets the log for a specific key.
		/// If the key is not registered the default will be returned.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="key">The key.</param>
		/// <returns>The typed <see cref="ILog"/> for the specified key</returns>
		public static ILog GetLog( Type type, String key )
		{
			ILogLocator locator;
			if ( _sLocators.TryGetValue( key, out locator ) )
			{
				return locator.TypedLocator( type );
			}
			return _sDefaultLocator.TypedLocator( type );
		}

		/// <summary>
		/// Gets the log for a specific key.
		/// If the key is not registered the default will be returned.
		/// </summary>
		/// <typeparam name="T">The type to get the log for</typeparam>
		/// <param name="key">The key.</param>
		/// <returns>The typed <see cref="ILog"/> for the specified key</returns>
		public static ILog GetLog<T>( String key )
		{
			return GetLog( typeof( T ), key );
		}

		/// <summary>
		/// Gets the log for a specific key.
		/// If the key is not registered the default will be returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The default type <see cref="ILog"/> for the specified key</returns>
		public static ILog GetLog( String key )
		{
#if SILVERLIGHT
			var frame = new StackFrame(1);
#else
			var frame = new StackFrame( 1, false );
#endif
			return GetLog( frame.GetMethod().DeclaringType, key );
		}

		/// <summary>
		/// Gets the named log for a specific key.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="key">The key.</param>
		/// <returns>The named <see cref="ILog"/> for the specified key.</returns>
		public static ILog GetNamedLog( String name, String key )
		{
			ILogLocator locator;
			if ( _sLocators.TryGetValue( key, out locator ) )
			{
				return locator.NamedLocator( name );
			}
			return _sDefaultLocator.NamedLocator( name );
		}
	}
}