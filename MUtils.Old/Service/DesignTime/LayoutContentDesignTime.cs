namespace MUtils.Service.DesignTime
{
	using System.Collections.ObjectModel;
	using System.ComponentModel.Composition;
	using Contracts;
	using MEFedMVVM.ViewModelLocator;
	using ViewModel;

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( ServiceType.DesignTime, typeof( ILayoutContentService ) )]
	public class LayoutContentDesignTime : ILayoutContentService
	{
		private ObservableCollection<IAvalonDockViewModel> _documents;
		private AvalonDockHost _host;
		private ObservableCollection<IAvalonDockViewModel> _panes;

		public LayoutContentDesignTime()
		{
			_panes = new ObservableCollection<IAvalonDockViewModel>();
			_documents = new ObservableCollection<IAvalonDockViewModel>();
		}

		#region ILayoutContentService Members

		public AvalonDockHost Host
		{
			get { return _host; }
		}

		public ObservableCollection<IAvalonDockViewModel> Panes
		{
			get { return _panes; }
		}

		public ObservableCollection<IAvalonDockViewModel> Documents
		{
			get { return _documents; }
		}

		public IAvalonDockViewModel ActivePane
		{
			get { return null; }
		}

		public IAvalonDockViewModel ActiveDocument
		{
			get { return null; }
		}

		public void Initialize( AvalonDockHost host )
		{
			_host = host;

			new PropertyPaneViewModel( this );
		}

		#endregion
	}
}