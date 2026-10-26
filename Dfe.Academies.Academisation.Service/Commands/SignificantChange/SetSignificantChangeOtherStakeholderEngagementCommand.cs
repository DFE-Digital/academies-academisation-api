using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeOtherStakeholderEngagementPublicCommand(
	bool? trustConsultedStakeholders,
	string? trustConsultedStakeholdersNotConsultedReason,
	bool? wereThereAnyObjectionsRaised,
	string? stakeholderObjectionsDetails,
	YesNoNotApplicableAnswer? relevantStakeholdersNotifiedOfAdmissionsChanges,
	string? relevantStakeholdersNotifiedOfAdmissionsChangesReason,
	string? supportingEvidence) : IRequest<CommandResult>
{
	public bool? TrustConsultedStakeholders { get; set; } = trustConsultedStakeholders;
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; } = trustConsultedStakeholdersNotConsultedReason;
	public bool? WereThereAnyObjectionsRaised { get; set; } = wereThereAnyObjectionsRaised;
	public string? StakeholderObjectionsDetails { get; set; } = stakeholderObjectionsDetails;
	public YesNoNotApplicableAnswer? RelevantStakeholdersNotifiedOfAdmissionsChanges { get; set; } = relevantStakeholdersNotifiedOfAdmissionsChanges;
	public string? RelevantStakeholdersNotifiedOfAdmissionsChangesReason { get; set; } = relevantStakeholdersNotifiedOfAdmissionsChangesReason;
	public string? SupportingEvidence { get; set; } = supportingEvidence;
}

public class SetSignificantChangeOtherStakeholderEngagementCommand(
	int id,
	bool? trustConsultedStakeholders,
	string? trustConsultedStakeholdersNotConsultedReason,
	bool? wereThereAnyObjectionsRaised,
	string? stakeholderObjectionsDetails,
	YesNoNotApplicableAnswer? relevantStakeholdersNotifiedOfAdmissionsChanges,
	string? relevantStakeholdersNotifiedOfAdmissionsChangesReason,
	string? supportingEvidence)
	: SetSignificantChangeOtherStakeholderEngagementPublicCommand(
		trustConsultedStakeholders,
		trustConsultedStakeholdersNotConsultedReason,
		wereThereAnyObjectionsRaised,
		stakeholderObjectionsDetails,
		relevantStakeholdersNotifiedOfAdmissionsChanges,
		relevantStakeholdersNotifiedOfAdmissionsChangesReason,
		supportingEvidence)
{
	public int Id { get; set; } = id;
}
