
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Dfe.Academies.Academisation.Service.CommandValidations;
using FluentValidation.TestHelper;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.CommandValidations
{
	public class CreateSignificantProjectCommandValidatorTests
	{
		private CreateSignificantProjectCommandValidator _validator = new();

		[Fact]
		public async Task Route_WhenEmpty_ShouldHaveValidationError()
		{
			var command = new CreateSignificantProjectCommand(1234, 1, string.Empty, "12345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldHaveValidationErrorFor(x => x.Route)
				.WithErrorMessage("Route must not be empty");
		}

		[Fact]
		public async Task TrustUkprn_WhenNull_ShouldHaveValidationError()
		{
			var command = new CreateSignificantProjectCommand(1234, 1, "Sponsored", null);

			var result = await _validator.TestValidateAsync(command);

			result.ShouldHaveValidationErrorFor(x => x.TrustUkprn)
				.WithErrorMessage("TrustUkprn must not be null");
		}

		[Theory]
		[InlineData("1234567")]
		[InlineData("123456789")]
		public async Task TrustUkprn_WhenLengthIsNot8_ShouldHaveValidationError(string trustUkprn)
		{
			var command = new CreateSignificantProjectCommand(1234, 1, "Sponsored", trustUkprn);

			var result = await _validator.TestValidateAsync(command);
			result.ShouldHaveValidationErrorFor(x => x.TrustUkprn)
				.WithErrorMessage("TrustUkprn must be length 8");
		}

		[Fact]
		public async Task TrustUkprn_WhenDoesNotStartWith1_ShouldHaveValidationError()
		{
			var command = new CreateSignificantProjectCommand(1234, 1, "Sponsored", "22345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldHaveValidationErrorFor(x => x.TrustUkprn)
				.WithErrorMessage("TrustUkprn must start with a 1");
		}		

		[Theory]
		[InlineData(99999)]
		[InlineData(200000)]
		[InlineData(1234)]
		public async Task Urn_WhenNot6DigitsStartingWith1_ShouldHaveValidationError(int urn)
		{
			var command = new CreateSignificantProjectCommand(urn, 1, "Sponsored", "12345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldHaveValidationErrorFor(x => x.Urn)
				.WithErrorMessage("Urn must be 6 digits and start with a 1");
		}

		[Theory]
		[InlineData(100000)]
		[InlineData(123456)]
		[InlineData(199999)]
		public async Task Urn_WhenValid_ShouldNotHaveValidationError(int urn)
		{
			var command = new CreateSignificantProjectCommand(urn, 1, "Sponsored", "12345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldNotHaveValidationErrorFor(x => x.Urn);
		}

		[Theory]
		[InlineData((byte)0)]
		[InlineData((byte)4)]
		public async Task Tier_WhenNotBetween1And3_ShouldHaveValidationError(byte tier)
		{
			var command = new CreateSignificantProjectCommand(123456, tier, "Sponsored", "12345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldHaveValidationErrorFor(x => x.Tier)
				.WithErrorMessage("Tier must be 1, 2 or 3");
		}

		[Theory]
		[InlineData((byte)1)]
		[InlineData((byte)2)]
		[InlineData((byte)3)]
		public async Task Tier_WhenValid_ShouldNotHaveValidationError(byte tier)
		{
			var command = new CreateSignificantProjectCommand(123456, tier, "Sponsored", "12345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldNotHaveValidationErrorFor(x => x.Tier);
		}

		[Fact]
		public async Task WhenValid_ShouldNotHaveValidationError()
		{
			var command = new CreateSignificantProjectCommand(123456, 1, "Sponsored", "12345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldNotHaveAnyValidationErrors();
		}
	}
}