namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

public record LegacyLeaseServiceModel(
	decimal SchoolLeaseRepaymentValue,
	decimal SchoolLeaseInterestRate,
	decimal SchoolLeasePaymentToDate,
	string? SchoolLeaseTerm = null,
	string? SchoolLeasePurpose = null,
	string? SchoolLeaseValueOfAssets = null,
	string? SchoolLeaseResponsibleForAssets = null
);
