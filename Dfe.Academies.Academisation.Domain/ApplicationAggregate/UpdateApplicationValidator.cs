﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

internal class UpdateApplicationValidator 
	: AbstractValidator<(
		ApplicationType type,
		ApplicationStatus status,
		Dictionary<int, ContributorDetails> contributors,
		Dictionary<int, SchoolDetails> schools,
		Application existing)>
{
	public UpdateApplicationValidator()
	{
		RuleFor(x => x.existing.ApplicationStatus)
			.Equal(ApplicationStatus.InProgress)
			.WithMessage("Application must be InProgress to use Update")
			.OverridePropertyName(nameof(Application.ApplicationStatus));

		RuleFor(x => x.status)
			.Must(x => false)
			.When(x => x.status != x.existing.ApplicationStatus)
			.WithMessage("Cannot change the status using this operation. To submit, use the Submit operation.")
			.OverridePropertyName(nameof(Application.ApplicationStatus));

		RuleFor(x => x.type)
			.Must(x => false)
			.When(x => x.type != x.existing.ApplicationType)
			.WithMessage("Cannot change the type using this operation of an existing Application. Please start a new one.")
			.OverridePropertyName(nameof(Application.ApplicationType));
	}
}
