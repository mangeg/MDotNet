namespace MDotNet.Common.OldIoC.IoC
{
	using System;
	using System.Collections.Generic;

	public interface IServiceLocator
	{
		object GetInstance( Type serviceType, String key );
		IEnumerable<object> GetAllInstances( Type serviceType );
	}
}