namespace MUtils.ViewModel
{
	using System;
	using System.ComponentModel.Composition;
	using System.Windows;
	using MEFedMVVM.Services.Contracts;
	using MEFedMVVM.ViewModelLocator;
	using Service.Contracts;

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportViewModel( "PropertyPaneVM" )]
	internal class PropertyPaneViewModel : IAvalonDockViewModel, IContextAware
	{
		private const string _paneName = "PropertiesPane";
		private const String _paneTitle = "Properties";
		private const String _paneTooltip = "Prioperties Pane";

		private readonly ILayoutContentService _layout;
		private FrameworkElement _view;

		[ImportingConstructor]
		public PropertyPaneViewModel( ILayoutContentService layout ) { _layout = layout; }

		public String PaneTitle
		{
			get { return _paneTitle; }
		}

		public String PaneTooltip
		{
			get { return _paneTooltip; }
		}

		#region IAvalonDockViewModel Members

		FrameworkElement IAvalonDockViewModel.View
		{
			get { return _view; }
		}

		public string PanelName
		{
			get { return _paneName; }
		}

		#endregion

		#region IContextAware Members

		public void InjectContext( object context )
		{
			_view = context as FrameworkElement;
			_layout.Panes.Add( this );
		}

		#endregion
	}
}