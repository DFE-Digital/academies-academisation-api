namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeProjectDatesResponse
{
	public DateTime? ProposedDecisionDate { get; set; }
	public DateTime? ProposedChangeDate { get; set; }
	public string Status { get; set; } = string.Empty;
}
