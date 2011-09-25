namespace MUtils.ViewModels
{
	using System.ComponentModel.Composition;
	using Caliburn.Micro;

	[Export(typeof (IShell))]
	public class ShellViewModel : PropertyChangedBase, IShell
	{
		[Import]
		public MenuViewModel Menu { get; set; }
		[Import]
		public StatusbarViewModel Statusbar { get; set; }

		public ShellViewModel()
		{
			
		}
	}
}

