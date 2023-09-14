namespace Dfe.Academies.Academisation.Domain.Academies;

public class Trust
{
	public long Id { get; set; }
	public string? GroupUId { get; set; }
	public string? GroupId { get; set; }
	public string? ReferenceId { get; set; }
	public string? Name { get; set; }
	public string? CompaniesHouseNumber { get; set; }
	public string? UKPRN { get; set; }
	
	public virtual Region? Region { get; set; }
	public long? RegionId { get; set; }
}
