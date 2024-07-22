using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate
{
	public class ProjectGroup : Entity, IProjectGroup, IAggregateRoot
	{
		public string TrustReference { private set; get; }

		public User? AssignedUser { private set; get; }

		public string? ReferenceNumber { private set; get; }

		public void SetAssignedUser(Guid userId, string fullName, string emailAddress)
		{
			throw new NotImplementedException();
		}

		public void SetProjectReference(int id)
		{
			throw new NotImplementedException();
		}
	}
}
