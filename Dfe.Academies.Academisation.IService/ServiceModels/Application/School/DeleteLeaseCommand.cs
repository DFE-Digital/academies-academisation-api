using System.Runtime.Serialization;
using MediatR;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

public class DeleteLeaseCommand : IRequest<bool>
{
	[DataMember]
	public int ApplicationId { get; set; }
	[DataMember]
	public int SchoolId { get; set; }
	[DataMember]
	public int LeaseId { get; set; }
}
