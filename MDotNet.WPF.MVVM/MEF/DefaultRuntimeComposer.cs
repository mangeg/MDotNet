namespace MDotNet.WPF.MVVM.MEF
{
	using System.ComponentModel.Composition.Hosting;
	using System.ComponentModel.Composition.Primitives;
	using System.Reflection;
	using Service;
	using Service.Contracts;

	public class DefaultRuntimeComposer : IComposer
	{
		#region IComposer Members

		public ComposablePartCatalog InitializeContainer()
		{
			//var catalog = new AggregateCatalog( new AssemblyCatalog( Assembly.GetEntryAssembly() ), new AssemblyCatalog( Assembly.GetExecutingAssembly() ) );
			var catalog = new AggregateCatalog( new AssemblyCatalog( Assembly.GetEntryAssembly() ) );
			return catalog;
		}

		#endregion
	}
}