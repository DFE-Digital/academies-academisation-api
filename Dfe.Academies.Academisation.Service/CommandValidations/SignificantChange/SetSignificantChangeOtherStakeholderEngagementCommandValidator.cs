using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.SignificantChange;

public class SetSignificantChangeOtherStakeholderEngagementCommandValidator
	: AbstractValidator<SetSignificantChangeOtherStakeholderEngagementCommand>
{
	public SetSignificantChangeOtherStakeholderEngagementCommandValidator()
	{
		RuleFor(command => command.TrustConsultedStakeholders)
			.NotNull()
			.WithMessage("Select an option");
		RuleFor(command => command.TrustConsultedStakeholdersNotConsultedReason)
			.Must(reason => !string.IsNullOrWhiteSpace(reason))
			.When(command => command.TrustConsultedStakeholders is false)
			.WithMessage("Add a reason");
		RuleFor(command => command.WereThereAnyObjectionsRaised)
			.NotNull()
			.WithMessage("Select an option");
		RuleFor(command => command.StakeholderObjectionsDetails)
			.Must(details => !string.IsNullOrWhiteSpace(details))
			.When(command => command.WereThereAnyObjectionsRaised is true)
			.WithMessage("Add details");
		RuleFor(command => command.RelevantStakeholdersNotifiedOfAdmissionsChanges)
			.Must(answer => answer is YesNoNotApplicableAnswer.Yes
				or YesNoNotApplicableAnswer.No
				or YesNoNotApplicableAnswer.NotApplicable)
			.WithMessage("Select a valid option");
		RuleFor(command => command.RelevantStakeholdersNotifiedOfAdmissionsChangesReason)
			.Must(reason => !string.IsNullOrWhiteSpace(reason))
			.When(command => command.RelevantStakeholdersNotifiedOfAdmissionsChanges is YesNoNotApplicableAnswer.No)
			.WithMessage("Add a reason");
	}
}
