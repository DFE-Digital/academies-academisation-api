namespace Dfe.Academies.Academisation.Domain.Academies;

public class LocalAuthority
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
	public ICollection<School> Schools { get; set; }
}
