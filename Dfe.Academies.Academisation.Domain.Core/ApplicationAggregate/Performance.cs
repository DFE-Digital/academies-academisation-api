namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record Performance(
		// TODO MR:- props as per V2 specification - part of additional info legacy model
		bool? SchoolOngoingSafeguardingInvestigations = null,
		string? SchoolOngoingSafeguardingDetails = null,
		bool? SchoolAdInspectedButReportNotPublished = null,
		string? SchoolAdInspectedButReportNotPublishedExplain = null
	);
}
