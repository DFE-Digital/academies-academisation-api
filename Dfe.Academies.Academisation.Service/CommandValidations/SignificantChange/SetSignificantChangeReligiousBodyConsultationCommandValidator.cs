using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.SignificantChange;

public class SetSignificantChangeReligiousBodyConsultationCommandValidator
	: AbstractValidator<SetSignificantChangeReligiousBodyConsultationCommand>
{
	public SetSignificantChangeReligiousBodyConsultationCommandValidator()
	{
		RuleFor(command => command.TrustConsultedReligiousBody)
			.Must(IsValidAnswer)
			.WithMessage("Select a valid option");
		RuleFor(command => command.TrustConsultedReligiousBodyNotConsultedReason)
			.Must(reason => !string.IsNullOrWhiteSpace(reason))
			.When(command => command.TrustConsultedReligiousBody is YesNoNotApplicableAnswer.No)
			.WithMessage("Add a reason");
		RuleFor(command => command.ReligiousBodyRaisedObjection)
			.Must(IsValidAnswer)
			.WithMessage("Select a valid option");
		RuleFor(command => command.ReligiousBodyRaisedObjectionReason)
			.Must(reason => !string.IsNullOrWhiteSpace(reason))
			.When(command => command.ReligiousBodyRaisedObjection is YesNoNotApplicableAnswer.No)
			.WithMessage("Add a reason");
		RuleFor(command => command.ReligiousBodyConsentGranted)
			.Must(IsValidAnswer)
			.WithMessage("Select a valid option");
		RuleFor(command => command.ReligiousBodyConsentNotGrantedReason)
			.Must(reason => !string.IsNullOrWhiteSpace(reason))
			.When(command => command.ReligiousBodyConsentGranted is YesNoNotApplicableAnswer.No)
			.WithMessage("Add a reason");
	}

	private static bool IsValidAnswer(YesNoNotApplicableAnswer? answer)
	{
		return answer is YesNoNotApplicableAnswer.Yes
			or YesNoNotApplicableAnswer.No
			or YesNoNotApplicableAnswer.NotApplicable;
	}
}
