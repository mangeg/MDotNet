namespace MUtils.ViewModels
{
	using System.ComponentModel.Composition;
	using System.Windows;
	using MDotNet.WPF.MVVM.MEF.Attributes;
	using Services;
	using Services.Contracts;

	[ExportViewModel]
	public class MenuViewModel
	{
		private IDocking _docking;

		[ImportingConstructor]
		public MenuViewModel( IDocking docking )
		{
			_docking = docking;
		}
	}
}