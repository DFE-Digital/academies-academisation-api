using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject;

public class CreateTransferProjectCommand : IRequest<CreateResult>
{
	
	public CreateTransferProjectCommand(string outgoingTrustUkprn, string outgoingTrustName, string? incomingTrustUkprn, string incomingTrustName, List<string> transferringAcademyUkprns)
	{
		OutgoingTrustUkprn = outgoingTrustUkprn;
		OutgoingTrustName = outgoingTrustName;
		IncomingTrustUkprn = incomingTrustUkprn;
		IncomingTrustName = incomingTrustName;
		TransferringAcademyUkprns = transferringAcademyUkprns;
	}
	
	public List<string> TransferringAcademyUkprns { get; set; }
	public string OutgoingTrustUkprn { get; set; }
	public string? IncomingTrustUkprn { get; set; }
	public string OutgoingTrustName { get; set; }
	public string IncomingTrustName { get; set; }
}
