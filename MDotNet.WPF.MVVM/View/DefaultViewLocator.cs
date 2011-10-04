namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using Extensions;
	using Logging;
	using MEF.Attributes;

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( typeof( IViewLocator ), ServiceType.Both )]
	public class DefaultViewLocator : IViewLocator
	{
		private static readonly ILog _sLog = LogManager.GetLog();

		#region IViewLocator Members
		/// <summary>
		/// Locates view for a model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>
		/// A view if found matched to the model.
		/// </returns>
		public DependencyObject LocateForModel( object model ) { return LocateForModelType( model.GetType() ); }
		/// <summary>
		/// Locates the view for a model type.
		/// </summary>
		/// <param name="modelType">Type of the model.</param>
		/// <returns>
		/// A view if found matched to the model type.
		/// </returns>
		public DependencyObject LocateForModelType( Type modelType )
		{
			foreach ( var name in GetNamesToCheck( modelType ) )
			{
				foreach ( var assembly in new[] { modelType.Assembly } )
				{
					var type = assembly.GetType( name, false );
					if ( type == null ) continue;

					var view = CreateViewForType( type );
					if ( view == null ) continue;

					//_sLog.Info( "Located view {0} for {1}", view, modelType );
					InitializeComponent( view );
					return view;
				}
			}

			return new TextBlock { Text = string.Format( "Cannot find view for {0}.", modelType.FullName ) };
		}
		#endregion

		private DependencyObject CreateViewForType( Type type )
		{
			var view = IoC.GetAllInstances( type ).FirstOrDefault() as DependencyObject;
			if ( view != null )
				return view;

			if ( type.IsInterface || type.IsAbstract || !typeof( DependencyObject ).IsAssignableFrom( type ) )
				return new TextBlock { Text = string.Format( "Cannot create {0}.", type.FullName ) };

			return ( DependencyObject )Activator.CreateInstance( type );
		}

		public virtual IEnumerable<String> GetNamesToCheck( Type modelType )
		{
			var keywords = GetSingularKeywords();
			var modelTypeName = modelType.FullName;

			foreach ( var keyword in keywords )
			{
				if ( !String.IsNullOrEmpty( keyword ) )
				{
					foreach ( var w in keywords )
					{
						var firstPass = ReplaceWithView( modelTypeName.Replace( MakeNamespacePart( w ), ".Views" ), w );
						foreach ( var pass in firstPass )
						{
							foreach ( var w2 in keywords )
							{
								var secondPass = ReplaceWithView( pass, w2 );
								foreach ( var result in secondPass )
								{
									if ( !result.Equals( modelTypeName ) )
										yield return result;
								}
							}
						}
					}
				}
			}
		}
		public virtual IEnumerable<String> ReplaceWithView( String part, String toReplace )
		{
			if ( String.IsNullOrEmpty( toReplace ) )
			{
				foreach ( var keyword in GetSingularKeywords().Except( new[] { String.Empty } ) )
				{
					if ( part.EndsWith( keyword ) )
						yield return "{0}{1}".FormatWidth( part.Remove( part.LastIndexOf( keyword ) ), "View" );
				}
			}
			else if ( part.EndsWith( toReplace ) )
			{
				part = "{0}{1}".FormatWidth( part.Substring( 0, part.Length - toReplace.Length ), "View" );
				yield return part;
			}
			else
				yield return part;
		}
		public virtual IEnumerable<String> GetSingularKeywords()
		{
			return new[]
				{
					"ViewModel",
					"Model",
				};
		}

		protected virtual String MakeNamespacePart( String part ) { return ".{0}s".FormatWidth( part ); }

		public static void InitializeComponent( object element )
		{
			var method = element.GetType()
				.GetMethod( "InitializeComponent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance );

			if ( method == null )
				return;

			method.Invoke( element, null );
		}
	}
}