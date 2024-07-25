using MediatR;
using Dfe.Academies.Academisation.Core;
namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommand(string trustUrn, List<int> conversionsUrns) : IRequest<CreateResult>
	{
		public string TrustUrn { get; set; } = trustUrn;

		public List<int> ConversionsUrns { get; set; } = conversionsUrns;
	}
}
