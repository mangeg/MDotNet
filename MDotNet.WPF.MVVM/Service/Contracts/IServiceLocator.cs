namespace MDotNet.WPF.MVVM.Service.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition.Hosting;

	public interface IServiceLocator
	{
		object GetInstance( Type serviceType );
		object GetInstance( String contractName );
		IEnumerable<object> GetAllInstances( Type serviceType );
		IEnumerable<object> GetAllInstances( String contractName );
		void Compose( object instance );
		void Initialize();

		IList<ExportProvider> ExtraProviders { get; }
	}
}