namespace Dfe.Academies.Academisation.IService.ServiceModels.Application.School
{
	public record DeleteLeaseCommand(int applicationId, int schoolId, int leaseId);
}
