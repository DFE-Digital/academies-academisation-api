namespace Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate
{
	public interface IRoleInfo
	{
		string GetId(RoleId roleId);
		RoleId GetEnum(string roleId);
	}

	public class RoleInfo(Dictionary<string, string> roleIds) : IRoleInfo
	{ 
		public string GetId(RoleId roleId)
		{
			var key = roleId.ToString();
			return roleIds.FirstOrDefault(kvp => kvp.Key == key && kvp.Value != "").Value;
		}

		public RoleId GetEnum(string roleId)
		{
			var role = roleIds.FirstOrDefault(kvp => kvp.Value == roleId).Key;
			return (RoleId)Enum.Parse(typeof(RoleId), role);
		}
	}
}
