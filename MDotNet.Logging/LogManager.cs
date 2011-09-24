using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MDotNet.Logging
{
	/// <summary>
	/// LogManager for ILog
	/// </summary>
	public static class LogManager
	{
		private static readonly ILog _sNullLogger = new NullLog();
		private static Func<Type, ILog> _sLogLocator = logLocator => _sNullLogger;

		/// <summary>
		/// Initializes the LogManager wit the specified log locator.
		/// </summary>
		/// <param name="logLocator">The log locator.</param>
		public static void Initialize(Func<Type, ILog> logLocator)
		{
			_sLogLocator = logLocator;
		}
		
		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static ILog GetLog(Type type)
		{
			return _sLogLocator( type );
		}
		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static ILog GetLog<T>()
		{
			return GetLog( typeof (T) );
		}
		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <returns></returns>
		public static ILog GetLog()
		{
#if SILVERLIGHT
			var frame = new StackFrame(1);
#else
			var frame = new StackFrame( 1, false );
#endif
			return GetLog(frame.GetMethod().DeclaringType);
		}
	}

	internal class NullLog : ILog
	{
		/// <summary>
		/// Log information.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Info(string message)
		{
		}
		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Warning(string message)
		{
		}
		/// <summary>
		/// Log trace.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Trace(string message)
		{
		}
		/// <summary>
		/// Log fatal.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Fatal(string message)
		{
		}
		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Error(string message)
		{
		}
		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void Error(Exception exception)
		{
		}
		/// <summary>
		/// Log information.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public void Info(string message, Exception exception)
		{
		}
		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Warning(string message, Exception exception)
		{
		}
		/// <summary>
		/// Log trace.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Trace(string message, Exception exception)
		{
		}
		/// <summary>
		/// Log fatal.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Fatal(string message, Exception exception)
		{
		}
		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		public void Error(string message, Exception exception)
		{
		}

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		public void Shutdown(){}
	}
}
