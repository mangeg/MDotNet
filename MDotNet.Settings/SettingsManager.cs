namespace MDotNet.Settings
{
	using System;
	using System.Collections.Generic;
	using Targets;

	/// <summary>
	/// Settings manager that can load and save settings with different targets.
	/// </summary>
	public static class SettingsManager
	{
		private static readonly Dictionary<Type, ITarget> _sTargets = new Dictionary<Type, ITarget>();
		private static readonly Dictionary<Type, SettingsBase> _sObjects = new Dictionary<Type, SettingsBase>();

		/// <summary>
		/// Sets the target for the specified type.
		/// </summary>
		/// <typeparam name="T">Type of the settings to set the target for.</typeparam>
		/// <param name="target">The target.</param>
		/// <returns></returns>
		public static ITarget SetTarget<T>( ITarget target ) where T : SettingsBase
		{
			if ( target == null )
				throw new ArgumentNullException( "target" );

			var type = typeof( T );
			if ( _sTargets.ContainsKey( type ) )
			{
				// TODO: Log Error that target is allready set.
				return target;
			}
			_sTargets[ type ] = target;

			return target;
		}

		/// <summary>
		/// Saves the settings.
		/// </summary>
		/// <typeparam name="T">The type of settings to save.</typeparam>
		public static void Save<T>() where T : SettingsBase
		{
			var type = typeof( T );

			if ( !_sTargets.ContainsKey( type ) || !_sObjects.ContainsKey( type ) )
			{
				// TODO: Log error with no target to save to.
				return;
			}

			var target = _sTargets[ type ];
			target.Value = _sObjects[ type ];
			target.Save();
		}

		/// <summary>
		/// Loads the settings.
		/// </summary>
		/// <typeparam name="T">The type of settings to load.</typeparam>
		public static void Load<T>() where T : SettingsBase
		{
			var type = typeof( T );
			if ( !_sTargets.ContainsKey( type ) )
			{
				// TODO: Log error with not target set to load from.
				return;
			}
			var target = _sTargets[ type ];
			target.Load();
			if ( target.Value != null )
			{
				if ( !_sObjects.ContainsKey( type ) )
				{
					_sObjects.Add( type, ( T )target.Value );
				}
				else
				{
					_sObjects[ type ] = ( T )target.Value;
				}
			}
		}

		/// <summary>
		/// Gets the specific settings.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Get<T>() where T : SettingsBase
		{
			var type = typeof( T );
			if ( !_sObjects.ContainsKey( type ) )
			{
				var newSettings = ( T )Activator.CreateInstance<T>();
				_sObjects.Add( type, newSettings );
			}

			return ( T )_sObjects[ type ];
		}
	}
}