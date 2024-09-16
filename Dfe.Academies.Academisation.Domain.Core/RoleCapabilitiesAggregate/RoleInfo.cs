namespace Dfe.Academies.Academisation.Domain.Core.RoleCapabilitiesAggregate
{
	public interface IRoleInfo
	{
		List<RoleCapability> GetRoleCapabilities(string roleId);
	}

	public class RoleInfo(Dictionary<string, string> roleIds) : IRoleInfo
	{
		private RoleId GetEnum(string roleId)
		{
			var role = roleIds.FirstOrDefault(kvp => kvp.Value == roleId).Key;
			return (RoleId)Enum.Parse(typeof(RoleId), role);
		}
		public List<RoleCapability> GetRoleCapabilities(string roleId)
		{
			switch (GetEnum(roleId))
			{
				case RoleId.Support:
					return SupportRoleCapabilities();
				case RoleId.ConversionCreation:
					return ConversionCreationRoleCapabilities();
				case RoleId.TransferCreation:
					return TransferCreationRoleCapabilities();
				default:
					return [];
			}
		}
		private static List<RoleCapability> TransferCreationRoleCapabilities() => [
				RoleCapability.CreateTransferProject
			];

		private static List<RoleCapability> ConversionCreationRoleCapabilities() => [
				RoleCapability.CreateConversionProject
			];

		private static List<RoleCapability> SupportRoleCapabilities() 
			=> ConversionCreationRoleCapabilities().Concat(TransferCreationRoleCapabilities()).Concat([
				RoleCapability.DeleteConversionProject,
				RoleCapability.DeleteTransferProject
			]).Distinct().ToList();
	}
}
