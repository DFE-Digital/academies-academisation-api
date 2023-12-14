namespace Dfe.Academies.Academisation.Domain.ProjectAggregate
{
	public class ProjectFilterParameters
	{
		public List<string>? Statuses { get; set; }
		public List<string>? AssignedUsers { get; set; }
		public List<string>? LocalAuthorities { get; set; }
		public List<string>? AdvisoryBoardDates { get; set; }
	}
}
