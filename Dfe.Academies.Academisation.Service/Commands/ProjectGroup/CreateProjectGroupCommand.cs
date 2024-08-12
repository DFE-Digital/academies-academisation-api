using MediatR;
using Dfe.Academies.Academisation.Core;
namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommand(string trustName, string trustReferenceNumber, string trustUkprn, List<int> conversionProjectIds, List<int>? transferProjectIds = null) : IRequest<CreateResult>
	{
		public string TrustName { get; set; } = trustName;
		public string TrustReferenceNumber { get; set; } = trustReferenceNumber;
		public string TrustUkprn { get; set; } = trustUkprn;	
		public List<int> ConversionProjectIds { get; set; } = conversionProjectIds;
		public List<int>? TransferProjectIds { get; set; } = transferProjectIds;
	}
}
