using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

internal class CreateConversionApplicationValidator : AbstractValidator<ContributorDetails>
{
	public CreateConversionApplicationValidator()
	{
		RuleFor(details => details.OtherRoleName)
			.NotEmpty()
			.When(details => details.Role == ContributorRole.Other)
			.WithMessage("OtherRoleName must be specified if Role is Other");

		RuleFor(details => details.EmailAddress)
			.EmailAddress();

		RuleFor(details => details.FirstName)
			.NotEmpty();

		RuleFor(details => details.LastName)
			.NotEmpty();
	}
}