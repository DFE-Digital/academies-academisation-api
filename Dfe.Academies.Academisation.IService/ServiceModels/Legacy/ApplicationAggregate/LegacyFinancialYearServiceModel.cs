namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

public record LegacyFinancialYearServiceModel(
	DateTime? FYEndDate = null,
	decimal? RevenueCarryForward = null,
	bool? RevenueIsDeficit = null,
	string? RevenueStatusExplained = null,
	decimal? CapitalCarryForward = null,
	bool? CapitalIsDeficit = null,
	string? CapitalStatusExplained = null
);
