namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record PerformanceServiceModel(
		bool? InspectedButReportNotPublished = null,
		string? InspectedButReportNotPublishedExplain = null, // provide inspection date && short summary in UI (1.5)
		bool? OngoingSafeguardingInvestigations = null,
		string? OngoingSafeguardingDetails = null
		);
}
