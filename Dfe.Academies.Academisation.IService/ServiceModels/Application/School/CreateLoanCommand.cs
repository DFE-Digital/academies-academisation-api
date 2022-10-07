namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

public record CreateLoanCommand(int applicationId, int schoolId, decimal amount, string purpose, string provider, decimal interestRate, string schedule);
