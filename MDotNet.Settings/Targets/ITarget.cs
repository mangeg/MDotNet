using System;

namespace MDotNet.Settings.Targets
{
	/// <summary>
	/// Interface for SettingsManager targets.
	/// </summary>
	public interface ITarget
	{
		/// <summary>
		/// Saves the value of this target.
		/// </summary>
		void Save();
		/// <summary>
		/// Loads the value for this target.
		/// </summary>
		void Load();
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		Object Value { get; set; }
	}
}