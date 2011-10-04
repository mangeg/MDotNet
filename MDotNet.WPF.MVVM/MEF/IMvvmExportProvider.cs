namespace MDotNet.WPF.MVVM.MEF
{
	using System.ComponentModel.Composition.Hosting;

	public interface IMvvmExportProvider
	{
		ExportProvider SourceProvider { get; set; }
	}
}