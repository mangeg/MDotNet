namespace MDotNet.WPF.MVVM.ViewModel
{
	using System.ComponentModel;

	/// <summary>
	/// Extension for <see cref="INotifyPropertyChanged"/> to allow
	/// controlling if the notifications should be enabled or not.
	/// It also adds functionallity to notify for refresh of all properties.
	/// </summary>
	public interface INotifyPropertyChangedEx : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets or sets a value indicating whether this instance is notifying
		/// on property changed.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is notifying; otherwise, <c>false</c>.
		/// </value>
		bool IsNotifying { get; set; }

		/// <summary>
		/// Notifies that the property has changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		void NotifyPorpertyChange( string propertyName );

		/// <summary>
		/// Refresh all properties.
		/// </summary>
		void Refres();
	}
}