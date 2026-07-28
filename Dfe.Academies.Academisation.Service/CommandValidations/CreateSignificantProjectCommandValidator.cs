using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations
{
	public class CreateSignificantProjectCommandValidator : AbstractValidator<CreateSignificantProjectCommand>
	{
		public CreateSignificantProjectCommandValidator()
		{

			RuleFor(x => x.Route)
				.NotEmpty()
				.WithMessage("Route must not be empty");

			// Tier should be 1,2 or 3
			RuleFor(x => x.Tier)
				.InclusiveBetween((byte)1, (byte)3)
				.WithMessage("Tier must be 1, 2 or 3");

			// Urn: 6 digits starting with 1
			RuleFor(x => x.Urn.ToString())
				.NotNull().WithMessage("Urn must not be null")
				.Length(6).WithMessage("Urn must be length 6")
				.Must(m => m.ToString().StartsWith('1')).WithMessage("Urn must start with a 1");

			// TrustUkprn: 8 digits starting with 1
			RuleFor(x => x.TrustUkprn)
				.NotNull().WithMessage("TrustUkprn must not be null")
				.Length(8).WithMessage("TrustUkprn must be length 8")
				.Must(m => m != null && m.StartsWith('1')).WithMessage("TrustUkprn must start with a 1");
		}
	}
}
