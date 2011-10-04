namespace MDotNet.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Extention methods for reflection stuff.
	/// </summary>
	public static class ReflectionExtensions
	{
		/// <summary>
		/// Gets all attributes of a specific type.
		/// </summary>
		/// <typeparam name="T">The attribute type</typeparam>
		/// <param name="member">The member.</param>
		/// <param name="inherit">if set to <c>true</c> [inherit].</param>
		/// <returns></returns>
		public static IEnumerable<T> GetAttributes<T>( this MemberInfo member, bool inherit )
		{
#if WinRT
			return member.GetCustomAttributes(inherit).OfType<T>();
#else
			return Attribute.GetCustomAttributes( member, inherit ).OfType<T>();
#endif
		}
	}
}