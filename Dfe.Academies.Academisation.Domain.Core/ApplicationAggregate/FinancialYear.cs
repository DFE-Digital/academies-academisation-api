namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public class FinancialYear
	{
		public FinancialYear(DateTime? FinancialYearEndDate = null,
			decimal? Revenue = null,
			RevenueType? RevenueStatus = null,		
			string? RevenueStatusExplained = null,
			string? RevenueStatusFileLink = null,
			decimal? CapitalCarryForward = null,
			RevenueType? CapitalCarryForwardStatus = null,
			string? CapitalCarryForwardExplained = null,
			string? CapitalCarryForwardFileLink = null)
		{
			this.FinancialYearEndDate = FinancialYearEndDate;
			this.Revenue = Revenue;
			this.RevenueStatus = RevenueStatus;
			this.RevenueStatusExplained = RevenueStatusExplained;
			this.RevenueStatusFileLink = RevenueStatusFileLink;
			this.CapitalCarryForward = CapitalCarryForward;
			this.CapitalCarryForwardStatus = CapitalCarryForwardStatus;
			this.CapitalCarryForwardExplained = CapitalCarryForwardExplained;
			this.CapitalCarryForwardFileLink = CapitalCarryForwardFileLink;
		}

		public DateTime? FinancialYearEndDate { get; init; }
		public decimal? Revenue { get; init; }
		public RevenueType? RevenueStatus { get; init; }
		public string? RevenueStatusExplained { get; init; }
		public string? RevenueStatusFileLink { get; init; }
		public decimal? CapitalCarryForward { get; init; }
		public RevenueType? CapitalCarryForwardStatus { get; init; }
		public string? CapitalCarryForwardExplained { get; init; }
		public string? CapitalCarryForwardFileLink { get; init; }

		public void Deconstruct(out DateTime? FinancialYearEndDate, out decimal? Revenue, out RevenueType? RevenueStatus, out string? RevenueStatusExplained, out string? RevenueStatusFileLink, out decimal? CapitalCarryForward, out RevenueType? CapitalCarryForwardStatus, out string? CapitalCarryForwardExplained, out string? CapitalCarryForwardFileLink)
		{
			FinancialYearEndDate = this.FinancialYearEndDate;
			Revenue = this.Revenue;
			RevenueStatus = this.RevenueStatus;
			RevenueStatusExplained = this.RevenueStatusExplained;
			RevenueStatusFileLink = this.RevenueStatusFileLink;
			CapitalCarryForward = this.CapitalCarryForward;
			CapitalCarryForwardStatus = this.CapitalCarryForwardStatus;
			CapitalCarryForwardExplained = this.CapitalCarryForwardExplained;
			CapitalCarryForwardFileLink = this.CapitalCarryForwardFileLink;
		}
	}
}
