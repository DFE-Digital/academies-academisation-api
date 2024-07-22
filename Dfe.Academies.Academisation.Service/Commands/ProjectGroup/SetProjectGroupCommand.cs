using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommand : IRequest<CommandResult>
	{
		public required string TrustReference { get; set; }
		public required string Urn { get; set; }
		public List<int> ConversionProjectsUrns { get; set; }
	}
}
