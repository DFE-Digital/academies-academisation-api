using Dfe.Academies.Academisation.Service.Extensions;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Extensions.ListExtensions
{
	public class TypespaceExtensionsTests
	{
		[Theory]
		[InlineData(new string[] {""}, "")]
		[InlineData(new string[] {"Test"}, "Test")]
		[InlineData(new string[] {"Value one", "Value two"}, "Value one, Value two")]
		[InlineData(new string[] {"Value one", " Value two "}, "Value one,  Value two ")]
		[InlineData(new string[] {"Value one", "Value two", "", "Value three", ""}, "Value one, Value two, Value three")]
		[InlineData(new string[] {"Value one", "Value two", " ", "Value three"}, "Value one, Value two, Value three")]
		public void JoinNonEmpty_ReturnsExpectedResult(IEnumerable<string> input, string expected)
		{
			// Arrange
			// Act
			var result = input.JoinNonEmpty(", ");

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
