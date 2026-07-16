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

			RuleFor(x => x.TrustUkprn)
				.NotNull().WithMessage("TrustUkprn must not be null")
				.Length(8).WithMessage("TrustUkprn must be length 8");
		}
	}
}
