﻿namespace MDotNet.WPF.MVVM.Action
{
	using System;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Interactivity;

	/// <summary>
	///   Represents a parameter of an <see cref = "ActionMessage" />.
	/// </summary>
	public class Parameter : Freezable, IAttachedObject
	{
		/// <summary>
		///   A dependency property representing the parameter's value.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value",
				typeof( object ),
				typeof( Parameter ),
				new PropertyMetadata( OnValueChanged )
				);

		private DependencyObject _associatedObject;
		private WeakReference _owner;

		/// <summary>
		///   Gets or sets the value of the parameter.
		/// </summary>
		/// <value>The value.</value>
		[Category( "Common Properties" )]
		public object Value
		{
			get { return GetValue( ValueProperty ); }
			set { SetValue( ValueProperty, value ); }
		}

		private ActionMessage Owner
		{
			get { return _owner == null ? null : _owner.Target as ActionMessage; }
			set { _owner = new WeakReference( value ); }
		}

		#region IAttachedObject Members
		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				ReadPreamble();
				return _associatedObject;
			}
		}

		void IAttachedObject.Attach( DependencyObject dependencyObject )
		{
			WritePreamble();
			_associatedObject = dependencyObject;
			WritePostscript();
		}
		void IAttachedObject.Detach()
		{
			WritePreamble();
			_associatedObject = null;
			WritePostscript();
		}
		#endregion

		/// <summary>
		///   When implemented in a derived class, creates a new instance of the <see cref = "T:System.Windows.Freezable" /> derived class.
		/// </summary>
		/// <returns>The new instance.</returns>
		protected override Freezable CreateInstanceCore()
		{
			return new Parameter();
		}

		/// <summary>
		///   Makes the parameter aware of the <see cref = "ActionMessage" /> that it's attached to.
		/// </summary>
		/// <param name = "owner">The action message.</param>
		internal void MakeAwareOf( ActionMessage owner )
		{
			Owner = owner;
		}

		private static void OnValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var parameter = ( Parameter )d;

			if ( parameter.Owner != null )
				parameter.Owner.UpdateAvailability();
		}
	}
}