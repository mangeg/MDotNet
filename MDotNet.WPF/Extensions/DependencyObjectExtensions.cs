namespace MDotNet.WPF.Extensions
{
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Media;

	public static class DependencyObjectExtensions
	{
		/// <summary>
		/// Find the first child of the specified type.
		/// </summary>
		/// <param name="parent"></param>
		/// <typeparam name="T">The type to find</typeparam>
		/// <returns><see cref="DependencyObject"/> of the type requested if found; else <c>null</c></returns>
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

		/// <summary>
		/// Finds all children of the specified type of <see cref="DependencyObject"/>.
		/// </summary>
		/// <typeparam name="T">Type of <see cref="DependencyObject"/> to find.</typeparam>
		/// <param name="parent">The parent.</param>
		/// <returns><see cref="IEnumerable{T}"/> of all found children.</returns>
		public static IEnumerable<T> FindAllChildren<T>( this DependencyObject parent ) where T : DependencyObject
		{
			if ( parent != null )
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount( parent );
				for ( var i = 0; i < childrenCount; i++ )
				{
					var child = VisualTreeHelper.GetChild( parent, i );
					var childType = child as T;
					if ( childType != null )
						yield return childType;

					if ( child == null ) continue;

					var children = child.FindAllChildren<T>();
					foreach ( var subChild in children )
					{
						yield return subChild;
					}
				}
			}
		}
	}
}