using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.Validations;

public class UpdateApplicationValidator
	: AbstractValidator<(ApplicationType type, ApplicationStatus status, IEnumerable<KeyValuePair<int, ContributorDetails>> contributors, IEnumerable<UpdateSchoolParameter> schools, Application existing)>
{
	public UpdateApplicationValidator()
	{
		RuleFor(x => x.existing.ApplicationStatus)
			.Equal(ApplicationStatus.InProgress)
			.WithMessage("Application must be InProgress to use Update")
			.OverridePropertyName(nameof(Application.ApplicationStatus));

		RuleFor(x => x.existing.ApplicationType)
			.Must(x => false)
			.When(x => x.type == ApplicationType.FormASat && x.schools.Count() > 1)
			.WithMessage("Cannot add more than one school when forming a single academy trust.")
			.OverridePropertyName(nameof(Application.ApplicationType));

		RuleFor(x => x.existing.ApplicationType)
			.Must(x => false)
			.When(x => x.type == ApplicationType.JoinAMat && x.schools.Count() > 1)
			.WithMessage("Cannot add more than one school when joining a multi academy trust.")
			.OverridePropertyName(nameof(Application.ApplicationType));

		RuleFor(x => x.existing.ApplicationType)
			.Must(x => false)
			.When(x => x.type == ApplicationType.FormAMat && x.schools.Count() <= 1)
			.WithMessage("Must add more than one school when forming a multi academy trust.")
			.OverridePropertyName(nameof(Application.ApplicationType));

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

		RuleFor(x => x.contributors)
			.Must(x => false)
			.When(x => x.contributors
				.Select(s => s.Key)
				.Where(id => id != 0)
				.Except(x.existing.Contributors.Select(s => s.Id))
				.Any())
			.WithMessage("Added Contributors must have an Id of zero.")
			.OverridePropertyName(nameof(Application.Contributors));

		RuleForEach(x => x.contributors.Join(
			x.existing.Contributors,
			updated => updated.Key,
			existing => existing.Id,
			(updated, existing) => new ContributorPair(updated.Value, existing.Details)))
			.SetValidator(new UpdateContributorValidator())
			.OverridePropertyName(nameof(Contributor));

		RuleFor(x => x.schools)
			.Must(x => false)
			.When(x => x.schools
				.Select(s => s.Id)
				.Where(id => id != 0)
				.Except(x.existing.Schools.Select(s => s.Id))
				.Any())
			.WithMessage("Added Schools must have an Id of zero.")
			.OverridePropertyName(nameof(Application.Schools));

		RuleForEach(x => x.schools.Select(s => s.SchoolDetails))
			.SetValidator(new SchoolValidator())
			.OverridePropertyName(nameof(SchoolDetails));

		RuleForEach(x => x.schools.Join(
			x.existing.Schools,
			updated => updated.Id,
			existing => existing.Id,
			(updated, existing) => new SchoolDetailsPair(updated.SchoolDetails, existing.Details)))
			.SetValidator(new UpdateSchoolValidator())
			.OverridePropertyName(nameof(School));
	}
}
