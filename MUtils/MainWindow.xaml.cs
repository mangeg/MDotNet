using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvalonDock;
using MDotNet.Settings;
using MUtils.Settings;

namespace MUtils
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const String _layoutFile = @"Layout.xml";

		public MainWindow()
		{
			InitializeComponent();
			var settings = SettingsManager.Get<MainSettings>();
			WindowState = settings.WindowState;
			Width = settings.WindowSize.Width;
			Height = settings.WindowSize.Height;
			Left = settings.StartLocation.X;
			Top = settings.StartLocation.Y;

			DockMngr.DeserializationCallback = ( a, b ) =>
			{

			};
		}

		private void DockMngr_Loaded( object sender, RoutedEventArgs e )
		{
			if ( File.Exists( _layoutFile ) )
			{
				DockMngr.RestoreLayout( _layoutFile );
			}
		}

		protected override void OnClosing( System.ComponentModel.CancelEventArgs e )
		{
			DockMngr.SaveLayout( _layoutFile );

			var settings = SettingsManager.Get<MainSettings>();
			settings.WindowState = WindowState;
			settings.WindowSize = new Size( Width, Height );
			settings.StartLocation = new Point( Left, Top );
		}
	}
}
