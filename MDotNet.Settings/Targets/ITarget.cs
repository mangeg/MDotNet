namespace MDotNet.Settings.Targets
{
	using System;

	/// <summary>
	/// Interface for SettingsManager targets.
	/// </summary>
	public interface ITarget
	{
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		Object Value { get; set; }

		/// <summary>
		/// Saves the value of this target.
		/// </summary>
		void Save();

		/// <summary>
		/// Loads the value for this target.
		/// </summary>
		void Load();
	}
}