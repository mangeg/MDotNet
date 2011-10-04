namespace MUtils
{
	using System.Windows;
	using MDotNet.Settings;
	using MDotNet.Settings.Targets;
	using Settings;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			var target = new XmlTarget<MainSettings>();
			target.Filename = "MainSettings.xml";
			SettingsManager.SetTarget<MainSettings>( target );
			SettingsManager.Load<MainSettings>();

			MainWindow = new MainWindow();
			MainWindow.Title = "MUtils";
			MainWindow.Show();
		}

		protected override void OnExit( ExitEventArgs e )
		{
			SettingsManager.Save<MainSettings>();

			base.OnExit( e );
		}
	}
}