using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeFundingPublicCommand(
		FundingAnswer? FundingAnswer,
		string? AdditionalInformation,
		string? SupportingEvidence);


	public record SetSignificantChangeFundingCommand(
		int Id,
		FundingAnswer? FundingAnswer,
		string? AdditionalInformation,
		string? SupportingEvidence)
		: SetSignificantChangeFundingPublicCommand(FundingAnswer, AdditionalInformation, SupportingEvidence),
			IRequest<CommandResult>;
}
