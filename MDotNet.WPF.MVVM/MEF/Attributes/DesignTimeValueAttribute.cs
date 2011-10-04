namespace MDotNet.WPF.MVVM.MEF.Attributes
{
	using System;

	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false )]
	public class DesignTimeValueAttribute : Attribute
	{
		public DesignTimeValueAttribute( object defaultValue ) { DefaultValue = defaultValue; }
		public object DefaultValue { get; set; }
	}
}