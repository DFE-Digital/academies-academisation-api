namespace Dfe.Academies.Academisation.Domain.SignificantChange
{
	public enum SignificantChangeStatus
	{
		PreDecision,
		Approved,
		ApprovedWithConditions,
		Deferred,
		Declined,
		Withdrawn
	}

	public static class SignificantChangeStatusExtensions
	{
		public static string ToDisplayName(this SignificantChangeStatus status) => status switch
		{
			SignificantChangeStatus.PreDecision => "Pre decision",
			SignificantChangeStatus.Approved => "Approved",
			SignificantChangeStatus.ApprovedWithConditions => "Approved with conditions",
			SignificantChangeStatus.Deferred => "Deferred",
			SignificantChangeStatus.Declined => "Declined",
			SignificantChangeStatus.Withdrawn => "Withdrawn",
			_ => status.ToString()
		};
	}
}