﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;


namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate;

public interface IProject
{
	public int Id { get; }

	DateTime CreatedOn { get; }
	DateTime LastModifiedOn { get; }

	public IReadOnlyCollection<IProjectNote> Notes { get; }

	public ProjectDetails Details { get; }

	public CommandResult Update(ProjectDetails detailsToUpdate);

	public void SetExternalApplicationForm(bool ExternalApplicationFormSaved, string ExternalApplicationFormUrl);
	public void SetSchoolOverview(
		string publishedAdmissionNumber,
		string viabilityIssues,
		string partOfPfiScheme,
		string financialDeficit,
		decimal? numberOfPlacesFundedFor,
		decimal? numberOfResidentialPlaces,
		decimal? numberOfFundedResidentialPlaces,
		string pfiSchemeDetails,
		decimal? distanceFromSchoolToTrustHeadquarters,
		string distanceFromSchoolToTrustHeadquartersAdditionalInformation,
		string memberOfParliamentNameAndParty,
		bool? pupilsAttendingGroupPermanentlyExcluded,
		bool? pupilsAttendingGroupMedicalAndHealthNeeds,
		bool? pupilsAttendingGroupTeenageMums
		);
}
