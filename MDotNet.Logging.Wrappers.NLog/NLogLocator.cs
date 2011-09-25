namespace MDotNet.Logging.Wrappers.NLog
{
	using System;
	using global::NLog;
	using global::NLog.Config;
	using global::NLog.Targets;

	/// <summary>
	/// NLog log locator
	/// </summary>
	public class NLogLocator : ILogLocator
	{
		private static LogFactory _sFactory = new LogFactory();

		/// <summary>
		/// Gets the typed locator.
		/// </summary>
		public Func<Type, ILog> TypedLocator { get; private set; }

		/// <summary>
		/// Gets the named locator.
		/// </summary>
		public Func<string, ILog> NamedLocator { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="NLogLocator"/> class.
		/// </summary>
		public NLogLocator()
		{
			_sFactory.Configuration = GetDefaultConsoleLogConfig();

			TypedLocator = type => new NLogWrapper( _sFactory.GetLogger( type.FullName ) );
			NamedLocator = name => new NLogWrapper( _sFactory.GetLogger( name ) );
		}

		/// <summary>
		/// Gets a default console log config.
		/// </summary>
		/// <returns></returns>
		public static LoggingConfiguration GetDefaultConsoleLogConfig()
		{
			var config = new LoggingConfiguration();

			var consoleTarget = GetDefaultColoredConsoleTarget();
			config.AddTarget( "ColoredConsole", consoleTarget );

			var logRule = GetDefaultRule();
			logRule.Targets.Add( consoleTarget );

			config.LoggingRules.Add( logRule );

			return config;
		}

		/// <summary>
		/// Gets a default colored console target.
		/// </summary>
		/// <returns></returns>
		public static ColoredConsoleTarget GetDefaultColoredConsoleTarget()
		{
			var consoleTarget = new ColoredConsoleTarget();

			consoleTarget.Header = "${date:format=yyyy-MM-dd - hh\\:mm\\:ss} - Logging Started";
			consoleTarget.Layout =
				"${date:format=HH\\:MM\\:ss} ${logger} ${message}${onexception:inner=${newline}${exception:format=tostring}}";
			consoleTarget.Footer = "${date:format=yyyy-MM-dd - hh\\:mm\\:ss} - Logging Ended";

			return consoleTarget;
		}

		/// <summary>
		/// Gets the default rule.
		/// It has all log levels enabled.
		/// </summary>
		/// <returns>A default <see cref="LoggingRule"/>.</returns>
		public static LoggingRule GetDefaultRule()
		{
			var logRule = new LoggingRule();
			logRule.LoggerNamePattern = "*";
			logRule.EnableLoggingForLevel( LogLevel.Trace );
			logRule.EnableLoggingForLevel( LogLevel.Info );
			logRule.EnableLoggingForLevel( LogLevel.Debug );
			logRule.EnableLoggingForLevel( LogLevel.Warn );
			logRule.EnableLoggingForLevel( LogLevel.Error );
			logRule.EnableLoggingForLevel( LogLevel.Fatal );
			return logRule;
		}

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
	}
}