using System;

namespace MDotNet.Logging
{
	/// <summary>
	/// Interface for logger.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Log information.
		/// </summary>
		/// <param name="message">The message.</param>
		void Info(String message);
		/// <summary>
		/// Log information.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		void Info(String format, params object[] args);
		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="message">The message.</param>
		void Warning(String message);
		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		void Warning( String format, params object[] args );
		/// <summary>
		/// Log trace.
		/// </summary>
		/// <param name="message">The message.</param>
		void Trace(String message);
		/// <summary>
		/// Log fatal.
		/// </summary>
		/// <param name="message">The message.</param>
		void Fatal(String message);
		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="message">The message.</param>
		void Error(String message);
		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="exception">The exception.</param>
		void Error(Exception exception);

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		void Shutdown();
	}
}