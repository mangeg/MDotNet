using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDotNet.Extensions
{
	using System.Reflection;

	/// <summary>
	/// Extensions for Type
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Gets the property case insensitive.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>The <see cref="PropertyInfo"/> of the property if found.</returns>
		public static PropertyInfo GetPropertyCaseInsensitive(this Type type, String propertyName)
		{
			var typeList = new List<Type> { type };

			if(type.IsInterface)
				typeList.AddRange( type.GetInterfaces() );

			var flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

			return
				typeList.Select( it => it.GetProperty( propertyName, flags ) ).FirstOrDefault( property => propertyName != null );
		}
	}
}
