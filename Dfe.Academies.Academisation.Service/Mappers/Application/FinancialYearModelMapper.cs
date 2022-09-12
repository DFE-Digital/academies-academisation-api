using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class FinancialYearModelMapper
	{
		internal static FinancialYearServiceModel ToServiceModel(this FinancialYear financialYear)
		{
			return new FinancialYearServiceModel
			{
				FinancialYearEndDate = financialYear.FinancialYearEndDate,
				Revenue = financialYear.Revenue,
				RevenueStatus = financialYear.RevenueStatus,
				RevenueStatusExplained = financialYear.RevenueStatusExplained,
				CapitalCarryForward = financialYear.CapitalCarryForward,
				CapitalCarryForwardStatus = financialYear.CapitalCarryForwardStatus,
				CapitalCarryForwardExplained = financialYear.CapitalCarryForwardExplained,
				CapitalCarryForwardFileLink = financialYear.CapitalCarryForwardFileLink
			};
		}

		internal static FinancialYear ToDomain(this FinancialYearServiceModel serviceModel)
		{
			return new FinancialYear
			{
				FinancialYearEndDate = serviceModel.FinancialYearEndDate,
				Revenue = serviceModel.Revenue,
				RevenueStatus = serviceModel.RevenueStatus,
				RevenueStatusExplained = serviceModel.RevenueStatusExplained,
				CapitalCarryForward = serviceModel.CapitalCarryForward,
				CapitalCarryForwardStatus = serviceModel.CapitalCarryForwardStatus,
				CapitalCarryForwardExplained = serviceModel.CapitalCarryForwardExplained,
				CapitalCarryForwardFileLink = serviceModel.CapitalCarryForwardFileLink
			};
		}
	}
}
