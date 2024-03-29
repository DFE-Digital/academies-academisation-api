﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations;

internal class UpdateSchoolValidator : AbstractValidator<SchoolDetailsPair>
{
	public UpdateSchoolValidator()
	{
		//107147 bug fix - remove validator so school can me changed on an update
		//RuleFor(x => x)
		//	.Must(x => x.existing.Urn == x.updated.Urn)
		//	.OverridePropertyName(nameof(SchoolDetails.Urn));
	}
}

internal record SchoolDetailsPair(SchoolDetails updated, SchoolDetails existing);
