namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Windows;

	public interface IViewLocator
	{
		/// <summary>
		/// Locates view for a model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>A view if found matched to the model.</returns>
		DependencyObject LocateForModel( object model );

		/// <summary>
		/// Locates the view for a model type.
		/// </summary>
		/// <param name="modelType">Type of the model.</param>
		/// <returns>A view if found matched to the model type.</returns>
		DependencyObject LocateForModelType( Type modelType );
	}
}