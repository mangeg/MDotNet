namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using Extensions;

	public static class BindingScope
	{
		public static FrameworkElement FindName( this IEnumerable<FrameworkElement> elementsToSearch, String name )
		{
			return elementsToSearch.FirstOrDefault( x => x.Name.Equals( name, StringComparison.InvariantCultureIgnoreCase ) );
		}

		public static IEnumerable<FrameworkElement> GetNamedElements( DependencyObject element )
		{
			var root = element;
			var previous = element;
			DependencyObject contentPresenter = null;
			var routeHops = new Dictionary<DependencyObject, DependencyObject>();

			while ( true )
			{
				if ( root == null )
				{
					root = previous;
					break;
				}
				if ( root is UserControl )
					break;
#if SILVERLIGHT
				if(root is Page)
				{
					root = ((Page)root).Content as DependancyObject ?? root;
				}
#endif
				if ( root is ContentPresenter )
					contentPresenter = root;
				else if ( root is ItemsPresenter && contentPresenter != null )
				{
					routeHops[ root ] = contentPresenter;
					contentPresenter = null;
				}

				previous = root;
				root = VisualTreeHelper.GetParent( previous );
			}

			var descendants = new List<FrameworkElement>();
			var queue = new Queue<DependencyObject>();
			queue.Enqueue( root );

			while ( queue.Count > 0 )
			{
				var current = queue.Dequeue();
				var currentElement = current as FrameworkElement;

				if ( currentElement != null && !String.IsNullOrEmpty( currentElement.Name ) )
					descendants.Add( currentElement );

				if ( current is UserControl && current != root )
					continue;

				if ( routeHops.ContainsKey( current ) )
				{
					queue.Enqueue( routeHops[ current ] );
					continue;
				}

#if SILVERLIGHT
#else
				var childCount = ( current is UIElement || current is UIElement3D || current is ContainerVisual )
				                 	? VisualTreeHelper.GetChildrenCount( current )
				                 	: 0;
#endif
				if ( childCount > 0 )
				{
					for ( var i = 0; i < childCount; i++ )
					{
						var child = VisualTreeHelper.GetChild( current, i );
						queue.Enqueue( child );
					}
				}
				else
				{
					var contentControl = current as ContentControl;
					if ( contentControl != null )
					{
						if ( contentControl.Content is DependencyObject )
							queue.Enqueue( contentControl.Content as DependencyObject );

						var headerControl = contentControl as HeaderedContentControl;
						if ( headerControl != null && headerControl.Header is DependencyObject )
							queue.Enqueue( headerControl.Content as DependencyObject );
					}
					else
					{
						var itemsControl = current as ItemsControl;
						if ( itemsControl != null )
						{
							itemsControl.Items.OfType<DependencyObject>().Apply( queue.Enqueue );

							var headerControl = itemsControl as HeaderedItemsControl;
							if ( headerControl != null && headerControl.Header is DependencyObject )
								queue.Enqueue( headerControl.Header as DependencyObject );
						}
					}
				}
			}

			return descendants;
		}
	}
}