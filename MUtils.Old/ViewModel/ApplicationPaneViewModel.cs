namespace MUtils.ViewModel
{
	using System;
	using System.ComponentModel.Composition;
	using System.Windows;
	using MEFedMVVM.Services.Contracts;
	using MEFedMVVM.ViewModelLocator;
	using Service.Contracts;

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportViewModel( "ApplicationPaneVM" )]
	internal class ApplicationPaneViewModel : IAvalonDockViewModel, IContextAware
	{
		private const string _paneName = "ApplicationsPane";
		private const String _paneTitle = "Applications";
		private const String _paneTooltip = "Applications Pane";

		private readonly ILayoutContentService _layout;
		private FrameworkElement _view;

		[ImportingConstructor]
		public ApplicationPaneViewModel( ILayoutContentService layout ) { _layout = layout; }

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