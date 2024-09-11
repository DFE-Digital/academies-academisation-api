using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.UserRole
{
	public class RoleCapabilitiesModel(List<RoleCapability> capabilities)
	{
		public List<RoleCapability> Capabilities { get; set; } = capabilities;
	}
}
