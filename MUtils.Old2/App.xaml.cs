namespace MUtils
{
	using System.Windows;
	using MDotNet.Logging;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			MDotNet.Logging.LogManager.Initialize( NLogManager.DefaultNLogLocator );
			Caliburn.Micro.LogManager.GetLog = ( type ) =>
				new Logging.CaliburnLogWrapper( MDotNet.Logging.LogManager.GetLog( type ) );
		}
	}
}
