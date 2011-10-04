namespace MUtils
{
	using System.ComponentModel.Composition;
	using System.Windows;
	using Extention;
	using MDotNet.Logging;
	using MDotNet.Logging.Wrappers.NLog;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App() { LogManager.SetDefaultLocator( new NLogLocator() ); }
	}

	[Extension("Test Extension")]
	public class TestExtension : IExtension
	{
		public void Test()
		{
		}
	}

	[Extension( "Test Extension2" )]
	public class TestExtension2 : IExtension
	{
		public void Test()
		{
		}
	}
}