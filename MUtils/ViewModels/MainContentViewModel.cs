namespace MUtils.ViewModels
{
	using System;
	using System.ComponentModel.Composition;
	using MDotNet.WPF.MVVM;
	using MDotNet.WPF.MVVM.MEF.Attributes;
	using MDotNet.WPF.MVVM.ViewModel;
	using Services.Contracts;

	[ExportViewModel]
	public class MainContentViewModel : NotifyPropertyChangedBase
	{
		private const String LayoutFile = @"Layout.xml";

		private readonly IDocking _docking;

		[ImportingConstructor]
		public MainContentViewModel( IDocking docking )
		{
			IoC.Compose( this );

			_docking = docking;

			Host = new AvalonDockHost { Name = "AvalonDockHost" };
			Host.AvalonDockLoaded += OnAvalondDockLoaded;
			Host.DocumentClosing += AvalonDockOnDocumentClosing;

			_docking.Initialize( Host );
			_docking.Panes.Add( ApplicationsPane );
		}

		[Import]
		private ApplicationsPaneViewModel ApplicationsPane { get; set; }

		public AvalonDockHost Host { get; private set; }

		private void AvalonDockOnDocumentClosing( object sender, DocumentClosingEventArgs e ) {}

		private void OnAvalondDockLoaded( object sender, EventArgs e )
		{
			//Host.DockingManager.RestoreLayout( LayoutFile );
		}
	}
}