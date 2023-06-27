namespace Dfe.Academies.Academisation.Service.UnitTest.Extensions;
using Dfe.Academies.Academisation.Service.Extensions;
using Xunit;

/// <summary>
/// The string extensions tests.
/// </summary>
public class StringExtensionsTests
{
	/// <summary>
	/// The to title case tests.
	/// </summary>
	public class ToTitleCaseTests
	{
		/// <summary>
		/// Givens the string_ should convert to title case.
		/// </summary>
		/// <param name="givenString">The given string.</param>
		/// <param name="expectedStringAsTitleCase">The expected string as title case.</param>
		[Theory]
		[InlineData(null, null)]
		[InlineData("", "")]
		[InlineData("All Title Case", "All Title Case")]
		[InlineData("all lower case", "All Lower Case")]
		public void GivenString_ShouldConvertToTitleCase(string givenString, string expectedStringAsTitleCase)
		{
			string? result = givenString?.ToTitleCase();
			Assert.Equal(expectedStringAsTitleCase, result);
		}
	}
}
