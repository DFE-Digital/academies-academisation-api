namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record FinancialYear(
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
