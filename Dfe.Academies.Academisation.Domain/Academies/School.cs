namespace Dfe.Academies.Academisation.Domain.Academies;

public class School
{
	public long Id { get; set; }
	public string Name { get; set; }
	public virtual LocalAuthority LocalAuthority { get; set; }
	public long LocalAuthorityId { get; set; }
}
