namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Reflection;
	using System.Windows;

	public interface IConventionManager
	{
		ElementConvention GetElementConvention( Type elementType );

		bool SetBinding( Type viewModelType, String path, PropertyInfo property, FrameworkElement element,
		                 ElementConvention convention );
		bool HasBinding( FrameworkElement element, DependencyProperty property );

		ElementConvention AddElementConvetion<T>( DependencyProperty bindableProperty, String parameterProperty, String eventName );
		ElementConvention AddElementConvetion( ElementConvention convention );
	}

	public class ElementConvention
	{
		public Func<DependencyObject, DependencyProperty> GetBindableProperty;
		public Type ElementType { get; set; }
		public String ParameterProperty { get; set; }
		public Func<Type, string, PropertyInfo, FrameworkElement, bool> Extra;
		public Func<System.Windows.Interactivity.TriggerBase> CreateTrigger;
	}
}