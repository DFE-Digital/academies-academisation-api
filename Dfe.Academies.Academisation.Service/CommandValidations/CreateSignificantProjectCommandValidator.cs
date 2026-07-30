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

			RuleFor(x => x.Tier)
				.InclusiveBetween((byte)1, (byte)3)
				.WithMessage("Tier must be 1, 2 or 3");

			RuleFor(x => x.Urn)
				.InclusiveBetween(100000, 199999)
				.WithMessage("Urn must be 6 digits and start with a 1");

			RuleFor(x => x.TrustUkprn)
				.NotNull().WithMessage("TrustUkprn must not be null")
				.Length(8).WithMessage("TrustUkprn must be length 8")
				.Must(m => m != null && m.StartsWith('1')).WithMessage("TrustUkprn must start with a 1");
		}
	}
}
