using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate
{
	public class FormAMatProject : Entity, IFormAMatProject, IAggregateRoot
	{
		private FormAMatProject(string proposedTrustName, string applicationReference, DateTime createdOn)
		{
			ProposedTrustName = proposedTrustName;
			ApplicationReference = applicationReference;
			CreatedOn = createdOn;
		}
		public string ProposedTrustName { get; private set; }

		public string ApplicationReference { get; private set; }
		public User? AssignedUser { get; private set; }

		public static FormAMatProject Create(string proposedTrustName, string applicationReference, DateTime createdOn)
		{
			return new FormAMatProject(proposedTrustName, applicationReference, createdOn) { };
		}
		public void SetAssignedUser(Guid userId, string fullName, string emailAddress)
		{
			// Update the respective properties in the Details object
			this.AssignedUser = new User(userId, fullName, emailAddress);

			// Update the LastModifiedOn property to the current time to indicate the object has been modified
			this.LastModifiedOn = DateTime.UtcNow;
		}
	}
}
