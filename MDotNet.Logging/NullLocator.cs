namespace MDotNet.Logging
{
	using System;

	internal class NullLocator : ILogLocator
	{
		private NullLog _log = new NullLog();

		/// <summary>
		/// Gets the typed locator.
		/// </summary>
		public Func<Type, ILog> TypedLocator { get; private set; }

		/// <summary>
		/// Gets the named locator.
		/// </summary>
		public Func<string, ILog> NamedLocator { get; private set; }

		public NullLocator()
		{
			TypedLocator = type => _log;
			NamedLocator = name => _log;
		}
	}
}