using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ITransferProjectQueryService
	{
		Task<AcademyTransferProjectResponse?> GetByUrn(int Urn);
		Task<AcademyTransferProjectResponse?> GetById(int id);

	}
}
