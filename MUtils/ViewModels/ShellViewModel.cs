namespace MUtils.ViewModels
{
	using System.ComponentModel.Composition;
	using System.Windows;
	using MDotNet.Settings;
	using MDotNet.WPF.MVVM.MEF.Attributes;
	using MDotNet.WPF.MVVM.Service.Contracts;
	using MDotNet.WPF.MVVM.View;
	using Settings;

	[ExportViewModel]
	public class ShellViewModel : IViewAware
	{
		[Import]
		public MenuViewModel Menu { get; set; }
		[Import]
		public StatusbarViewModel Statusbar { get; set; }
		[Import]
		public MainContentViewModel MainContent { get; set; }


		#region IViewAware Members

		public void AttachView( object view )
		{
			var wnd = view as Window;
			if ( wnd == null ) return;

			SettingsManager.Get<MainSettings>().MainWindowSettings.ApplySettings( wnd );
			wnd.Closing += MainWindowClosing;
		}

		#endregion

		private void MainWindowClosing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			var wnd = sender as Window;
			if ( wnd == null ) return;

			var s = SettingsManager.Get<MainSettings>().MainWindowSettings;
			s.WindowState = wnd.WindowState;
			s.WindowSize = new Size( wnd.Width, wnd.Height );
			s.StartLocation = new Point( wnd.Left, wnd.Top );
		}
	}
}