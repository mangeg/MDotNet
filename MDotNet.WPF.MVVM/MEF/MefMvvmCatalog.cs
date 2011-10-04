namespace MDotNet.WPF.MVVM.MEF
{
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Primitives;
	using System.Linq;
	using System.Reflection;
	using Attributes;
	using MDotNet.WPF.MVVM.ApplicationModel;

	/// <summary>
	/// A special <see cref="ComposablePartCatalog"/> that handles design time for services.
	/// </summary>
	public class MefMvvmCatalog : ComposablePartCatalog
	{
		private readonly ComposablePartCatalog _inner;
		private readonly IQueryable<ComposablePartDefinition> _query;

		/// <summary>
		/// Initializes a new instance of the <see cref="MefMvvmCatalog"/> class.
		/// </summary>
		/// <param name="inner">The inner.</param>
		/// <param name="designTime">if set to <c>true</c> [design time].</param>
		public MefMvvmCatalog( ComposablePartCatalog inner, bool designTime )
		{
			_inner = inner;
			_query = _inner.Parts.Where( p => p.ExportDefinitions.Any( ed => designTime
			                                                                 	? CheckMetadata( ed, true )
			                                                                 	: CheckMetadata( ed, false ) )
				);
		}

		/// <summary>
		/// Gets the part definitions that are contained in the catalog.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> contained in the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/>.
		/// </returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/> object has been disposed of.</exception>
		public override IQueryable<ComposablePartDefinition> Parts
		{
			get { return _query; }
		}

		/// <summary>
		/// Creates a <see cref="MefMvvmCatalog"/>.
		/// </summary>
		/// <param name="inner">The inner catalog.</param>
		/// <returns>The new instnace of <see cref="MefMvvmCatalog"/></returns>
		public static MefMvvmCatalog Create( ComposablePartCatalog inner ) { return new MefMvvmCatalog( inner, FrameworkConfiguration.IsInDesignMode ); }

		private T GetTypedMetadata<T>( IDictionary<string, object> metadata )
		{
			try
			{
				var typedMetadata = AttributedModelServices.GetMetadataView<T>( metadata );
				return typedMetadata;
			}
			catch ( TargetInvocationException )
			{
			}

			return default( T );
		}

		private bool CheckMetadata( ExportDefinition metadata, bool checkDesignTime )
		{
			if ( !metadata.Metadata.ContainsKey( "IsDesignTimeService" ) ) return true;
			var typedMetadata = GetTypedMetadata<IExportServiceMetadata>( metadata.Metadata );
			if ( typedMetadata != null )
			{
				if ( checkDesignTime )
					return typedMetadata.IsDesignTimeService == ServiceType.Both ||
					       typedMetadata.IsDesignTimeService == ServiceType.DesignTime;

				return typedMetadata.IsDesignTimeService == ServiceType.Both ||
				       typedMetadata.IsDesignTimeService == ServiceType.Runtime;
			}

			return true;
		}
	}
}