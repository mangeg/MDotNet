using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDotNet.WPF.MVVM.View
{
	public interface IViewAware
	{
		void AttachView( object view );
	}
}
