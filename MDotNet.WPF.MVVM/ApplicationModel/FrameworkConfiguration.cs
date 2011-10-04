namespace MDotNet.WPF.MVVM.ApplicationModel
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Windows;

	public static class FrameworkConfiguration
	{
		private static bool? _sIsInDesignMode;

		public static bool IsInDesignMode
		{
			get
			{
#if SILVERLIGHT
				_sIsInDesignMode = DesignerProperties.IsDesignTool;
#else
				if ( !_sIsInDesignMode.HasValue )
				{
					var prop = DesignerProperties.IsInDesignModeProperty;
					_sIsInDesignMode = ( bool )DependencyPropertyDescriptor.FromProperty( prop, typeof( FrameworkElement ) )
					                           	.Metadata.DefaultValue;

					if ( !_sIsInDesignMode.GetValueOrDefault( false )
					     && Process.GetCurrentProcess()
					        	.ProcessName.StartsWith( "devenv", StringComparison.Ordinal ) )
						_sIsInDesignMode = true;
				}
#endif
				return _sIsInDesignMode.Value;
			}
			set { _sIsInDesignMode = value; }
		}

		public static void Initialize() { }
	}
}