namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

public record UpdateLoanCommand(int applicationId, int schoolId, int loanId, decimal amount, string purpose, string provider, decimal interestRate, string schedule);
