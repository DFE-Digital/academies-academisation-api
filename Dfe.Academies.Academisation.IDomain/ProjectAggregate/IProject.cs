﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;


namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate;

public interface IProject
{
	public int Id { get; }

	public Guid? SchoolSharePointId { get; }
	public Guid? ApplicationSharePointId { get; }
	public int? FormAMatProjectId { get; }
	public int? ProjectGroupId { get; } 
	public DateTime? ReadOnlyDate { get; }
	public bool ProjectSentToComplete { get; }
	DateTime CreatedOn { get; }
	DateTime LastModifiedOn { get; }

	public IReadOnlyCollection<IProjectNote> Notes { get; }
	public IReadOnlyCollection<ISchoolImprovementPlan> SchoolImprovementPlans { get; }

	public ProjectDetails Details { get; }

	public CommandResult Update(ProjectDetails detailsToUpdate);

	public void SetExternalApplicationForm(bool ExternalApplicationFormSaved, string ExternalApplicationFormUrl);
	public void SetIncomingTrust(string trustReferrenceNumber, string trustName);
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
	public void SetAssignedUser(Guid userId, string fullName, string emailAddress);
	public void SetFormAMatProjectReference(int FormAMAtProjectId);
	public void SetDeletedAt();

	public void SetPerformanceData(string? keyStage2PerformanceAdditionalInformation, string? keyStage4PerformanceAdditionalInformation, string? keyStage5PerformanceAdditionalInformation, string? educationalAttendanceAdditionalInformation);
	void SetFormAMatProjectId(int id);
	void SetProjectGroupId(int? id);
	void SetRoute(string route);
	void AddNote(string subject, string note, string author, DateTime date);
	void RemoveNote(int id);
	void AddSchoolImprovementPlan(List<SchoolImprovementPlanArranger> arrangedBy,
			string? arrangedByOther,
			string providedBy,
			DateTime startDate,
			SchoolImprovementPlanExpectedEndDate expectedEndDate,
			DateTime? expectedEndDateOther,
			SchoolImprovementPlanConfidenceLevel confidenceLevel,
			string? planComments);
	void UpdateSchoolImprovementPlan(int id, List<SchoolImprovementPlanArranger> arrangedBy, string? arrangedByOther, string providedBy, DateTime startDate, SchoolImprovementPlanExpectedEndDate expectedEndDate, DateTime? expectedEndDateOther, SchoolImprovementPlanConfidenceLevel confidenceLevel, string? planComments);

	public void SetProjectDates(DateTime? advisoryBoardDate, DateTime? previousAdvisoryBoard, DateTime? proposedConversionDate, bool? projectDatesSectionComplete, List<ReasonChange>? reasonsChanged, string? changedBy);
	void SetProjectSentToComplete();
	
	
}
