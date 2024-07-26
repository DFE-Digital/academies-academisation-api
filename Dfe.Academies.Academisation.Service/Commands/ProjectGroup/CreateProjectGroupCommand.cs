using MediatR;
using Dfe.Academies.Academisation.Core;
namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommand(string trustReferenceNumber, string trustUkprn, List<int> conversionProjectIds) : IRequest<CreateResult>
	{
		public string TrustReferenceNumber { get; set; } = trustReferenceNumber;
		public string TrustUkprn { get; set; } = trustUkprn;

		public List<int> ConversionProjectIds { get; set; } = conversionProjectIds;
	}
}
