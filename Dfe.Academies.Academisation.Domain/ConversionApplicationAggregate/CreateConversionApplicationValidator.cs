using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate
{
	internal class CreateConversionApplicationValidator : AbstractValidator<IContributorDetails>
	{
		public CreateConversionApplicationValidator()
		{
			RuleFor(details => details.OtherRoleName)
				.NotNull()
				.When(details => details.Role == ContributorRole.Other)
				.WithMessage("OtherRoleName must be specified if Role is Other");
		}
	}
}
