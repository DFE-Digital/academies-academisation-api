using System.Data;
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

		public static FormAMatProject Create(string proposedTrustName, string applicationReference, DateTime createdOn)
		{
			return new FormAMatProject(proposedTrustName, applicationReference, createdOn) { };
		}
	}
}
