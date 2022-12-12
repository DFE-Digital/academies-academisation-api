namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate
{
	public record ProjectNote(
		string Summary,
		string Note,
		string Author,
		int ProjectId);
}
