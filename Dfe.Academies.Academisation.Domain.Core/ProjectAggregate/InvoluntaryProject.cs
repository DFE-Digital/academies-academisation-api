namespace Dfe.Academies.Academisation.Core.ProjectAggregate
{
	public record InvoluntaryProject(InvoluntaryProjectSchool? School, InvoluntaryProjectTrust? Trust);
	public record InvoluntaryProjectTrust(string Name, string ReferenceNumber);
	public record InvoluntaryProjectSchool(string Name, int Urn, DateTime? OpeningDate, bool? PartOfPfiScheme);
}
