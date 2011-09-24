using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using MEFedMVVM.ViewModelLocator;
using MUtils.Service.Contracts;
using MUtils.ViewModel;

namespace MUtils.Service.DesignTime
{
	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( ServiceType.DesignTime, typeof( ILayoutContentService ) )]
	public class LayoutContentDesignTime : ILayoutContentService
	{
		private AvalonDockHost _host;
		private ObservableCollection<IAvalonDockViewModel> _panes;
		private ObservableCollection<IAvalonDockViewModel> _documents;

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

		public LayoutContentDesignTime()
		{
			_panes = new ObservableCollection<IAvalonDockViewModel>();
			_documents = new ObservableCollection<IAvalonDockViewModel>();
		}

		public void Initialize( AvalonDockHost host )
		{
			_host = host;

			new PropertyPaneViewModel( this );
		}
	}
}
