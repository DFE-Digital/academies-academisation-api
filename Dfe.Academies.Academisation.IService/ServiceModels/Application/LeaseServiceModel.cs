namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record LeaseServiceModel(int leaseId,
	int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate,
	string purpose,
	string valueOfAssets, string responsibleForAssets
);
