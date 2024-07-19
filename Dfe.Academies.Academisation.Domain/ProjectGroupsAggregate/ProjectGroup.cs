using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate
{
	public class ProjectGroup : Entity, IProjectGroup, IAggregateRoot
	{
		public string TrustReference { private set; get; } = string.Empty;

		public User? AssignedUser { private set; get; }

		public string ReferenceNumber { private set; get; } = string.Empty;

		public void SetAssignedUser(Guid userId, string fullName, string emailAddress)
		{
			throw new NotImplementedException();
		}

		public void SetProjectReference(int id)
		{
			throw new NotImplementedException();
		}

		public static ProjectGroup Create(string trustReference, string referenceNumber, DateTime createdOn)
		{
			return new ProjectGroup(trustReference, referenceNumber, createdOn);
		}

		private ProjectGroup(string trustReference, string referenceNumber, DateTime createdOn)
		{
			TrustReference = trustReference;
			ReferenceNumber = referenceNumber;
			CreatedOn = createdOn;
		}
	}
}
