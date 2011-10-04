namespace MDotNet.Common.OldIoC.IoC
{
	using System.Collections.Generic;

	public interface IRegistry
	{
		void Register( IEnumerable<ICoponentRegistration> registrations );
	}
}