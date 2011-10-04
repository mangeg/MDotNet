namespace MDotNet.Extensions
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Extensions for <see cref="IEnumerable{T}"/>
	/// </summary>
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Applies the specified action for each item.
		/// </summary>
		/// <typeparam name="T">IEnumerable type</typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="action">The action.</param>
		public static void Apply<T>( this IEnumerable<T> enumerable, Action<T> action )
		{
			foreach ( var item in enumerable )
			{
				action( item );
			}
		}
	}
}