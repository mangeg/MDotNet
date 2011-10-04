namespace MUtils.Extention
{
	using System;
	using System.ComponentModel.Composition;

	public interface IExtension
	{
		void Test();
	}

	[MetadataAttribute]
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
	public class ExtensionAttribute : ExportAttribute, IExtensionMetadata
	{
		public ExtensionAttribute( String extensionName )
			: base( typeof( IExtension ) )
		{
			ExtensionName = extensionName;
		}

		#region IExtensionMetadata Members
		public string ExtensionName { get; private set; }
		#endregion
	}

	public interface IExtensionMetadata
	{
		String ExtensionName { get; }
	}
}