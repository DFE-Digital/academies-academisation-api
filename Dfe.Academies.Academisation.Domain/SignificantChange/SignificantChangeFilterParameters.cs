namespace Dfe.Academies.Academisation.Domain.SignificantChange
{
	public class SignificantChangeFilterParameters
	{
		public List<FilterValueDisplay> Statuses { get; set; } = [];
		public List<FilterValueDisplay> AssignedUsers { get; set; } = [];
		public List<FilterValueDisplay> Tiers { get; set; } = [];
		public List<FilterValueDisplay> Routes { get; set; } = [];
	}

    public record FilterValueDisplay(string Value, string Display);
}