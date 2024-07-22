using MediatR;
using Dfe.Academies.Academisation.Core;
namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommand(string trustReference, List<int> conversionProjects) : IRequest<CommandResult>
	{
		public string TrustReference { get; set; } = trustReference;

		public List<int> ConversionProjectsUrns { get; set; } = conversionProjects;
	}
}
