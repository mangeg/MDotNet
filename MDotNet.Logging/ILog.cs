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
		/// Log warning.
		/// </summary>
		/// <param name="message">The message.</param>
		void Warning(String message);
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
		/// Log information.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		void Info(String message, Exception exception);
		/// <summary>
		/// Log warning.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		void Warning( String message, Exception exception );
		/// <summary>
		/// Log trace.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		void Trace( String message, Exception exception );
		/// <summary>
		/// Log fatal.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		void Fatal( String message, Exception exception );
		/// <summary>
		/// Log error.
		/// </summary>
		/// <param name="message">The message.</param>
		/// /// <param name="exception">The exception.</param>
		void Error( String message, Exception exception );

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		void Shutdown();
	}
}