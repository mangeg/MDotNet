namespace MDotNet.Logging
{
	using System;

	internal class NullLog : ILog
	{
		#region ILog Members

		/// <summary>
		/// Log a information message.
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Info( string format, params object[] args ) { }

		/// <summary>
		/// Log a warning message.
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Warn( string format, params object[] args ) { }

		/// <summary>
		/// Log a trace message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Trace( string format, params object[] args ) { }

		/// <summary>
		/// Log a fatal message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Fatal( string format, params object[] args ) { }

		/// <summary>
		/// Log a error message
		/// </summary>
		/// <param name="format">The format string or just the message.</param>
		/// <param name="args">The args.</param>
		public void Error( string format, params object[] args ) { }

		/// <summary>
		/// Log specified exception as an error.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void Error( Exception exception ) { }

		/// <summary>
		/// Shutdowns this instance.
		/// </summary>
		public void Shutdown() { }

		#endregion
	}
}