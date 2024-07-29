using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommand(string trustUrn, List<int> conversionProjectIds) : IRequest<CommandResult>
	{
		public string TrustReference { get; set; } = trustUrn;
		public string GroupReferenceNumber { get; set; } = string.Empty;
		public List<int> ConversionProjectIds { get; set; } = conversionProjectIds;
	}
}
