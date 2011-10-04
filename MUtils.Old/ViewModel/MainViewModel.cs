namespace MUtils.ViewModel
{
	using System;
	using System.ComponentModel.Composition;
	using System.Windows.Input;
	using MEFedMVVM.Common;
	using MEFedMVVM.Services.Contracts;
	using MEFedMVVM.ViewModelLocator;
	using Service.Contracts;
	using View;

	[PartCreationPolicy( CreationPolicy.NonShared )]
	[ExportViewModel( "MainVM" )]
	public class MainViewModel : NotifyPropertyChangedBase
	{
		private const String LayoutFile = @"Layout.xml";

		[ImportingConstructor]
		public MainViewModel( IMediator mediator, ILayoutContentService layout )
		{
			ClosingCmd = new DelegateCommand<object>( ( p ) => Host.DockingManager.SaveLayout( LayoutFile ) );

			Host = new AvalonDockHost();
			Host.Name = "AvalonDockHost";
			Host.AvalonDockLoaded += new EventHandler<EventArgs>( Host_AvalonDockLoaded );

			layout.Initialize( Host );

			new PropertyPane();
			new ApplicationPane();
		}

		public AvalonDockHost Host { get; private set; }
		public ICommand ClosingCmd { get; private set; }

		private void Host_AvalonDockLoaded( object sender, EventArgs e ) { Host.DockingManager.RestoreLayout( LayoutFile ); }
	}
}