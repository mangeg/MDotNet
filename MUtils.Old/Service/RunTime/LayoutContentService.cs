namespace MUtils.Service.RunTime
{
	using System.Collections.ObjectModel;
	using System.ComponentModel.Composition;
	using System.Windows.Data;
	using Contracts;
	using MEFedMVVM.Common;
	using MEFedMVVM.ViewModelLocator;

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( ServiceType.Both, typeof( ILayoutContentService ) )]
	internal class LayoutContentService : NotifyPropertyChangedBase, ILayoutContentService
	{
		private IAvalonDockViewModel _activeDocument;
		private IAvalonDockViewModel _activePane;

		public LayoutContentService()
		{
			Panes = new ObservableCollection<IAvalonDockViewModel>();
			Documents = new ObservableCollection<IAvalonDockViewModel>();
		}

		#region ILayoutContentService Members

		public AvalonDockHost Host { get; private set; }

		public ObservableCollection<IAvalonDockViewModel> Panes { get; private set; }
		public ObservableCollection<IAvalonDockViewModel> Documents { get; private set; }

		public IAvalonDockViewModel ActivePane
		{
			get { return _activePane; }
			set
			{
				_activePane = value;
				OnPropertyChanged( () => ActivePane );
			}
		}

		public IAvalonDockViewModel ActiveDocument
		{
			get { return _activeDocument; }
			set
			{
				_activeDocument = value;
				OnPropertyChanged( () => ActiveDocument );
			}
		}

		public void Initialize( AvalonDockHost host )
		{
			Host = host;
			Host.SetBinding( AvalonDockHost.DocumentsProperty, new Binding( "Documents" ) { Source = this } );
			Host.SetBinding( AvalonDockHost.PanesProperty, new Binding( "Panes" ) { Source = this } );
			Host.SetBinding( AvalonDockHost.ActivePaneProperty, new Binding( "ActivePane" ) { Source = this } );
			Host.SetBinding( AvalonDockHost.ActiveDocumentProperty, new Binding( "ActiveDocument" ) { Source = this } );
		}

		#endregion
	}
}