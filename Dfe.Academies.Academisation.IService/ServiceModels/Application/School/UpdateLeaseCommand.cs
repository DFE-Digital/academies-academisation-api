using System.Runtime.Serialization;
using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

public class UpdateLeaseCommand : IRequest<CommandResult>
{
	[DataMember]
	public int ApplicationId { get; set; }
	[DataMember]
	public int SchoolId { get; set; }
	[DataMember]
	public int LeaseId { get; set; }
	[DataMember]
	public string LeaseTerm { get; set; }
	[DataMember]
	public decimal RepaymentAmount { get; set; }
	[DataMember]
	public decimal InterestRate { get; set; }
	[DataMember]
	public decimal PaymentsToDate { get; set; }
	[DataMember]
	public string Purpose { get; set; }
	[DataMember]
	public string ValueOfAssets { get; set; }
	[DataMember]
	public string ResponsibleForAssets { get; set; }
	
	public override string ToString()
	{
		return $"Application ID: {ApplicationId}\n School ID: {SchoolId}\n Lease ID: {LeaseId}";
	}
}
