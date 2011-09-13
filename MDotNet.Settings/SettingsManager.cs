using System;
using System.Collections.Generic;
using MDotNet.Settings.Targets;

namespace MDotNet.Settings
{
	public static class SettingsManager
	{
		private static readonly Dictionary<Type, ITarget> _sTargets = new Dictionary<Type, ITarget>();
		private static readonly Dictionary<Type, SettingsBase> _sObjects = new Dictionary<Type, SettingsBase>();

		public static TTarget AddTarget<TObject, TTarget>( TTarget target )
			where TTarget : ITarget
			where TObject : SettingsBase
		{
			var type = typeof( TObject );
			if ( !_sTargets.ContainsKey( type ) )
			{
				// TODO: Log Error that target is allready set.
				return target;
			}
			_sTargets[ type ] = target;

			return target;
		}
		public static void Save<T>() where T : SettingsBase
		{
			var type = typeof( T );

			if ( !_sTargets.ContainsKey( type ) )
			{
				// TODO: Log error with no target to save to.
			}

			var target = _sTargets[ type ];
			target.Value = _sObjects[ type ];
			target.Save();
		}
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
		}
	}
}
