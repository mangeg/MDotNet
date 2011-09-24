using System;
using System.ComponentModel.Composition;
using System.Windows;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.ViewModelLocator;
using MUtils.Service.Contracts;

namespace MUtils.ViewModel
{
	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportViewModel( "ApplicationPaneVM" )]
	class ApplicationPaneViewModel : IAvalonDockViewModel, IContextAware
	{
		private const string _paneName = "ApplicationsPane";
		private const String _paneTitle = "Applications";
		private const String _paneTooltip = "Applications Pane";

		private readonly ILayoutContentService _layout;
		private FrameworkElement _view;

		FrameworkElement IAvalonDockViewModel.View { get { return _view; } }

		public string PanelName { get { return _paneName; } }
		public String PaneTitle { get { return _paneTitle; } }
		public String PaneTooltip { get { return _paneTooltip; } }

		[ImportingConstructor]
		public ApplicationPaneViewModel( ILayoutContentService layout )
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