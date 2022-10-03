﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class SetJoinTrustDetailsValidator
	: AbstractValidator<Application>
{
	public SetJoinTrustDetailsValidator()
	{
		RuleFor(x => x.ApplicationType)
			.Must(x => false)
			.When(x => x.ApplicationType != ApplicationType.JoinAMat)
			.WithMessage("Can only update join trust deails on join a MAT applications")
			.OverridePropertyName(nameof(Application.ApplicationType));
	}
}
