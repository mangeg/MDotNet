namespace MUtils
{
	using System.Windows;
	using MDotNet.Settings;
	using Settings;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var settings = SettingsManager.Get<MainSettings>();
			WindowState = settings.WindowState;
			Width = settings.WindowSize.Width;
			Height = settings.WindowSize.Height;
			Left = settings.StartLocation.X;
			Top = settings.StartLocation.Y;
		}

		protected override void OnClosing( System.ComponentModel.CancelEventArgs e )
		{
			var settings = SettingsManager.Get<MainSettings>();
			settings.WindowState = WindowState;
			settings.WindowSize = new Size( Width, Height );
			settings.StartLocation = new Point( Left, Top );
		}
	}
}