namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Markup;
	using Extensions;
	using Logging;

	public static class View
	{
		private static readonly ILog _sLog = LogManager.GetLog();
		private static readonly ContentPropertyAttribute _sDefaultContentProperty = new ContentPropertyAttribute( "Content" );

		public static readonly object DefaultContext = new object();

		public static readonly DependencyProperty ApplyConventionsProperty =
			DependencyProperty.RegisterAttached(
				"ApplyConventions",
				typeof( bool? ),
				typeof( View ),
				null
				);

		public static DependencyProperty ModelProperty =
			DependencyProperty.RegisterAttached(
				"Model",
				typeof( object ),
				typeof( View ),
				new PropertyMetadata( OnModelChanged )
				);

		public static readonly DependencyProperty IsGeneratedProperty =
			DependencyProperty.RegisterAttached(
				"IsGenerated",
				typeof( bool ),
				typeof( View ),
				new PropertyMetadata( false, null )
				);

		private static IViewLocator _locator;
		private static IViewBinder _binder;

		/// <summary>
		///   Used to retrieve the root, non-framework-created view.
		/// </summary>
		/// <param name = "view">The view to search.</param>
		/// <returns>The root element that was not created by the framework.</returns>
		/// <remarks>
		///   In certain instances the services create UI elements.
		///   For example, if you ask the window manager to show a UserControl as a dialog, it creates a window to host the UserControl in.
		///   The WindowManager marks that element as a framework-created element so that it can determine what it created vs. what was intended by the developer.
		///   Calling GetFirstNonGeneratedView allows the framework to discover what the original element was.
		/// </remarks>
		public static Func<object, object> GetFirstNonGeneratedView = view => {
			var dependencyObject = view as DependencyObject;
			if ( dependencyObject == null )
				return view;

			if ( ( bool )dependencyObject.GetValue( IsGeneratedProperty ) )
			{
				if ( dependencyObject is ContentControl )
					return ( ( ContentControl )dependencyObject ).Content;

				var type = dependencyObject.GetType();
				var contentProperty = type.GetAttributes<ContentPropertyAttribute>( true )
										.FirstOrDefault() ?? _sDefaultContentProperty;

				return type.GetProperty( contentProperty.Name )
					.GetValue( dependencyObject, null );
			}

			return dependencyObject;
		};

		public static void Initialize( IViewLocator locator, IViewBinder binder )
		{
			_locator = locator;
			_binder = binder;
		}

		/// <summary>
		/// Executes the handler immediately if the element is loaded, otherwise wires it to the Loaded event.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="handler">The handler.</param>
		/// <returns>true if the handler was executed immediately; false otherwise</returns>
		public static bool ExecuteOnLoad( FrameworkElement element, RoutedEventHandler handler )
		{
#if SILVERLIGHT
			if((bool)element.GetValue(IsLoadedProperty))
#else
			if ( element.IsLoaded )
#endif
			{
				handler( element, new RoutedEventArgs() );
				return true;
			}
			else
			{
				RoutedEventHandler loaded = null;
				loaded = ( s, e ) =>
				{
#if SILVERLIGHT
					element.SetValue(IsLoadedProperty, true);
#endif
					handler( s, e );
					element.Loaded -= loaded;
				};

				element.Loaded += loaded;
				return false;
			}
		}

		public static bool? GetApplyConventions( DependencyObject d )
		{
			return ( bool? )d.GetValue( ApplyConventionsProperty );
		}
		public static void SetApplyConvetions( DependencyObject d, bool? value )
		{
			d.SetValue( ApplyConventionsProperty, value );
		}

		public static void SetModel( DependencyObject d, object model )
		{
			d.SetValue( ModelProperty, model );
		}
		public static object GetModel( DependencyObject d )
		{
			return d.GetValue( ModelProperty );
		}
		private static void OnModelChanged( DependencyObject targetLocation, DependencyPropertyChangedEventArgs args )
		{
			if ( args.OldValue == args.NewValue )
				return;

			if ( args.NewValue != null )
			{
				var view = _locator.LocateForModel( args.NewValue );
				SetContent( targetLocation, view );
				_binder.Bind( args.NewValue, view );
			}
		}

		public static void SetContent( object targetLocation, object view )
		{
			var fe = view as FrameworkElement;
			if ( fe != null && fe.Parent != null )
				SetContentCore( fe.Parent, null );

			SetContentCore( targetLocation, view );
		}
		public static void SetContentCore( object targetLocation, object view )
		{
			try
			{
				var type = targetLocation.GetType();
				var contentProperty = type.GetAttributes<ContentPropertyAttribute>( true )
										.FirstOrDefault() ?? _sDefaultContentProperty;

				type.GetProperty( contentProperty.Name )
					.SetValue( targetLocation, view, null );
			}
			catch ( Exception e )
			{
				_sLog.Error( e );
			}
		}
	}
}