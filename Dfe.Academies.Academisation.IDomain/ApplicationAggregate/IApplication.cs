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
		IEnumerable<UpdateSchoolParameter> schools
		);

	/// <summary>
	/// Be careful of using this method outside the Domain Layer - this only submits the Application but doesn't create the Project.
	/// Use the IApplicationSubmissionService to submit the Application and (conditionally) create a Project. 
	/// </summary>
	CommandResult Submit();
}
