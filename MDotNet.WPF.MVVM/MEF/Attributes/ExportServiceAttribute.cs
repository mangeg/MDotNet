namespace MDotNet.WPF.MVVM.MEF.Attributes
{
	using System;
	using System.ComponentModel.Composition;

	public enum ServiceType : short
	{
		Runtime,
		DesignTime,
		Both,
	}

	[MetadataAttribute]
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false )]
	public class ExportServiceAttribute : ExportAttribute, IExportServiceMetadata
	{
		public ExportServiceAttribute( Type contractType, ServiceType isDesignTimeService )
			: base( contractType )
		{
			ServiceContract = contractType;
			IsDesignTimeService = isDesignTimeService;
			IsDefault = false;
		}

		public ExportServiceAttribute( Type contractType )
			: base( contractType )
		{
			ServiceContract = contractType;
			IsDefault = false;
		}

		#region IExportServiceMetadata Members

		public Type ServiceContract { get; set; }

		public bool IsDefault { get; set; }

		public ServiceType IsDesignTimeService { get; set; }

		#endregion
	}
}