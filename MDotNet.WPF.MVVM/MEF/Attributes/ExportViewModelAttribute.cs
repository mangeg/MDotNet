namespace MDotNet.WPF.MVVM.MEF.Attributes
{
	using System;
	using System.ComponentModel.Composition;

	[MetadataAttribute]
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
	public class ExportViewModelAttribute : ExportAttribute, IExportViewModelMetadata
	{
	}

	public interface IExportViewModelMetadata
	{
	}
}