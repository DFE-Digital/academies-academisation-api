namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School
{
	public record DeleteLoanCommand(int applicationId, int schoolId, int loanId);
}
