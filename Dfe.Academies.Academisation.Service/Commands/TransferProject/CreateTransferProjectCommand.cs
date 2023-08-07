using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject;

public class CreateTransferProjectCommand : IRequest<CreateResult>
{
	
	public CreateTransferProjectCommand(string outgoingTrustUkprn, string incomingTrustUkprn, List<string> transferringAcademyUkprns)
	{
		OutgoingTrustUkprn = outgoingTrustUkprn;
		IncomingTrustUkprn = incomingTrustUkprn;
		TransferringAcademyUkprns = transferringAcademyUkprns;
	}
	
	public List<string> TransferringAcademyUkprns { get; set; }
	public string OutgoingTrustUkprn { get; set; }
	public string IncomingTrustUkprn { get; }
}
