using Dfe.Academies.Academisation.Domain.Core.RoleCapabilitiesAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.RoleCapabilities
{
	public class RoleCapabilitiesModel(List<RoleCapability> capabilities)
	{
		public List<RoleCapability> Capabilities { get; set; } = capabilities;
	}
}
