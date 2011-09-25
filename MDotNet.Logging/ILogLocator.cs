namespace MDotNet.Logging
{
	using System;

	/// <summary>
	/// Log locator interface
	/// </summary>
	public interface ILogLocator
	{
		/// <summary>
		/// Gets the typed locator.
		/// </summary>
		Func<Type, ILog> TypedLocator { get; }

		/// <summary>
		/// Gets the named locator.
		/// </summary>
		Func<String, ILog> NamedLocator { get; }
	}
}