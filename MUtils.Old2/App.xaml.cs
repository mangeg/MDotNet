namespace MUtils
{
	using System.Windows;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			Caliburn.Micro.LogManager.GetLog = ( type ) =>
			                                   new Logging.CaliburnLogWrapper( MDotNet.Logging.LogManager.GetLog( type ) );
		}
	}
}