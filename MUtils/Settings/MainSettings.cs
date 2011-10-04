namespace MUtils.Settings
{
	using System.Windows;
	using MDotNet.Settings;

	public class MainSettings : SettingsBase
	{
		public MainSettings()
		{
			MainWindowSettings = new WindowSettings();
		}

		public WindowSettings MainWindowSettings { get; set; }
	}

	public class WindowSettings
	{
		public WindowSettings()
		{
			WindowState = WindowState.Normal;
			WindowSize = new Size( 800, 600 );
			StartLocation = new Point( 100, 100 );
		}

		public WindowState WindowState { get; set; }
		public Size WindowSize { get; set; }
		public Point StartLocation { get; set; }

		public void ApplySettings(Window window)
		{
			window.WindowState = WindowState;
			window.WindowStartupLocation = WindowStartupLocation.Manual;
			window.Left = StartLocation.X;
			window.Top = StartLocation.Y;
			window.Width = WindowSize.Width;
			window.Height = WindowSize.Height;
		}
	}
}