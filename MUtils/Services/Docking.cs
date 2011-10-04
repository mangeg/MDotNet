namespace MUtils.Services
{
	using System.Collections.ObjectModel;
	using System.Windows.Data;
	using Contracts;
	using MDotNet.WPF.MVVM.MEF.Attributes;
	using MDotNet.WPF.MVVM.ViewModel;

	[ExportService( typeof( IDocking ), ServiceType.Both )]
	public class Docking : NotifyPropertyChangedBase, IDocking
	{
		private IAvalonDockViewModel _activeDocument;
		private IAvalonDockViewModel _activePane;
		private ObservableCollection<IAvalonDockViewModel> _documents;
		private AvalonDockHost _host;
		private ObservableCollection<IAvalonDockViewModel> _panes;

		public AvalonDockHost Host
		{
			get { return _host; }
			set { _host = value; }
		}

		#region IDocking Members
		public IAvalonDockViewModel ActivePane
		{
			get { return _activePane; }
			set
			{
				_activePane = value;
				NotifyPropertyChange( () => ActivePane );
			}
		}
		public IAvalonDockViewModel ActiveDocument
		{
			get { return _activeDocument; }
			set
			{
				_activeDocument = value;
				NotifyPropertyChange( () => ActiveDocument );
			}
		}
		public ObservableCollection<IAvalonDockViewModel> Panes
		{
			get { return _panes; }
			private set { _panes = value; }
		}
		public ObservableCollection<IAvalonDockViewModel> Documents
		{
			get { return _documents; }
			private set { _documents = value; }
		}

		public void Initialize( AvalonDockHost host )
		{
			_documents = new ObservableCollection<IAvalonDockViewModel>();
			_panes = new ObservableCollection<IAvalonDockViewModel>();

			_host = host;
			_host.DataContext = this;

			_host.SetBinding( AvalonDockHost.DocumentsProperty, new Binding( "Documents" ) );
			_host.SetBinding( AvalonDockHost.PanesProperty, new Binding( "Panes" ) );
			_host.SetBinding( AvalonDockHost.ActivePaneProperty, new Binding( "ActivePane" ) );
			_host.SetBinding( AvalonDockHost.ActiveDocumentProperty, new Binding( "ActiveDocument" ) );
		}
		#endregion
	}
}