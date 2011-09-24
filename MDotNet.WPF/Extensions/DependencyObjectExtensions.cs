using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MDotNet.WPF.Extensions
{
	public static class DependencyObjectExtensions
	{
		/// <summary>
		/// Find the first child of the specified type.
		/// </summary>
		/// <param name="parent"></param>
		/// <typeparam name="T">The type to find</typeparam>
		/// <returns></returns>
		public static T FindFirstChild<T>( this DependencyObject parent ) where T : DependencyObject
		{
			// Confirm parent and childName are valid. 
			if ( parent == null ) return null;

			int childrenCount = VisualTreeHelper.GetChildrenCount( parent );
			for ( var i = 0; i < childrenCount; i++ )
			{
				var child = VisualTreeHelper.GetChild( parent, i );
				// If the child is not of the request child type child
				var childType = child as T;
				if ( childType == null )
				{
					// recursively drill down the tree
					var foundChild = child.FindFirstChild<T>();

					// If the child is found, break so we do not overwrite the found child. 
					if ( foundChild != null )
						return foundChild;
				}
				else
				{
					return childType;
				}
			}

			return null;
		}

		public static IEnumerable<T> FindAllChildren<T>( this DependencyObject parent ) where T : DependencyObject
		{
			if ( parent == null ) return null;

			var ret = new List<T>();
			int childrenCount = VisualTreeHelper.GetChildrenCount( parent );
			for ( var i = 0; i < childrenCount; i++ )
			{
				var child = VisualTreeHelper.GetChild( parent, i );
				var childType = child as T;
				if ( childType != null )
					ret.Add( childType );

				if ( child != null )
				{
					var children = child.FindAllChildren<T>();
					foreach ( var subChild in children )
					{
						ret.Add( subChild );
					}
				}
			}

			return ret;
		}
	}
}
