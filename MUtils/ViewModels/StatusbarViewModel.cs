namespace MUtils.ViewModels
{
	using System;
	using MDotNet.WPF.MVVM;
	using MDotNet.WPF.MVVM.MEF.Attributes;
	using MDotNet.WPF.MVVM.ViewModel;

	[ExportViewModel]
	public class StatusbarViewModel : NotifyPropertyChangedBase 
	{
		private String _lastMessage = String.Empty;

		public String LastMessage
		{
			get { return _lastMessage; }
			set
			{
				if ( value == _lastMessage ) return;

				_lastMessage = value;
				NotifyPropertyChange( () => LastMessage );
			}
		}

		public StatusbarViewModel()
		{
			LastMessage = "First message!";
		}
	}
}