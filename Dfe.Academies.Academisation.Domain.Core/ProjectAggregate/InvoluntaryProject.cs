namespace Dfe.Academies.Academisation.Core.ProjectAggregate
{
	public record InvoluntaryProject(ProjectSchool? School, ProjectTrust? Trust);
	public record ProjectTrust(string Name, string ReferenceNumber);
	public record ProjectSchool(string Name, int Urn, DateTime? OpeningDate, bool PartOfPfiScheme);
}
