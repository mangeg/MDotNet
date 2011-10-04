namespace MUtils.Settings
{
	using System.Windows;
	using MDotNet.Settings;

	public class MainSettings : SettingsBase
	{
		public MainSettings()
		{
			WindowState = WindowState.Normal;
			WindowSize = new Size( 800, 600 );
			StartLocation = new Point( 100, 100 );
		}

		public WindowState WindowState { get; set; }
		public Size WindowSize { get; set; }
		public Point StartLocation { get; set; }
	}
}