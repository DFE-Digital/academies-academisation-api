using System.Runtime.Serialization;
using MediatR;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;


//Mediatr isn't currently used but the IRequest interface will make it easier to manually inject validators into the command handlers
//This makes the refactoring later with moving commands to mediatr simple

public class CreateLeaseCommand : IRequest<bool>
{
	[DataMember]
	public int ApplicationId { get; set; }
	[DataMember]
	public int SchoolId { get; set; }
	[DataMember]
	public int LeaseTerm { get; set; }
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
}
