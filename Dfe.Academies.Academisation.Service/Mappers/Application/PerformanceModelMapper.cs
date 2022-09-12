using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class PerformanceModelMapper
	{
		internal static PerformanceServiceModel ToServiceModel(this Performance performance)
		{
			return new PerformanceServiceModel
			{
				InspectedButReportNotPublished = performance.InspectedButReportNotPublished,
				InspectedButReportNotPublishedExplain = performance.InspectedButReportNotPublishedExplain,
				OngoingSafeguardingInvestigations = performance.OngoingSafeguardingInvestigations,
				OngoingSafeguardingDetails = performance.OngoingSafeguardingDetails
			};
		}

		internal static Performance ToDomain(this PerformanceServiceModel serviceModel)
		{
			return new Performance
			{
				InspectedButReportNotPublished = serviceModel.InspectedButReportNotPublished,
				InspectedButReportNotPublishedExplain = serviceModel.InspectedButReportNotPublishedExplain,
				OngoingSafeguardingInvestigations = serviceModel.OngoingSafeguardingInvestigations,
				OngoingSafeguardingDetails = serviceModel.OngoingSafeguardingDetails
			};
		}
	}
}
