using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.IService.Queries
{
    public interface IApplicationGetQuery
    {
        Task<ApplicationServiceModel> Execute(int id);
    }
}
