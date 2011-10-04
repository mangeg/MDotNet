namespace MDotNet.WPF.MVVM.MEF
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition.Hosting;
	using System.ComponentModel.Composition.Primitives;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using Service;
	using Service.Contracts;

	/// <summary>
	/// Default composer for Design time. This will load all assemblies that have the DesignTimeCatalog attibute
	/// </summary>
	public class DefaultDesignTimeComposer : IComposer
	{
		private const string MdotnetWpfMvvm = "MDotNet.WPF.MVVM";
		private const string MdotnetWpfMvvmDll = "MDotNet.WPF.MVVM.dll";

		#region IComposer Members

		public ComposablePartCatalog InitializeContainer() { return GetCatalog(); }

		#endregion

		private AggregateCatalog GetCatalog()
		{
			//CAS - Modified to work around Blend’s caching of different versions of an assembly at the same time.
			//This will ensure that only the newest assembly found is actually in the catalog.

			var assemDict = new Dictionary<string, AssemblyCatalog>();


			IList<AssemblyCatalog> assembliesLoadedCatalogs =
				( from assembly in AppDomain.CurrentDomain.GetAssemblies()
				  //only load assemblyies with this attribute
				  where assembly.GetReferencedAssemblies().Where( x => x.Name.Contains( MdotnetWpfMvvm ) ).Count() > 0 ||
				        assembly.ManifestModule.Name == MdotnetWpfMvvmDll &&
				        !ShouldIgnoreAtDesignTime( assembly )
				  select new AssemblyCatalog( assembly ) ).ToList();

			if ( assembliesLoadedCatalogs.Where( x => x.Assembly.ManifestModule.Name != MdotnetWpfMvvmDll ).Count() == 0 )
			{
				Debug.WriteLine( "No assemblies found for Design time. Quick tip... " );
				return null;
			}

			var catalog = new AggregateCatalog();

			foreach ( var item in assembliesLoadedCatalogs )
			{
				AssemblyCatalog ass;
				if ( assemDict.TryGetValue( item.Assembly.FullName, out ass ) )
				{
					var oldAssDt = File.GetLastAccessTime( ass.Assembly.Location );
					var newAssDt = File.GetLastAccessTime( item.Assembly.Location );
					if ( newAssDt > oldAssDt )
					{
						assemDict[ item.Assembly.FullName ] = item;
					}
				}
				else
				{
					assemDict[ item.Assembly.FullName ] = item;
				}
			}

			foreach ( var item in assemDict.Values )
				catalog.Catalogs.Add( item );
			return catalog;
		}

		private static bool ShouldIgnoreAtDesignTime( Assembly assembly )
		{
			object[] customAttributes = null; /*assembly.GetCustomAttributes( typeof( IgnoreAtDesignTimeAttribute ), true );*/
			return customAttributes != null && customAttributes.Length != 0;
		}
	}
}