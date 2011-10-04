namespace MUtils.Logging
{
	using System;

	public class CaliburnLogWrapper : Caliburn.Micro.ILog
	{
		private MDotNet.Logging.ILog _log;

		public CaliburnLogWrapper( MDotNet.Logging.ILog log ) { _log = log; }

		#region ILog Members

		/// <summary>
		/// Logs the message as info.
		/// </summary>
		/// <param name="format">A formatted message.</param><param name="args">Parameters to be injected into the formatted message.</param>
		public void Info( string format, params object[] args ) { }

		/// <summary>
		/// Logs the message as a warning.
		/// </summary>
		/// <param name="format">A formatted message.</param><param name="args">Parameters to be injected into the formatted message.</param>
		public void Warn( string format, params object[] args ) { }

		/// <summary>
		/// Logs the exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void Error( Exception exception ) { }

		#endregion
	}
}