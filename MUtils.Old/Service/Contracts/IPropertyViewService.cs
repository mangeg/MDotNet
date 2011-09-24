using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using MEFedMVVM.Common;
using MEFedMVVM.ViewModelLocator;

namespace MUtils.Service.Contracts
{
	public interface IPropertyViewService
	{
	}

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( ServiceType.Both, typeof( IPropertyViewService ) )]
	class PropertyViewService : IPropertyViewService
	{
		public PropertyViewService()
		{
			
		}
	}
}
