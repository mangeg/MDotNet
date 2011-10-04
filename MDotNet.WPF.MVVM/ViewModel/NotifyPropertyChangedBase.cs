namespace MDotNet.WPF.MVVM.ViewModel
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using Extensions;

	/// <summary>
	///   Base class for handling <see cref = "INotifyPropertyChanged" />
	/// </summary>
	public class NotifyPropertyChangedBase : INotifyPropertyChangedEx
	{
		public NotifyPropertyChangedBase()
		{
			IsNotifying = true;
		}

		#region INotifyPropertyChangedEx Members
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///   Gets or sets a value indicating whether this instance is notifying
		///   on property changed.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is notifying; otherwise, <c>false</c>.
		/// </value>
		public bool IsNotifying { get; set; }

		/// <summary>
		///   Notifies that the property has changed.
		/// </summary>
		/// <param name = "propertyName">Name of the property.</param>
		public virtual void NotifyPorpertyChange( string propertyName )
		{
			if ( IsNotifying )
				RaisePropertyChangeed( propertyName );
		}

		/// <summary>
		///   Refresh all properties.
		/// </summary>
		public void Refres()
		{
			NotifyPorpertyChange( string.Empty );
		}
		#endregion

		/// <summary>
		///   Notifies that the property has changed.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "property">The property.</param>
		public virtual void NotifyPropertyChange<TProperty>( Expression<Func<TProperty>> property )
		{
			NotifyPorpertyChange( property.GetMemberInfo().Name );
		}

		private void RaisePropertyChangeed( string propertyName )
		{
			if ( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}