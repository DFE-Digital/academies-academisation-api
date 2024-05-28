using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject;

public class CreateTransferProjectCommand : IRequest<CreateResult>
{
	
	public CreateTransferProjectCommand(string outgoingTrustUkprn, string outgoingTrustName, List<TransferringAcademyDto> transferringAcademies, bool? isFormAMat)
	{
		OutgoingTrustUkprn = outgoingTrustUkprn;
		OutgoingTrustName = outgoingTrustName;
		TransferringAcademies = transferringAcademies;
		IsFormAMat = isFormAMat;
	}
	
	public List<TransferringAcademyDto> TransferringAcademies { get; set; }
	public string OutgoingTrustUkprn { get; set; }
	public string OutgoingTrustName { get; set; }
	public bool? IsFormAMat { get; set; }
}
