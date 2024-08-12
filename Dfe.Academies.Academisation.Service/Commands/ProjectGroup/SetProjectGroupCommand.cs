using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommand(List<int> conversionProjectIds, List<int>? transferProjectIds) : IRequest<CommandResult>
	{
		public string GroupReferenceNumber { get; set; } = string.Empty;
		public List<int> ConversionProjectIds { get; set; } = conversionProjectIds;
		public List<int>? TransferProjectIds { get; set; } = transferProjectIds;
	}
}
