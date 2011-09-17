using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MDotNet.Logging
{
	/// <summary>
	/// LogManager for NLog
	/// </summary>
	public static class LogManager
	{
		private static LogFactory _factory;
		private static LoggingConfiguration _config;

		static LogManager()
		{
			_config = new LoggingConfiguration();

			var consoleTarget = GetDefaultColoredConsoleTarget();
			_config.AddTarget( "ColoredConsole", consoleTarget );

			var logRule = new LoggingRule();
			logRule.LoggerNamePattern = "*";
			logRule.Targets.Add( consoleTarget );
			logRule.EnableLoggingForLevel( LogLevel.Trace );
			logRule.EnableLoggingForLevel( LogLevel.Info );
			logRule.EnableLoggingForLevel( LogLevel.Debug );
			logRule.EnableLoggingForLevel( LogLevel.Warn );
			logRule.EnableLoggingForLevel( LogLevel.Error );
			logRule.EnableLoggingForLevel( LogLevel.Fatal );

			_config.LoggingRules.Add( logRule );
		}

		private static string GetNLogConfigFilePath()
		{
			Assembly thisAssembly = Assembly.GetEntryAssembly();

			var thisAssemblyPath = Path.GetDirectoryName( thisAssembly.Location );
			var assmConfig = Path.ChangeExtension( thisAssembly.Location, ".nlog" );

			if ( File.Exists( assmConfig ) )
				return Path.ChangeExtension( thisAssembly.Location, ".nlog" );

			return Path.Combine( thisAssemblyPath, "Default.nlog" );
		}

		/// <summary>
		/// Gets log factory instance for the colling assembly.
		/// </summary>
		/// <returns></returns>
		public static LogFactory Get()
		{
			if ( _factory == null )
			{
				var configFile = GetNLogConfigFilePath();
				if ( File.Exists( configFile ) )
					_factory = new LogFactory( new XmlLoggingConfiguration( configFile ) );
				else
				{
					_factory = new LogFactory( _config );
				}
			}

			return _factory;
		}
		/// <summary>
		/// Gets the default colored console target.
		/// </summary>
		/// <returns></returns>
		public static ColoredConsoleTarget GetDefaultColoredConsoleTarget()
		{
			var consoleTarget = new ColoredConsoleTarget();

			consoleTarget.Header = "${date:format=yyyy-MM-dd - hh\\:mm\\:ss} - Logging Started";
			consoleTarget.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}${onexception:inner=${newline}${exception:format=tostring}}";
			/*consoleTarget.Layout += "${onexception:inner=${newline}${pad:padding=50:padCharacter=*:inner=}${newline}" +
				"${pad:padding=10:inner=Method > :padCharacter= }" + 
				"${exception:format=Method}${newline}" + 
				"${pad:padding=10:inner=Type > :padCharacter= }" + 
				"${exception:format=Type}${newline}" + 
				"${pad:padding=10: inner=Message > :padCharacter= }" + 
				"${exception:format=Message }${newline}" + 
				"${pad:padding=10:inner=Stack >:padCharacter= }" +
				"${exception:separator= ssss :format=StackTrace}${newline}" + 
				"Inner Exception${newline}" + 
				"${pad:padding=10:inner=Type > :padCharacter= }" + 
				"${exception:format=:innerFormat=Type,:maxInnerExceptionLevel=5:innerExceptionSeparator=}${newline}" + 
				"${pad:padding=10:inner=Message > :padCharacter= }" + 
				"${exception:format=:innerFormat=Message:separator=->,:maxInnerExceptionLevel=5:innerExceptionSeparator=}${newline}" + 
				"${pad:padding=10:inner=Method > :padCharacter= }" + 
				"${exception:format=:innerFormat=Method,:maxInnerExceptionLevel=5:innerExceptionSeparator=}${newline}" +
				"${pad:padding=50:padCharacter=*:inner=}}";*/
			consoleTarget.Footer = "${date:format=yyyy-MM-dd - hh\\:mm\\:ss} - Logging Ended";

			return consoleTarget;
		}
	}
}
