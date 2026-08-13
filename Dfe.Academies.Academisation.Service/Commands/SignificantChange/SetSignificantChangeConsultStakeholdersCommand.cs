using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeConsultStakeholdersPublicCommand(
	bool? trustConsultedStakeholders,
	string? trustConsultedStakeholdersNotConsultedReason) : IRequest<CommandResult>
{
	public bool? TrustConsultedStakeholders { get; set; } = trustConsultedStakeholders;
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; } = trustConsultedStakeholdersNotConsultedReason;
}

public class SetSignificantChangeConsultStakeholdersCommand(
	int id,
	bool? trustConsultedStakeholders,
	string? trustConsultedStakeholdersNotConsultedReason)
	: SetSignificantChangeConsultStakeholdersPublicCommand(trustConsultedStakeholders, trustConsultedStakeholdersNotConsultedReason)
{
	public int Id { get; set; } = id;
}