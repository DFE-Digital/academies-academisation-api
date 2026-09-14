using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeReligiousBodyConsultationPublicCommand(
	bool? trustConsultedReligiousBody,
	string? trustConsultedReligiousBodyNotConsultedReason) : IRequest<CommandResult>
{
	public bool? TrustConsultedReligiousBody { get; set; } = trustConsultedReligiousBody;
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; } = trustConsultedReligiousBodyNotConsultedReason;
}

public class SetSignificantChangeReligiousBodyConsultationCommand(
	int id,
	bool? trustConsultedReligiousBody,
	string? trustConsultedReligiousBodyNotConsultedReason)
	: SetSignificantChangeReligiousBodyConsultationPublicCommand(trustConsultedReligiousBody,
		trustConsultedReligiousBodyNotConsultedReason)
{
	public int Id { get; set; } = id;
}