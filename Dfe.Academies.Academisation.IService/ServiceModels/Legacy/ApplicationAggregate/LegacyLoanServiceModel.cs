namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

public record LegacyLoanServiceModel(
	decimal? SchoolLoanAmount,
	string? SchoolLoanPurpose = null,
	string? SchoolLoanProvider = null,
	string? SchoolLoanInterestRate = null,
	string? SchoolLoanSchedule = null
);
