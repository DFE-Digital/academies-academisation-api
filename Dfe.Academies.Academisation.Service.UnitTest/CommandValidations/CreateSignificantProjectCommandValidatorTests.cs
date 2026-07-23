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

		[Fact]
		public async Task WhenValid_ShouldNotHaveValidationError()
		{
			var command = new CreateSignificantProjectCommand(1234, 1, "Sponsored", "12345678");

			var result = await _validator.TestValidateAsync(command);

			result.ShouldNotHaveAnyValidationErrors();
		}
	}
}
