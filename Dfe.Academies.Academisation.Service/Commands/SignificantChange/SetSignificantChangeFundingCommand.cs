using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeFundingPublicCommand(
		FundingAnswer? FundingAnswer,
		string? AdditionalInformation,
		string? SupportingEvidenceSharepointLink);


	public record SetSignificantChangeFundingCommand(
		int Id,
		FundingAnswer? FundingAnswer,
		string? AdditionalInformation,
		string? SupportingEvidenceSharepointLink)
		: SetSignificantChangeFundingPublicCommand(FundingAnswer, AdditionalInformation, SupportingEvidenceSharepointLink),
			IRequest<CommandResult>;
}
