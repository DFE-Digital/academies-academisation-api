using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectLegalRequirementsCommand : IRequest<CommandResult>
	{
		public int Id { get; set; }
		public string OutgoingTrustConsent { get; set; }
		public string IncomingTrustAgreement { get; set; }
		public string DiocesanConsent { get; set; }
		public bool? IsCompleted { get; set; }
	}
}
