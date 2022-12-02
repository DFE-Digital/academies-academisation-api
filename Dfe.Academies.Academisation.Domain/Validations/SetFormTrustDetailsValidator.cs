using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations;

public class SetFormTrustDetailsValidator
	: AbstractValidator<Application>
{
	public SetFormTrustDetailsValidator()
	{
		RuleFor(x => x.ApplicationType)
			.Must(x => false)
			.When(x => x.ApplicationType != ApplicationType.FormAMat)
			.WithMessage("Can only update fomr trust deails on form a MAT applications")
			.OverridePropertyName(nameof(Application.ApplicationType));
	}
}
