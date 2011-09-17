using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDotNet.Common
{
	public sealed class AppInfo
	{
		/// <summary>
		/// The company name
		/// </summary>
		public const String CompanyName = "MSoft";
		/// <summary>
		/// Application name format.
		/// Supply the rest of the information after "MDotNext.".
		/// </summary>
		public const String AppNameFormat = "MDotNet.{0}";
		/// <summary>
		/// Application name.
		/// </summary>
		public const String AppName = "MDotNet";

		public const String CopyrightNote = "Copyright © MSoft 2011";

		/// <summary>
		/// Default XamlNamespace
		/// </summary>
		public const String XamlNamepsace = "http://www.azeroth.se/MDotNet";
		/// <summary>
		/// Gets the local user application data path.
		/// </summary>
		public static String LocalUserAppDataPath
		{
			get
			{
				var folderName = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
				folderName = Path.Combine( folderName, String.Format( @"{0}\{1}", AppInfo.CompanyName, AppInfo.AppName ) );
				if ( !Directory.Exists( folderName ) )
					Directory.CreateDirectory( folderName );
				return folderName;
			}
		}
		/// <summary>
		/// Gets the local user application company data path.
		/// </summary>
		public static String LocalUserAppCompanyPath
		{
			get
			{
				var folderName = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
				folderName = Path.Combine( folderName, CompanyName );
				if ( !Directory.Exists( folderName ) )
					Directory.CreateDirectory( folderName );
				return folderName;
			}
		}
	}
}
