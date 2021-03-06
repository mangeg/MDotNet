﻿namespace MUtils.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel.Composition;
	using Caliburn.Micro;
	using Model;

	[Export( typeof( StatusbarViewModel ) )]
	public class StatusbarViewModel : PropertyChangedBase
	{
		private String _lastString;
		private ObservableCollection<StatusMessage> _messages = new ObservableCollection<StatusMessage>();

		[ImportingConstructor]
		public StatusbarViewModel() { }

		public String LastMessage
		{
			get { return _lastString; }
			set
			{
				if ( value == _lastString ) return;
				_lastString = value;
				NotifyOfPropertyChange( () => LastMessage );
			}
		}

		public IList<StatusMessage> AllMessages
		{
			get { return _messages; }
		}

		/*
		[MediatorMessageSink( MediatorMessages.StatusMessage, ParameterType = typeof( String ) )]
		public void OnStatusMessage( String message )
		{
			if ( !String.IsNullOrEmpty( LastMessage ) )
				_messages.Add( new StatusMessage() { Message = LastMessage } );

			if ( _messages.Count > 10 )
				_messages.RemoveAt( 0 );

			LastMessage = message;
		}
		 */
	}
}