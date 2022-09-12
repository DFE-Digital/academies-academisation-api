namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record Performance(
		bool? InspectedButReportNotPublished = null,
		string? InspectedButReportNotPublishedExplain = null, // provide inspection date && short summary in UI (1.5)
		bool? OngoingSafeguardingInvestigations = null,
		string? OngoingSafeguardingDetails = null
	);
}
