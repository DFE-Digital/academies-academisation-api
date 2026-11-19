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
	string? supportingEvidenceSharepointLink) : IRequest<CommandResult>
{
	public YesNoNotApplicableAnswer? TrustConsultedReligiousBody { get; set; } = trustConsultedReligiousBody;
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; } = trustConsultedReligiousBodyNotConsultedReason;
	public YesNoNotApplicableAnswer? ReligiousBodyRaisedObjection { get; set; } = religiousBodyRaisedObjection;
	public string? ReligiousBodyRaisedObjectionReason { get; set; } = religiousBodyRaisedObjectionReason;
	public YesNoNotApplicableAnswer? ReligiousBodyConsentGranted { get; set; } = religiousBodyConsentGranted;
	public string? ReligiousBodyConsentNotGrantedReason { get; set; } = religiousBodyConsentNotGrantedReason;
	public string? SupportingEvidenceSharepointLink { get; set; } = supportingEvidenceSharepointLink;
}

public class SetSignificantChangeReligiousBodyConsultationCommand(
	int id,
	YesNoNotApplicableAnswer? trustConsultedReligiousBody,
	string? trustConsultedReligiousBodyNotConsultedReason,
	YesNoNotApplicableAnswer? religiousBodyRaisedObjection,
	string? religiousBodyRaisedObjectionReason,
	YesNoNotApplicableAnswer? religiousBodyConsentGranted,
	string? religiousBodyConsentNotGrantedReason,
	string? supportingEvidenceSharepointLink)
	: SetSignificantChangeReligiousBodyConsultationPublicCommand(trustConsultedReligiousBody,
		trustConsultedReligiousBodyNotConsultedReason,
		religiousBodyRaisedObjection,
		religiousBodyRaisedObjectionReason,
		religiousBodyConsentGranted,
		religiousBodyConsentNotGrantedReason,
		supportingEvidenceSharepointLink)
{
	public int Id { get; set; } = id;
}