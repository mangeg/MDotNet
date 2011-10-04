namespace MDotNet.WPF.MVVM.Service.Contracts
{
	public interface IWindowManager
	{
		void ShowWindow( object rootModel, bool isDialog, object context );
	}
}