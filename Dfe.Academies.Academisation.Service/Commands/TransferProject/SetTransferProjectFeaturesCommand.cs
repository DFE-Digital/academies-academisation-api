using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectFeaturesCommand : IRequest<CommandResult>
	{
		public int Id { get; set; }
		public string TypeOfTransfer { get; set; }
		public string WhoInitiatedTheTransfer { get; set; }
		public bool? IsCompleted { get; set; }
	}
}
