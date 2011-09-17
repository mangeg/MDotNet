using System;
using System.ComponentModel;
using System.Windows;

namespace MDotNet.WPF.MVVM
{
	/// <summary>
	/// Base class for Viewmodels.
	/// </summary>
	public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged, IDisposable
	{
		/// <summary>
		/// Indicating if this instance is dirty
		/// </summary>
		public static readonly DependencyProperty IsDirtyProperty =
			DependencyProperty.Register( "IsDirty", typeof( bool ), typeof( ViewModelBase ), new PropertyMetadata( default( bool ) ) );

		/// <summary>
		/// Gets or sets a value indicating whether this instance is dirty.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
		/// </value>
		public bool IsDirty
		{
			get { return ( bool )GetValue( IsDirtyProperty ); }
			set { SetValue( IsDirtyProperty, value ); }
		}


		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// Raises a property changed event.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public void RaisPropertyChanged( String propertyName )
		{
			if ( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			OnDispose();
		}
		/// <summary>
		/// Called on dispose, override to make custom version.
		/// </summary>
		protected virtual void OnDispose()
		{
		}
	}
}
