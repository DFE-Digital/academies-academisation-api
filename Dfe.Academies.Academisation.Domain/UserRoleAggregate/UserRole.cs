using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.UserRoleAggregate;

namespace Dfe.Academies.Academisation.Domain.UserRoleAggregate
{
	public class UserRole : Entity, IUserRole, IAggregateRoot
	{
		public string RoleId { private set; get; } = string.Empty;
		public User? AssignedUser { private set; get; }
		public bool IsEnabled { private set; get; } = true;

		public UserRole(string roleId, bool isEnabled, DateTime createdOn)
		{ 
			RoleId = roleId;
			IsEnabled = isEnabled;
			CreatedOn = createdOn;
		}
		public void SetAssignedUser(Guid userId, string fullName, string emailAddress)
		{
			AssignedUser = new User(userId, fullName, emailAddress);
		}

		public void SetRole(RoleId roleId, DateTime lastModifiedOn, bool isEnabled = true)
		{
			RoleId = roleId.GetStringValue();
			IsEnabled = isEnabled;
			LastModifiedOn = lastModifiedOn;
		}
	}
}
