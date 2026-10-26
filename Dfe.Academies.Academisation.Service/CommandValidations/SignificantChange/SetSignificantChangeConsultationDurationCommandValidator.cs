using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.SignificantChange;

public class SetSignificantChangeConsultationDurationCommandValidator
	: AbstractValidator<SetSignificantChangeConsultationDurationCommand>
{
	public SetSignificantChangeConsultationDurationCommandValidator()
	{
		RuleFor(command => command.ConsultationLastedMinimumThreeWeeks)
			.Must(answer => answer is ConsultationDurationAnswer.Yes
				or ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut
				or ConsultationDurationAnswer.No)
			.WithMessage("Select a valid option");
		RuleFor(command => command.ConsultationDurationNotMetReason)
			.Must(reason => !string.IsNullOrWhiteSpace(reason))
			.When(command => command.ConsultationLastedMinimumThreeWeeks is ConsultationDurationAnswer.No)
			.WithMessage("Add a reason");
	}
}
