using System.Runtime.Serialization;
using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

public class UpdateLoanCommand : IRequest<CommandResult>
{
	[DataMember]
	public int ApplicationId { get; set; }
	[DataMember]
	public int SchoolId { get; set; }
	[DataMember]
	public int LoanId { get; set; }
	[DataMember]
	public decimal Amount { get; set; }
	[DataMember]
	public string Purpose { get; set; }
	[DataMember]
	public string Provider { get; set; }
	[DataMember]
	public decimal InterestRate { get; set; }
	[DataMember]
	public string Schedule { get; set; }
}
