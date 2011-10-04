namespace MDotNet.WPF.MVVM.Action
{
	using System;
	using System.Windows;
	using ApplicationModel;
	using Logging;
	using View;

	public static class Action
	{
		private static readonly ILog _sLog = LogManager.GetLog();

		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.RegisterAttached(
				"Target",
				typeof( object ),
				typeof( Action ),
				new PropertyMetadata( OnTargetChanged )
				);
		public static readonly DependencyProperty TargetWithoutContextProperty =
			DependencyProperty.RegisterAttached(
				"TargetWithoutContext",
				typeof( object ),
				typeof( Action ),
				new PropertyMetadata( OnTargetWithoutContextChanged ) 
				);

		public static object GetTarget( DependencyObject d )
		{
			return d.GetValue( TargetProperty );
		}
		public static void SetTarget( DependencyObject d, object target )
		{
			d.SetValue( TargetProperty, target );
		}
		public static object GetTargetWithoutContext(DependencyObject d)
		{
			return d.GetValue( TargetWithoutContextProperty );
		}
		public static void SetTargetWithoutContext(DependencyObject d, object value)
		{
			d.SetValue( TargetWithoutContextProperty, value );
		}

		public static void OnTargetChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			SetTargetCore( e, d, true );
		}
		public static void OnTargetWithoutContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SetTargetCore( e, d, false );
		}
		private static void SetTargetCore( DependencyPropertyChangedEventArgs e, DependencyObject d, bool setContext )
		{
			if ( FrameworkConfiguration.IsInDesignMode || e.NewValue == e.OldValue || e.NewValue == null )
				return;

			var target = e.NewValue;
			var containerKey = e.NewValue as String;

			if ( containerKey != null )
				target = IoC.GetInstance( containerKey );

			if ( setContext && d is FrameworkElement )
			{
				//_sLog.Info( "Setting DC of {0} to {1}", d, target );
				( ( FrameworkElement )d ).DataContext = target;
			}

			//_sLog.Info( "Attaching message handler {0} to {1}.", target, d );
			Message.SetHandler( d, target );
		}

		public static bool HasTargetSet( DependencyObject element )
		{
			if ( GetTarget( element ) != null || GetTargetWithoutContext( element ) != null )
				return true;

			var frameworkElement = element as FrameworkElement;
			if ( frameworkElement == null )
				return false;

			var conventionManager = IoC.GetInstance<IConventionManager>();

			return conventionManager.HasBinding( frameworkElement, TargetProperty )
				   || conventionManager.HasBinding( frameworkElement, TargetWithoutContextProperty );
		}
	}
}