using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeLocalAuthorityObjectionsPublicCommand(
	bool? localAuthorityRaisedObjections,
	string? localAuthorityObjectionsFurtherInformation,
	string? localAuthoritySupportingEvidenceLinkSharepointLink) : IRequest<CommandResult>
{
	public bool? LocalAuthorityRaisedObjections { get; set; } = localAuthorityRaisedObjections;
	public string? LocalAuthorityObjectionsFurtherInformation { get; set; } = localAuthorityObjectionsFurtherInformation;
	public string? LocalAuthoritySupportingEvidenceLinkSharepointLink { get; set; } = localAuthoritySupportingEvidenceLinkSharepointLink;
}

public class SetSignificantChangeLocalAuthorityObjectionsCommand(
	int id,
	bool? localAuthorityRaisedObjections,
	string? localAuthorityObjectionsFurtherInformation,
	string? localAuthoritySupportingEvidenceLinkSharepointLink)
	: SetSignificantChangeLocalAuthorityObjectionsPublicCommand(localAuthorityRaisedObjections,
		localAuthorityObjectionsFurtherInformation,
		localAuthoritySupportingEvidenceLinkSharepointLink)
{
	public int Id { get; set; } = id;
}
