using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MDotNet.Common.IoC
{
	public static class IoCExtensions
	{
		public static object GetInstance( this IServiceLocator locator, Type sericeType )
		{
			return locator.GetInstance( sericeType, null );
		}
		public static T GetInstance<T>( this IServiceLocator locator, String key )
		{
			return ( T )locator.GetInstance( typeof( T ), key );
		}
		public static T GetInstance<T>( this IServiceLocator locator )
		{
			return ( T )locator.GetInstance( typeof( T ) );
		}
		public static IEnumerable<T> GetAllInstances<T>( this IServiceLocator locator )
		{
			return locator.GetAllInstances( typeof( T ) ).Cast<T>();
		}

		internal static Func<Type, ConstructorInfo> SelectEligibleConstructorImplementation =
			DefaultSelectEliligibleConstructor;
		private static ConstructorInfo DefaultSelectEliligibleConstructor( Type type )
		{
			return type.GetConstructors().OrderByDescending( c => c.GetParameters().Length ).FirstOrDefault();
		}
		public static ConstructorInfo SelectEligibleConstructor( this Type type )
		{
			return SelectEligibleConstructorImplementation( type );
		}
		public static bool IsConcrete( this Type type )
		{
			return !type.IsAbstract && !type.IsInterface;
		}
		public static Type FindInterfaceThatCloses( this Type type, Type openGeneric )
		{
			if ( !type.IsConcrete() )
				return null;

			if ( openGeneric.IsInterface )
			{
				foreach ( var interfaceType in type.GetInterfaces() )
				{
					if ( interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric )
					{
						return interfaceType;
					}
				}
			}
			else if ( type.BaseType.IsGenericType &&
					type.BaseType.GetGenericTypeDefinition() == openGeneric )
			{
				return type.BaseType;
			}

			return type.BaseType == typeof( object )
					   ? null
					   : FindInterfaceThatCloses( type.BaseType, openGeneric );
		}  

	}
}
