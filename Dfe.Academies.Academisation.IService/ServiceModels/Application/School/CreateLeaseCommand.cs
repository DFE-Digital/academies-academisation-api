namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

public record CreateLeaseCommand(int applicationId, int schoolId, int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets);
