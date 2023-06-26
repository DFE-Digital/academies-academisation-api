namespace Dfe.Academies.Academisation.Domain.Core.ProjectAggregate
{
	public record SponsoredProject(SponsoredProjectSchool? School, SponsoredProjectTrust? Trust);
	public record SponsoredProjectTrust(string Name, string ReferenceNumber);
	public record SponsoredProjectSchool(string Name, int Urn, DateTime? ProposedAcademyOpeningDate, bool? PartOfPfiScheme, string? LocalAuthorityName, string? Region);
}
