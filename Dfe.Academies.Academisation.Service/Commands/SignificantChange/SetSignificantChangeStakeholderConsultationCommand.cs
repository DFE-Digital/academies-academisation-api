using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeStakeholderConsultationPublicCommand(
	bool? trustConsultedStakeholders,
	string? trustConsultedStakeholdersNotConsultedReason) : IRequest<CommandResult>
{
	public bool? TrustConsultedStakeholders { get; set; } = trustConsultedStakeholders;
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; } = trustConsultedStakeholdersNotConsultedReason;
}

public class SetSignificantChangeStakeholderConsultationCommand(
	int id,
	bool? trustConsultedStakeholders,
	string? trustConsultedStakeholdersNotConsultedReason)
	: SetSignificantChangeStakeholderConsultationPublicCommand(trustConsultedStakeholders, trustConsultedStakeholdersNotConsultedReason)
{
	public int Id { get; set; } = id;
}