using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MDotNet.Logging
{
	/// <summary>
	/// NLogManager
	/// </summary>
	public static class NLogManager
	{
		private static LogFactory _factory;
		private static LoggingConfiguration _config;

		/// <summary>
		/// The default NLog locator.
		/// </summary>
		public static Func<Type, ILog> DefaultNLogLocator = ( type ) =>
		{
		    Init();
			return new NLogWrapper( _factory.GetLogger( type.FullName ) );
		};

		static NLogManager()
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

		static LogFactory Init()
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
		private static string GetNLogConfigFilePath()
		{
			Assembly thisAssembly = Assembly.GetEntryAssembly();

			var thisAssemblyPath = Path.GetDirectoryName( thisAssembly.Location );
			var assmConfig = Path.ChangeExtension( thisAssembly.Location, ".nlog" );

			if ( File.Exists( assmConfig ) )
				return Path.ChangeExtension( thisAssembly.Location, ".nlog" );

			return Path.Combine( thisAssemblyPath, "Default.nlog" );
		}
	}

	class NLogWrapper : ILog
	{
		private Logger _log;

		public Logger NLog
		{
			get { return _log; }
		}

		public NLogWrapper(Logger log)
		{
			_log = log;
		}

		/// <summary>
		/// Log information.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Info(string message)
		{
			_log.Info( message );
		}

		/// <summary>
		/// Log information.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public void Info(string format, params object[] args)
		{
			_log.Info( format, args );
		}

		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Warning(string message)
		{
			_log.Warn( message );
		}

		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public void Warning(string format, params object[] args)
		{
			_log.Warn( format, args );
		}

		/// <summary>
		/// Log trace.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Trace(string message)
		{
			_log.Trace( message );
		}

		/// <summary>
		/// Log fatal.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Fatal(string message)
		{
			_log.Fatal( message );
		}

		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Error(string message)
		{
			_log.Error( message );
		}

		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void Error(Exception exception)
		{
			_log.Error( exception );
		}

		/// <summary>
		/// Log information.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public void Info(string message, Exception exception)
		{
			_log.Info( message, exception );
		}

		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Warning(string message, Exception exception)
		{
			_log.WarnException( message, exception );
		}

		/// <summary>
		/// Log trace.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Trace(string message, Exception exception)
		{
			_log.TraceException( message, exception );
		}

		/// <summary>
		/// Log fatal.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Fatal(string message, Exception exception)
		{
			_log.FatalException( message, exception );
		}

		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Error(string message, Exception exception)
		{
			_log.ErrorException( message, exception );
		}

		public void Shutdown()
		{
			_log.Factory.Configuration = null;
		}
	}
}
