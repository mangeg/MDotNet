namespace MUtils.ViewModels
{
	using System.ComponentModel.Composition;
	using Caliburn.Micro;

	[Export( typeof( MenuViewModel ) )]
	public class MenuViewModel : PropertyChangedBase
	{
		public MenuViewModel()
		{
		}
	}
}
