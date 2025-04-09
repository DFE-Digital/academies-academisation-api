using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
    public class SetPublicEqualityDutyCommand : IRequest<CommandResult>
	{
		public SetPublicEqualityDutyCommand(int id,
			string publicEqualityDutyImpact,
			string publicEqualityDutyReduceImpactReason,
			bool publicEqualityDutySectionComplete)
		{
			Id = id;
			PublicEqualityDutyImpact = publicEqualityDutyImpact;
			PublicEqualityDutyReduceImpactReason = publicEqualityDutyReduceImpactReason;
			PublicEqualityDutySectionComplete = publicEqualityDutySectionComplete;
		}

		public int Id { get; set; }
		public string PublicEqualityDutyImpact { get; set; }
		public string PublicEqualityDutyReduceImpactReason { get; set; }
		public bool PublicEqualityDutySectionComplete { get; set; }
	}
}
