namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Linq;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Data;
	using System.Windows.Markup;
	using Logging;
	using MEF.Attributes;

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( typeof( IConventionManager ), ServiceType.Both )]
	public class DefaultConventionManager : IConventionManager
	{
		private static readonly ILog _sLog = LogManager.GetLog();

		private static readonly Dictionary<Type, ElementConvention> _sElementConventions =
			new Dictionary<Type, ElementConvention>();


		public static DataTemplate DefaultItemTemplate =
			( DataTemplate )
#if SILVERLIGHT
		XamlReader.Load(
#else
			XamlReader.Parse(
#endif
				"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' " +
				"xmlns:mdot='http://www.azeroth.se/MDotNet'> " +
				"<ContentControl mdot:View.Model=\"{Binding}\" VerticalContentAlignment=\"Stretch\" HorizontalContentAlignment=\"Stretch\" IsTabStop=\"False\" />" +
				"</DataTemplate>"
				);

		public DefaultConventionManager()
		{
			AddElementConvetion<TextBox>( TextBox.TextProperty, "Text", "TextChanged" );
			AddElementConvetion<TextBlock>( TextBlock.TextProperty, "Text", "TextChanged" );
			AddElementConvetion<MenuItem>( ItemsControl.ItemsSourceProperty, "DataContext", "Click" );
			AddElementConvetion<ButtonBase>( ContentControl.ContentProperty, "DataContext", "Click" );

			AddElementConvetion<ContentControl>( ContentControl.ContentProperty, "DataContext", "Loaded" ).GetBindableProperty =
				delegate( DependencyObject foundControl ) {
					var element = ( ContentControl )foundControl;

					if ( element.Content is DependencyObject )
						return null;

#if SILVERLIGHT
					var useViewModel = element.ContentTemplate == null;
#else
					var useViewModel = element.ContentTemplate == null && element.ContentTemplateSelector == null;
#endif
					if ( useViewModel )
					{
						//_sLog.Info( "ViewModel bound on {0}", element.Name );
						return View.ModelProperty;
					}

					return ContentControl.ContentProperty;
				};
			AddElementConvetion<ItemsControl>( ItemsControl.ItemsSourceProperty, "DataContext", "Loaded" )
				.Extra = ( viewModelType, path, property, element ) => {
					ApplyItemTemplate( ( ItemsControl )element, property );
					return true;
				};
		}

		#region IConventionManager Members
		public ElementConvention GetElementConvention( Type elementType )
		{
			if ( elementType == null )
				return null;

			ElementConvention propertyConvention;
			_sElementConventions.TryGetValue( elementType, out propertyConvention );
			return propertyConvention ?? GetElementConvention( elementType.BaseType );
		}
		public bool SetBinding( Type viewModelType, string path, PropertyInfo property, FrameworkElement element,
								ElementConvention convention )
		{
			var bindableProperty = convention.GetBindableProperty( element );
			if ( bindableProperty == null || HasBinding( element, bindableProperty ) )
				return false;

			var binding = new Binding( path );

			ApplyBindingMode( binding, property );

			BindingOperations.SetBinding( element, bindableProperty, binding );

			return true;
		}
		public ElementConvention AddElementConvetion<T>( DependencyProperty bindableProperty, String parameterProperty, String eventName )
		{
			return AddElementConvetion( new ElementConvention()
				{
					ElementType = typeof( T ),
					GetBindableProperty = element => bindableProperty,
					ParameterProperty = parameterProperty,
					CreateTrigger = () => new System.Windows.Interactivity.EventTrigger { EventName = eventName }
				} );
		}
		public ElementConvention AddElementConvetion( ElementConvention convention )
		{
			_sElementConventions[ convention.ElementType ] = convention;
			return convention;
		}
		#endregion

		public void ApplyItemTemplate( ItemsControl itemsControl, PropertyInfo property )
		{
			if ( !string.IsNullOrEmpty( itemsControl.DisplayMemberPath )
				 || HasBinding( itemsControl, ItemsControl.DisplayMemberPathProperty )
				 || itemsControl.ItemTemplate != null
				 || !property.PropertyType.IsGenericType )
				return;

#if !WP7
			var itemType = property.PropertyType.GetGenericArguments().First();
			if ( itemType.IsValueType || typeof( string ).IsAssignableFrom( itemType ) )
				return;
#endif

#if !SILVERLIGHT && !WP7
			if ( itemsControl.ItemTemplateSelector == null )
			{
				itemsControl.ItemTemplate = DefaultItemTemplate;
				//_sLog.Info( "ItemTemplate applied to {0}.", itemsControl.Name );
			}
#else
			itemsControl.ItemTemplate = DefaultItemTemplate;
			Log.Info("ItemTemplate applied to {0}.", itemsControl.Name);
#endif
		}
		public void ApplyBindingMode( Binding binding, PropertyInfo property )
		{
			var setMethod = property.GetSetMethod();
			binding.Mode = ( property.CanWrite && setMethod != null && setMethod.IsPublic )
							? BindingMode.TwoWay
							: BindingMode.OneWay;
		}
		public bool HasBinding( FrameworkElement element, DependencyProperty property )
		{
			return element.GetBindingExpression( property ) != null;
		}
	}
}