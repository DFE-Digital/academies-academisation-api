using MediatR;
using Dfe.Academies.Academisation.Core;
namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommand(string trustReferenceNumber, string trustUkprn, List<int> conversionsUrns) : IRequest<CreateResult>
	{
		public string TrustReferenceNumber { get; set; } = trustReferenceNumber;
		public string TrustUkprn { get; set; } = trustUkprn;

		public List<int> ConversionsUrns { get; set; } = conversionsUrns;
	}
}
