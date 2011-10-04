namespace MUtils.ViewModels
{
	using System.Collections.ObjectModel;
	using MDotNet.WPF.MVVM;
	using MDotNet.WPF.MVVM.MEF.Attributes;
	using MDotNet.WPF.MVVM.ViewModel;

	[ExportViewModel]
	public class ApplicationsPaneViewModel : NotifyPropertyChangedBase, IAvalonDockViewModel
	{
		private bool _isPaneVisible;

		public ApplicationsPaneViewModel()
		{
			IoC.Compose( this );
			ApplicationList = new ObservableCollection<object>();
		}

		public ObservableCollection<object> ApplicationList { get; set; }

		#region IAvalonDockViewModel Members
		public string PaneTitle
		{
			get { return "Applications"; }
		}
		public string PaneName
		{
			get { return "PaneApplications"; }
		}
		public bool IsPaneVisible
		{
			get { return _isPaneVisible; }
			set
			{
				if ( value == _isPaneVisible ) return;
				_isPaneVisible = value;
				NotifyPropertyChange( () => IsPaneVisible );
			}
		}
		#endregion
	}
}