using Dfe.Academies.Academisation.Domain.Core.RoleCapabilitiesAggregate; 
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.RoleCapabilities;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class RoleCapabilitiesQueryService(IRoleInfo roleInfo): IRoleCapabilitiesQueryService
	{
		public RoleCapabilitiesModel GetRolesCapabilitiesAsync(List<string> roles)
		{
			var capabilities = new List<RoleCapability>();
			roles.ForEach(role => capabilities.AddRange(roleInfo.GetRoleCapabilities(role)));
			return new RoleCapabilitiesModel(capabilities.Distinct().ToList());
		}
	} 
}
