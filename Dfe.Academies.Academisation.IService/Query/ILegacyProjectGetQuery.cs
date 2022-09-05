using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query;

public interface ILegacyProjectGetQuery
{
	Task<LegacyProjectServiceModel> Execute(int id);
}
