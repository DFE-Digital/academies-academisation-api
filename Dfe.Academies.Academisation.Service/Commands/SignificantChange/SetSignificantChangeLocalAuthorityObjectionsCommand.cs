using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeLocalAuthorityObjectionsPublicCommand(
	bool? localAuthorityRaisedObjections,
	string? localAuthorityObjectionsFurtherInformation,
	string? supportingEvidenceLink) : IRequest<CommandResult>
{
	public bool? LocalAuthorityRaisedObjections { get; set; } = localAuthorityRaisedObjections;
	public string? LocalAuthorityObjectionsFurtherInformation { get; set; } = localAuthorityObjectionsFurtherInformation;
	public string? SupportingEvidenceLink { get; set; } = supportingEvidenceLink;
}

public class SetSignificantChangeLocalAuthorityObjectionsCommand(
	int id,
	bool? localAuthorityRaisedObjections,
	string? localAuthorityObjectionsFurtherInformation,
	string? supportingEvidenceLink)
	: SetSignificantChangeLocalAuthorityObjectionsPublicCommand(localAuthorityRaisedObjections,
		localAuthorityObjectionsFurtherInformation,
		supportingEvidenceLink)
{
	public int Id { get; set; } = id;
}
