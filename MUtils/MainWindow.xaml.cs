using System;
using System.Collections.Generic;
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
using MDotNet.Settings;
using MUtils.Settings;

namespace MUtils
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
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
