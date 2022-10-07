namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

public record UpdateLeaseCommand(int applicationId, int schoolId, int leaseId, int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets);
