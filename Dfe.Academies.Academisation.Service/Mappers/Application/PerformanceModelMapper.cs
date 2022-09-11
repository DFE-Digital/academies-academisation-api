using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class PerformanceModelMapper
	{
		internal static PerformanceServiceModel ToServiceModel(this Performance performance)
		{
			//TODO MR:-
			//bool? InspectedButReportNotPublished = null,
			//string? InspectedButReportNotPublishedExplain = null, // provide inspection date && short summary in UI (1.5)
			//bool? OngoingSafeguardingInvestigations = null,
			//string? OngoingSafeguardingDetails = null
		}

		internal static Performance ToDomain(this PerformanceServiceModel serviceModel)
		{
			//TODO MR:-
		}
	}
}
