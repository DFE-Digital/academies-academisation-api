using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate
{
	public interface IFormAMatProject
	{
		int Id { get; }
		string ProposedTrustName { get; }

		string ApplicationReference { get; }
		User? AssignedUser { get; }

		void SetAssignedUser(Guid userId, string fullName, string emailAddress);
	}
}
