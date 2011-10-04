namespace MDotNet.WPF.MVVM.Service.Contracts
{
	/// <summary>
	/// Base interface for event handlers
	/// </summary>
	public interface IHandleEvent{}
	/// <summary>
	/// Interface to handle specific type s of event.
	/// </summary>
	/// <typeparam name="T">The type of event to handle</typeparam>
	public interface IHandleEvent<in T>
	{
		/// <summary>
		/// Handles the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Handle( T message );
	}

	/// <summary>
	/// Interface for the event handler.
	/// </summary>
	public interface IEventHandler
	{
		/// <summary>
		/// Registers an instance to handlel events.
		/// </summary>
		/// <param name="instance">The instance.</param>
		void Register( object instance );
		/// <summary>
		/// Unregisters the specified instance.
		/// </summary>
		/// <param name="instance">The instance.</param>
		void Unregister( object instance );
		/// <summary>
		/// Fires an event for the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Fire( object message );
	}
}