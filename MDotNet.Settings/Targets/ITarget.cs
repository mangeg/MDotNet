using System;

namespace MDotNet.Settings.Targets
{
	public interface ITarget
	{
		void Save();
		void Load();
		Object Value { get; set; }
	}
}