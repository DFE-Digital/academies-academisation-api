namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange;

public class SignificantChangeProjectDto
{
	public int Id { get; set; }
	public int Urn { get; set; }
	public string SchoolName { get; set; } = string.Empty;
	public byte Tier { get; set; }
	public string TrustName { get; set; } = string.Empty;
	public string TrustUkprn { get; set; } = string.Empty;
	public Guid? AssignedUserId { get; set; }
	public string? AssignedUserFullName { get; set; }
	public string? AssignedUserEmailAddress { get; set; }
	public string TypeOfSignificantChange { get; set; } = string.Empty;
	public string Status { get; set; } = string.Empty;
}