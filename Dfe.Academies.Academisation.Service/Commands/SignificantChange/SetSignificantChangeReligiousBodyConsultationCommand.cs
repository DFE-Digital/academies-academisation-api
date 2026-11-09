using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeReligiousBodyConsultationPublicCommand(
	YesNoNotApplicableAnswer? trustConsultedReligiousBody,
	string? trustConsultedReligiousBodyNotConsultedReason,
	YesNoNotApplicableAnswer? religiousBodyRaisedObjection,
	string? religiousBodyRaisedObjectionReason,
	YesNoNotApplicableAnswer? religiousBodyConsentGranted,
	string? religiousBodyConsentNotGrantedReason,
	string? supportingEvidence) : IRequest<CommandResult>
{
	public YesNoNotApplicableAnswer? TrustConsultedReligiousBody { get; set; } = trustConsultedReligiousBody;
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; } = trustConsultedReligiousBodyNotConsultedReason;
	public YesNoNotApplicableAnswer? ReligiousBodyRaisedObjection { get; set; } = religiousBodyRaisedObjection;
	public string? ReligiousBodyRaisedObjectionReason { get; set; } = religiousBodyRaisedObjectionReason;
	public YesNoNotApplicableAnswer? ReligiousBodyConsentGranted { get; set; } = religiousBodyConsentGranted;
	public string? ReligiousBodyConsentNotGrantedReason { get; set; } = religiousBodyConsentNotGrantedReason;
	public string? SupportingEvidence { get; set; } = supportingEvidence;
}

public class SetSignificantChangeReligiousBodyConsultationCommand(
	int id,
	YesNoNotApplicableAnswer? trustConsultedReligiousBody,
	string? trustConsultedReligiousBodyNotConsultedReason,
	YesNoNotApplicableAnswer? religiousBodyRaisedObjection,
	string? religiousBodyRaisedObjectionReason,
	YesNoNotApplicableAnswer? religiousBodyConsentGranted,
	string? religiousBodyConsentNotGrantedReason,
	string? supportingEvidence)
	: SetSignificantChangeReligiousBodyConsultationPublicCommand(trustConsultedReligiousBody,
		trustConsultedReligiousBodyNotConsultedReason,
		religiousBodyRaisedObjection,
		religiousBodyRaisedObjectionReason,
		religiousBodyConsentGranted,
		religiousBodyConsentNotGrantedReason,
		supportingEvidence)
{
	public int Id { get; set; } = id;
}