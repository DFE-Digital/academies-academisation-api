namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate
{
	public record NewProject(NewProjectSchool? School, NewProjectTrust? Trust, string HasSchoolApplied, string HasPreferredTrust);
	public record NewProjectTrust(string Name, string ReferenceNumber);
	public record NewProjectSchool(string Name, int Urn, DateTime? ProposedAcademyOpeningDate, bool? PartOfPfiScheme, string? LocalAuthorityName, string? Region);
}
