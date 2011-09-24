using System.ComponentModel.Composition;
using System.Windows.Input;
using MEFedMVVM.Common;
using MEFedMVVM.Services.Contracts;
using MEFedMVVM.ViewModelLocator;

namespace MUtils.ViewModel
{
	[ExportViewModel( "MenuVM" )]
	public class MenuViewModel
	{
		private ITaskDialog _dialog;
		
		public ICommand ExitCommand { get; private set; }

		[ImportingConstructor]
		public MenuViewModel( ITaskDialog dialog )
		{
			_dialog = dialog;
			ExitCommand = new DelegateCommand<object>( x =>
			{
				var res = _dialog.Show( "Are you sure?", "Are you sure you want to exit the application?", "Exit application", TaskDialogButtons.OK | TaskDialogButtons.Cancel );
				if ( res == TaskDialogResult.OK )
					App.Current.MainWindow.Close();
			} );
		}
	}
}
