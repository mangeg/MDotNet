using System;
using System.ComponentModel.Composition;
using System.Windows;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.ViewModelLocator;
using MUtils.Service.Contracts;

namespace MUtils.ViewModel
{
	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportViewModel( "PropertyPaneVM" )]
	class PropertyPaneViewModel : IAvalonDockViewModel, IContextAware
	{
		private const string _paneName = "PropertiesPane";
		private const String _paneTitle = "Properties";
		private const String _paneTooltip = "Prioperties Pane";

		private readonly ILayoutContentService _layout;
		private FrameworkElement _view;
		
		FrameworkElement IAvalonDockViewModel.View
		{
			get { return _view; }
		}

		public string PanelName
		{
			get { return _paneName; }
		}

		public String PaneTitle { get { return _paneTitle; } }
		public String PaneTooltip { get { return _paneTooltip; } }

		[ImportingConstructor]
		public PropertyPaneViewModel( ILayoutContentService layout )
		{
			_layout = layout;
		}

		public void InjectContext( object context )
		{
			_view = context as FrameworkElement;
			_layout.Panes.Add( this );
		}
	}
}
