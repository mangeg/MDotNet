using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MDotNet.Settings.Targets
{
	public class XmlTarget<T> : ITarget where T : class, new()
	{
		private XmlSerializer _xmls = new XmlSerializer( typeof( T ) );
		private XmlWriterSettings _xmlwSettings;

		public String Filename { get; set; }
		public bool FailSilent { get; set; }

		public object Value { get; set; }

		public XmlTarget()
		{
			FailSilent = true;
			_xmlwSettings = new XmlWriterSettings() {
				Indent = true,
				IndentChars = "\t",
				Encoding = Encoding.UTF8,
			};
		}

		public XmlTarget( string filename )
			: this()
		{
			Filename = filename;
		}

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
				if ( String.IsNullOrEmpty( folder ) )
				{
					throw new ArgumentException( "Cannot extract path from filename", "Filename" );
				}
				if ( !Directory.Exists( folder ) )
					Directory.CreateDirectory( folder );

				using ( var xw = XmlWriter.Create( File.Create( Filename ), _xmlwSettings ) )
				{
					_xmls.Serialize( xw, Value );
				}
			}
			catch ( Exception ex )
			{
				// TODO: Log the exception.
				if ( !FailSilent )
					throw ex;
			}
		}
		public void Load()
		{
			try
			{
				if ( String.IsNullOrEmpty( Filename ) )
				{
					throw new ArgumentNullException( "Filename" );
				}
				if ( !File.Exists( Filename ) )
				{
					throw new ArgumentException( "File does not exist.", "Filename" );
				}
			}
			catch ( Exception ex )
			{
				// TODO: Log the exception.
				if ( !FailSilent )
					throw ex;
			}

		}
	}
}