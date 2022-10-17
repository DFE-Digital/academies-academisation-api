namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record LeaseServiceModel(int LeaseId,
	int LeaseTerm, decimal RepaymentAmount, decimal InterestRate, decimal PaymentsToDate,
	string Purpose,
	string ValueOfAssets, string ResponsibleForAssets
);
