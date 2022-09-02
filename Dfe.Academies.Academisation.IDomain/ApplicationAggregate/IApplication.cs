using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface IApplication
{
	int ApplicationId { get; }
	DateTime CreatedOn { get; }
	DateTime LastModifiedOn { get; }

	ApplicationType ApplicationType { get; }
	ApplicationStatus ApplicationStatus { get; }

	IReadOnlyCollection<IContributor> Contributors { get; }

	IReadOnlyCollection<ISchool> Schools { get; }

	void SetIdsOnCreate(int applicationId, int conversionId);

	CommandResult Update(
		ApplicationType applicationType,
		ApplicationStatus applicationStatus,
		IEnumerable<KeyValuePair<int, ContributorDetails>> contributors,
		IEnumerable<KeyValuePair<int, SchoolDetails>> schools);

	CommandResult Submit();
}
