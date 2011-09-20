using System;
using System.ComponentModel.Composition;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.ViewModelLocator;

namespace MUtils.ViewModel
{
	[ExportViewModel("MainVM")]
	public class MainViewModel 
	{
		[ImportingConstructor]
		public MainViewModel(IMediator mediator)
		{
			mediator.NotifyColleagues<String>( MediatorMessages.StatusMessage, "MainViewModel loaded" );
		}
	}
}
