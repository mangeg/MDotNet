namespace MUtils
{
	using System.ComponentModel.Composition.Hosting;
	using System.IO;
	using MDotNet.Settings;
	using MDotNet.Settings.Targets;
	using MDotNet.WPF.MVVM.ApplicationModel;
	using MDotNet.WPF.MVVM.MEF;
	using Settings;
	using ViewModels;

	public class AppBootstrapper : Bootstrapper<ShellViewModel>
	{
		public AppBootstrapper()
		{
			SettingsManager.SetTarget<MainSettings>( new XmlTarget<MainSettings>( "Settings.xml" ) );
			SettingsManager.Load<MainSettings>();
		}

		public override void OnExit( object sender, System.EventArgs e )
		{
			base.OnExit( sender, e );

			SettingsManager.Save<MainSettings>();
		}

		public override void Configure()
		{
			if ( Directory.Exists( "Extensions" ) )
			{
				var directoryCatalog = new DirectoryCatalog( "Extensions" );
				var directoryProvider = new MvvmExportProvider( MefMvvmCatalog.Create( directoryCatalog ) );
				Container.ExtraProviders.Add( directoryProvider );
			}
		}
	}
}