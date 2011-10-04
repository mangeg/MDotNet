namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Collections.Generic;
	using System.Windows;

	public interface IViewBinder
	{
		/// <summary>
		/// Binds the properties if an element to the view model.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <param name="viewModelType">Type of the view model.</param>
		/// <returns>A list of unmatched elements</returns>
		IEnumerable<FrameworkElement> BindProperties( IEnumerable<FrameworkElement> elements, Type viewModelType );

		/// <summary>
		/// Binds the actions of an element to the view model.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <param name="viewModelType">Type of the view model.</param>
		/// <returns>A list of unmatched elements</returns>
		IEnumerable<FrameworkElement> BindActions( IEnumerable<FrameworkElement> elements, Type viewModelType );

		/// <summary>
		/// Binds the specified view model to the view.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <param name="view">The view.</param>
		void Bind( object viewModel, DependencyObject view );
	}
}