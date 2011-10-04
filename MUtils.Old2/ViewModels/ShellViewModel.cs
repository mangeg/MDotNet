namespace MUtils.ViewModels
{
	using System.ComponentModel.Composition;
	using Caliburn.Micro;

	[Export( typeof( IShell ) )]
	public class ShellViewModel : PropertyChangedBase, IShell
	{
		public ShellViewModel() { }

		[Import]
		public MenuViewModel Menu { get; set; }

		[Import]
		public StatusbarViewModel Statusbar { get; set; }
	}
}