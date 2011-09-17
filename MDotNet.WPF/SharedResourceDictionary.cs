using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using MS.Internal;
using MS.Internal.Utility;

namespace MDotNet.WPF
{
	public class SharedResourceDictionary : ResourceDictionary
	{
		/// <summary>
		/// Internal cache of loaded dictionaries 
		/// </summary>
		public static Dictionary<Uri, ResourceDictionary> _sharedDictionaries =
			new Dictionary<Uri, ResourceDictionary>();

		/// <summary>
		/// Local member of the source uri
		/// </summary>
		private Uri _sourceUri;

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) to load resources from.
		/// </summary>
		public new Uri Source
		{
			get
			{
				if ( IsInDesignMode )
				{
					return base.Source;
					try
					{
						return base.Source;
					}
					catch ( Exception )
					{
					}
				}
				return _sourceUri;
			}
			set
			{
				if ( IsInDesignMode )
				{
					if ( value == null || String.IsNullOrEmpty( value.OriginalString ) )
					{
						
					}
					else
						base.Source = value;
					return;
				}

				_sourceUri = value;
				//_sourceUri = new Uri( value.OriginalString );

				lock ( ( ( ICollection )_sharedDictionaries ).SyncRoot )
				{
					if ( !_sharedDictionaries.ContainsKey( value ) )
					{
						// If the dictionary is not yet loaded, load it by setting
						// the source of the base class
						base.Source = value;

						// add it to the cache
						_sharedDictionaries.Add( value, this );
					}
					else
					{
						// If the dictionary is already loaded, get it from the cache
						MergedDictionaries.Add( _sharedDictionaries[ value ] );
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is in design mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is in design mode; otherwise, <c>false</c>.
		/// </value>
		private static bool IsInDesignMode
		{
			get
			{
				return ( bool )DependencyPropertyDescriptor.FromProperty( DesignerProperties.IsInDesignModeProperty,
					typeof( DependencyObject ) ).Metadata.DefaultValue;
			}
		}
	}
}
