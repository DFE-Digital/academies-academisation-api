using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommand(string trustUrn, List<int> conversionsUrns) : IRequest<CommandResult>
	{
		public string TrustUrn { get; set; } = trustUrn;
		public string Urn { get; set; } = string.Empty;
		public List<int> ConversionsUrns { get; set; } = conversionsUrns;
	}
}
