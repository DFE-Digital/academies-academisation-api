using Dfe.Academies.Academisation.IService.ServiceModels.RoleCapabilities; 

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IRoleCapabilitiesQueryService
	{
		RoleCapabilitiesModel GetRolesCapabilitiesAsync(List<string> roles);
	}
}
