using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

internal class CreateConversionApplicationValidator : AbstractValidator<IContributorDetails>
{
	public CreateConversionApplicationValidator()
	{
		RuleFor(details => details.OtherRoleName)
			.NotEmpty()
			.When(details => details.Role == ContributorRole.Other)
			.WithMessage("OtherRoleName must be specified if Role is Other");

		RuleFor(details => details.EmailAddress)
			.EmailAddress();
	}
}
