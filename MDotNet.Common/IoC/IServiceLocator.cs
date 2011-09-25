using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDotNet.Common.IoC
{
	public interface IServiceLocator
	{
		object GetInstance( Type serviceType, String key );
		IEnumerable<object> GetAllInstances( Type serviceType );
	}
}