using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
    public class SetTransferPublicEqualityDutyCommand : IRequest<CommandResult>
	{
		public SetTransferPublicEqualityDutyCommand(int urn,
			string publicEqualityDutyImpact,
			string publicEqualityDutyReduceImpactReason,
			bool publicEqualityDutySectionComplete)
		{
			Urn = urn;
			PublicEqualityDutyImpact = publicEqualityDutyImpact;
			PublicEqualityDutyReduceImpactReason = publicEqualityDutyReduceImpactReason;
			PublicEqualityDutySectionComplete = publicEqualityDutySectionComplete;
		}

		public int Urn { get; set; }
		public string PublicEqualityDutyImpact { get; set; }
		public string PublicEqualityDutyReduceImpactReason { get; set; }
		public bool PublicEqualityDutySectionComplete { get; set; }
	}
}
