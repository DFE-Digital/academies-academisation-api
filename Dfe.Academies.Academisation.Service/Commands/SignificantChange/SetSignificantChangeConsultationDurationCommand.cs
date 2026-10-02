using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeConsultationDurationPublicCommand(
	ConsultationDurationAnswer? consultationLastedMinimumThreeWeeks,
	string? consultationDurationNotMetReason) : IRequest<CommandResult>
{
	public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; } = consultationLastedMinimumThreeWeeks;
	public string? ConsultationDurationNotMetReason { get; set; } = consultationDurationNotMetReason;
}

public class SetSignificantChangeConsultationDurationCommand(
	int id,
	ConsultationDurationAnswer? consultationLastedMinimumThreeWeeks,
	string? consultationDurationNotMetReason)
	: SetSignificantChangeConsultationDurationPublicCommand(consultationLastedMinimumThreeWeeks, consultationDurationNotMetReason)
{
	public int Id { get; set; } = id;
}