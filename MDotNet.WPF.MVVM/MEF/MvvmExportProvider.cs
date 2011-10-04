namespace MDotNet.WPF.MVVM.MEF
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition.Hosting;
	using System.ComponentModel.Composition.Primitives;

	public class MvvmExportProvider : CatalogExportProvider, IMvvmExportProvider, IDisposable
	{
		public MvvmExportProvider( ComposablePartCatalog catalog )
			: base( catalog ) { }

		/// <summary>
		/// Gets all the exports that match the constraint defined by the specified definition.
		/// </summary>
		/// <returns>
		/// A collection that contains all the exports that match the specified condition.
		/// </returns>
		/// <param name="definition">The object that defines the conditions of the <see cref="T:System.ComponentModel.Composition.Primitives.Export"/> objects to return.</param><param name="atomicComposition">The transactional container for the composition.</param>
		protected override IEnumerable<Export> GetExportsCore( ImportDefinition definition,
		                                                       AtomicComposition atomicComposition )
		{
			var exports = base.GetExportsCore( definition, atomicComposition );
			/*return exports.Select( e => new Export( e.Definition, () => e.Value ) );*/
			return exports;
		}
	}
}