
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class GetProjectGroupQueryCommand : IRequest<ProjectGroupDto>
	{
		public string Urn { get; private set; }

		public GetProjectGroupQueryCommand(string urn)
		{
			Urn = urn;
		}
	}

	public class ProjectGroupDto
	{
		public int Urn { get; set; }

		public string TrustReference { get; set; }

		public List<int> ConversionsProjects { get; set; }
	}
}
