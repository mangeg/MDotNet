namespace MUtils.Service.Contracts
{
	using System.ComponentModel.Composition;
	using MEFedMVVM.ViewModelLocator;

	public interface IPropertyViewService
	{
	}

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( ServiceType.Both, typeof( IPropertyViewService ) )]
	internal class PropertyViewService : IPropertyViewService
	{
		public PropertyViewService() { }
	}
}