namespace MUtils.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel.Composition;
	using MEFedMVVM.Common;
	using MEFedMVVM.Services.CommonServices;
	using MEFedMVVM.Services.Contracts;
	using MEFedMVVM.ViewModelLocator;
	using Model;

	[ExportViewModel( "StatusbarVM" )]
	public class StatusbarViewModel : NotifyPropertyChangedBase
	{
		private String _lastString;
		private IMediator _mediator;
		private ObservableCollection<StatusMessage> _messages = new ObservableCollection<StatusMessage>();

		[ImportingConstructor]
		public StatusbarViewModel( IMediator mediator )
		{
			_mediator = mediator;

			mediator.Register( this );
		}

		public String LastMessage
		{
			get { return _lastString; }
			set
			{
				if ( value == _lastString ) return;
				_lastString = value;
				OnPropertyChanged( () => LastMessage );
			}
		}

		public IList<StatusMessage> AllMessages
		{
			get { return _messages; }
		}

		[MediatorMessageSink( MediatorMessages.StatusMessage, ParameterType = typeof( String ) )]
		public void OnStatusMessage( String message )
		{
			if ( !String.IsNullOrEmpty( LastMessage ) )
				_messages.Add( new StatusMessage() { Message = LastMessage } );

			if ( _messages.Count > 10 )
				_messages.RemoveAt( 0 );

			LastMessage = message;
		}
	}
}