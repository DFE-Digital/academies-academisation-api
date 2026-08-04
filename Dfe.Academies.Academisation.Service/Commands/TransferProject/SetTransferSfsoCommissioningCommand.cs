using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferSfsoCommissioningCommand : IRequest<CommandResult>
	{
		public SetTransferSfsoCommissioningCommand(int urn, string? sfsoCommissioningOverview)
		{
			Urn = urn;
			SfsoCommissioningOverview = sfsoCommissioningOverview;
		}

		public int Urn { get; set; }
		public string? SfsoCommissioningOverview { get; set; }
	}
}