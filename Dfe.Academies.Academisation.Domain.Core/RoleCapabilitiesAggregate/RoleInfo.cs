using System.Drawing;

namespace Dfe.Academies.Academisation.Domain.Core.RoleCapabilitiesAggregate
{
	public interface IRoleInfo
	{
		List<RoleCapability> GetRoleCapabilities(string roleId);
	}

	public class RoleInfo(Dictionary<string, string> roleIds) : IRoleInfo
	{
		private (bool, RoleId) GetEnum(string roleId)
		{
			var isSuccess = Enum.TryParse((string?)roleIds.FirstOrDefault(kvp => kvp.Value == roleId).Key, out RoleId enumRole);
			return (isSuccess, enumRole);
		}
		public List<RoleCapability> GetRoleCapabilities(string roleId)
		{
			var (isSuccess, enumRole) = GetEnum(roleId);
			if (isSuccess && Enum.IsDefined(typeof(RoleId), enumRole))
			{
				switch (enumRole)
				{
					case RoleId.SuperAdmin:
						return SuperAdminRoleCapabilities();
					case RoleId.ConversionCreation:
						return ConversionCreationRoleCapabilities();
					case RoleId.TransferCreation:
						return TransferCreationRoleCapabilities();
					default:
						return [];
				}
			}
			return [];
		}
		private static List<RoleCapability> TransferCreationRoleCapabilities() => [
				RoleCapability.CreateTransferProject
			];

		private static List<RoleCapability> ConversionCreationRoleCapabilities() => [
				RoleCapability.CreateConversionProject
			];

		private static List<RoleCapability> SuperAdminRoleCapabilities() 
			=> ConversionCreationRoleCapabilities().Concat(TransferCreationRoleCapabilities()).Concat([
				RoleCapability.DeleteConversionProject,
				RoleCapability.DeleteTransferProject
			]).Distinct().ToList();
	}
}
