using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

internal class UpdateSchoolValidator : AbstractValidator<SchoolDetailsPair>
{
	public UpdateSchoolValidator()
	{
		RuleFor(x => x)
			.Must(x => x.existing.Urn == x.updated.Urn)
			.OverridePropertyName(nameof(SchoolDetails.Urn));
	}
}

internal record SchoolDetailsPair(SchoolDetails updated, SchoolDetails existing);
