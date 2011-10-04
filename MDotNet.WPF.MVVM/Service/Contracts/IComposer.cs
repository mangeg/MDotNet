namespace MDotNet.WPF.MVVM.Service.Contracts
{
	using System.ComponentModel.Composition.Primitives;

	public interface IComposer
	{
		ComposablePartCatalog InitializeContainer();
	}
}