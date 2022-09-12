using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record FinancialYearServiceModel(
		DateTime? FinancialYearEndDate = null,
		decimal? Revenue = null,
		RevenueType? RevenueStatus = null,
		string? RevenueStatusExplained = null,
		string? RevenueStatusFileLink = null,
		decimal? CapitalCarryForward = null,
		RevenueType? CapitalCarryForwardStatus = null,
		string? CapitalCarryForwardExplained = null,
		string? CapitalCarryForwardFileLink = null
		);
}
