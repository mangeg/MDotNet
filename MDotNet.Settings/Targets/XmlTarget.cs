namespace MDotNet.Settings.Targets
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;
	using Extensions;

	/// <summary>
	/// Xml target for SettingsManager.
	/// </summary>
	/// <typeparam name="T">The type this target loads and saves.</typeparam>
	public class XmlTarget<T> : ITarget where T : class, new()
	{
		private readonly XmlSerializer _xmls; // = new XmlSerializer( typeof( T ) );
		private readonly XmlWriterSettings _xmlwSettings;

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlTarget&lt;T&gt;"/> class.
		/// </summary>
		public XmlTarget()
		{
			_xmls = new XmlSerializer( typeof( T ) );
			FailSilent = true;
			_xmlwSettings = new XmlWriterSettings
				{
					Indent = true,
					IndentChars = "\t",
					Encoding = Encoding.UTF8,
				};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlTarget&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public XmlTarget( string filename )
			: this() { Filename = filename; }

		/// <summary>
		/// Gets or sets the filename.
		/// </summary>
		/// <value>
		/// The filename.
		/// </value>
		public String Filename { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether it should fail silently
		/// and only produce log message. While in none silent mode it will
		/// throw exceptions together with log messages.
		/// </summary>
		/// <value>
		///   <c>true</c> to fail silently; otherwise, <c>false</c>.
		/// </value>
		public bool FailSilent { get; set; }

		#region ITarget Members

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public object Value { get; set; }

		/// <summary>
		/// Saves the value of this target.
		/// </summary>
		public void Save()
		{
			try
			{
				if ( String.IsNullOrEmpty( Filename ) )
				{
					throw new ArgumentNullException( "Filename" );
				}
				if ( Value == null )
				{
					throw new ArgumentException( "Value has not ben set", "Value" );
				}

				var folder = Path.GetDirectoryName( Filename );
				if ( folder == null )
				{
					throw new ArgumentException( "Cannot extract path from filename", "Filename" );
				}
				if ( !String.IsNullOrEmpty( folder ) && !Directory.Exists( folder ) )
					Directory.CreateDirectory( folder );

				using ( var fs = File.Create( Filename ) )
				{
					using ( var xw = XmlWriter.Create( fs, _xmlwSettings ) )
					{
						_xmls.Serialize( xw, Value );
					}
				}
			}
			catch ( Exception ex )
			{
				// TODO: Log the exception.
				if ( !FailSilent )
					throw ex;
			}
		}

		/// <summary>
		/// Loads the value for this target.
		/// </summary>
		public void Load()
		{
			try
			{
				if ( String.IsNullOrEmpty( Filename ) )
					throw new ArgumentNullException( "Filename" );
				if ( !File.Exists( Filename ) )
					throw new FileNotFoundException( "Settings file not fund", Filename );

				using ( var fs = File.OpenRead( Filename ) )
				{
					using ( var xr = XmlReader.Create( fs ) )
					{
						if ( !_xmls.CanDeserialize( xr ) )
							throw new FileLoadException( "Could not deserialize into {0}".FormatWidth( typeof( T ).FullName ) );

						Value = _xmls.Deserialize( xr );
					}
				}
			}
			catch ( Exception ex )
			{
				// TODO: Log the exception.
				if ( !FailSilent )
					throw ex;
			}
		}

		#endregion
	}
}