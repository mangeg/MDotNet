namespace MDotNet.Logging
{
	using System;

	/// <summary>
	/// Interface for logger.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Log a information message.
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		void Info( string format, params object[] args );

		/// <summary>
		/// Log a warning message.
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		void Warn( string format, params object[] args );

		/// <summary>
		/// Log a trace message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		void Trace( string format, params object[] args );

		/// <summary>
		/// Log a fatal message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		void Fatal( string format, params object[] args );

		/// <summary>
		/// Log a error message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		void Error( string format, params object[] args );

		/// <summary>
		/// Log specified exception as an error.
		/// </summary>
		/// <param name="exception">The exception.</param>
		void Error( Exception exception );

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		void Shutdown();
	}
}