namespace MDotNet.WPF.MVVM.MEF.Attributes
{
	using System;

	public interface IExportServiceMetadata
	{
		ServiceType IsDesignTimeService { get; }
		Type ServiceContract { get; }
		bool IsDefault { get; }
	}
}