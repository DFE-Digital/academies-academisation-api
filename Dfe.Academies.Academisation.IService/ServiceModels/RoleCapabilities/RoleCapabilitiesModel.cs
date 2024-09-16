using Dfe.Academies.Academisation.Domain.Core.RoleCapabilitiesAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.UserRole
{
	public class RoleCapabilitiesModel(List<RoleCapability> capabilities)
	{
		public List<RoleCapability> Capabilities { get; set; } = capabilities;
	}
}
