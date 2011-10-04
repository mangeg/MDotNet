namespace MDotNet.WPF.MVVM.MEF
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Hosting;
	using System.Linq;
	using System.Reflection;
	using ApplicationModel;
	using Service.Contracts;

	public class MefLocator : IServiceLocator
	{
		private readonly List<ExportProvider> _extraProviders = new List<ExportProvider>();
		private CompositionContainer _container;

		#region IServiceLocator Members
		public IList<ExportProvider> ExtraProviders
		{
			get { return _extraProviders; }
		}

		public object GetInstance( Type serviceType )
		{
			var contract = AttributedModelServices.GetTypeIdentity( serviceType );
			return GetInstance( contract );
		}

		public object GetInstance( string contractName )
		{
			var exports = _container.GetExportedValues<object>( contractName );
			return exports.FirstOrDefault();
		}

		public IEnumerable<object> GetAllInstances( Type serviceType )
		{
			var contract = AttributedModelServices.GetTypeIdentity( serviceType );
			return GetAllInstances( contract );
		}

		public IEnumerable<object> GetAllInstances( string contractName )
		{
			var exports = _container.GetExportedValues<object>( contractName );
			return exports;
		}

		public void Compose( object instance )
		{
			_container.SatisfyImportsOnce( instance );
		}
		#endregion

		public void Initialize()
		{
			var composer = FrameworkConfiguration.IsInDesignMode
			               	? new DefaultDesignTimeComposer()
			               	: new DefaultRuntimeComposer() as IComposer;

			var providerList = new List<ExportProvider>();

			var instanceCatalog = composer.InitializeContainer();
			var defaultCatalog = new AggregateCatalog( new AssemblyCatalog( Assembly.GetExecutingAssembly() ) );

			var instanceProvider = new MvvmExportProvider( MefMvvmCatalog.Create( instanceCatalog ) );
			var defaulProvider = new MvvmExportProvider( MefMvvmCatalog.Create( defaultCatalog ) );

			providerList.AddRange( _extraProviders );
			providerList.Add( instanceProvider );
			providerList.Add( defaulProvider );

			var container = new CompositionContainer( providerList.ToArray() );
			foreach ( var provider in container.Providers.OfType<IMvvmExportProvider>() )
			{
				provider.SourceProvider = container;
			}

			_container = container;
		}
	}
}