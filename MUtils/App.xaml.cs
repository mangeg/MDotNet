using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MDotNet.Settings;
using MDotNet.Settings.Targets;
using MUtils.Settings;

namespace MUtils
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
		}

		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			var target = new XmlTarget<MainSettings>();
			target.Filename = "MainSettings.xml";
			SettingsManager.SetTarget<MainSettings>( target );
			SettingsManager.Load<MainSettings>();

			var settings = SettingsManager.Get<MainSettings>();
			MainWindow = new MainWindow();

			MainWindow.Title = "MUtils";
			MainWindow.WindowState = settings.WindowState;
			MainWindow.Width = settings.WindowSize.Width;
			MainWindow.Height = settings.WindowSize.Height;
			MainWindow.Left = settings.StartLocation.X;
			MainWindow.Top = settings.StartLocation.Y;
			MainWindow.Show();
		}

		protected override void OnExit( ExitEventArgs e )
		{
			SettingsManager.Save<MainSettings>();

			base.OnExit( e );
		}
	}
}
