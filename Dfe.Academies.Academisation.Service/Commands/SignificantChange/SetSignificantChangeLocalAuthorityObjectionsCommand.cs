using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeLocalAuthorityObjectionsPublicCommand(
	bool? localAuthorityRaisedObjections,
	string? localAuthorityObjectionsFurtherInformation,
	string? localAuthoritySupportingEvidenceLink) : IRequest<CommandResult>
{
	public bool? LocalAuthorityRaisedObjections { get; set; } = localAuthorityRaisedObjections;
	public string? LocalAuthorityObjectionsFurtherInformation { get; set; } = localAuthorityObjectionsFurtherInformation;
	public string? LocalAuthoritySupportingEvidenceLink { get; set; } = localAuthoritySupportingEvidenceLink;
}

public class SetSignificantChangeLocalAuthorityObjectionsCommand(
	int id,
	bool? localAuthorityRaisedObjections,
	string? localAuthorityObjectionsFurtherInformation,
	string? localAuthoritySupportingEvidenceLink)
	: SetSignificantChangeLocalAuthorityObjectionsPublicCommand(localAuthorityRaisedObjections,
		localAuthorityObjectionsFurtherInformation,
		localAuthoritySupportingEvidenceLink)
{
	public int Id { get; set; } = id;
}
