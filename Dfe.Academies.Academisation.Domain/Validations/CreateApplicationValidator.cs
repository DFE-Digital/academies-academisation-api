﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations;

internal class CreateApplicationValidator : AbstractValidator<ContributorDetails>
{
	public CreateApplicationValidator()
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
