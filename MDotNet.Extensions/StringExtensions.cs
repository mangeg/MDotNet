namespace MDotNet.Extensions
{
	using System;
	using System.Globalization;

	/// <summary>
	/// Extensions for the String class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// String.Format extension.
		/// </summary>
		/// <param name="fmt">The format.</param>
		/// <param name="args">The args.</param>
		/// <returns>The formatted string.</returns>
		public static String FormatWidth( this String fmt, params object[] args ) { return String.Format( fmt, args ); }

		/// <summary>
		/// Capitalizes the specified string.
		/// </summary>
		/// <param name="s">The string.</param>
		/// <returns>The string with starting capital letter.</returns>
		public static String Capitalize( this String s )
		{
			if ( String.IsNullOrWhiteSpace( s ) )
				return s;

			var len = s.Length;

			if ( len == 1 )
				return s.ToUpper();

			if ( len > 1 )
			{
				return char.ToUpper( s[ 0 ] ) + s.Substring( 1 );
			}

			return s;
		}

		/// <summary>
		/// Titles case the string.
		/// </summary>
		/// <param name="s">The string.</param>
		/// <returns>A title cased string.</returns>
		public static String TitleCase( this String s ) { return CultureInfo.InvariantCulture.TextInfo.ToTitleCase( s ); }
	}
}