using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate
{
	public interface IProjectGroup
	{
		int Id { get; }
		string TrustReference{ get; }

		User? AssignedUser { get; }

		DateTime CreatedOn { get; }
		string ReferenceNumber { get; }

		void SetAssignedUser(Guid userId, string fullName, string emailAddress);
		void SetProjectReference(int id);
	}
}
