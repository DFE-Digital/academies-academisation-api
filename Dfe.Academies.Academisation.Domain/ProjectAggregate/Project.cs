﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class Project : IProject
{
	private Project(ProjectDetails projectDetails)
	{
		Details = projectDetails;
	}

	/// <summary>
	/// This is the persistence constructor, only use from the data layer
	/// </summary>
	public Project(int id, ProjectDetails projectDetails)
	{
		Id = id;
		Details = projectDetails;
	}

	public int Id { get; }

	public ProjectDetails Details { get; private set; }

	public static CreateResult<IProject> Create(IApplication application)
	{
		if (application.ApplicationType != ApplicationType.JoinAMat)
		{
			return new CreateValidationErrorResult<IProject>(
				new List<ValidationError>
				{
					new ValidationError("ApplicationStatus", "Only projects of type JoinAMat are supported")
				});
		}

		var school = application.Schools.Single().Details;

		var projectDetails = new ProjectDetails(
			school.Urn
		)
		{
			SchoolName = school.SchoolName,
			ApplicationReferenceNumber = application.ApplicationId.ToString(),
			ProjectStatus = "Converter Pre-AO (C)",
			ApplicationReceivedDate = application.ApplicationSubmittedDate,
			OpeningDate = DateTime.Today.AddMonths(6),
			TrustReferenceNumber = application.JoinTrust?.Id.ToString(),
			NameOfTrust = application.JoinTrust?.TrustName,
			AcademyTypeAndRoute = "Converter",
			ProposedAcademyOpeningDate = school.ConversionTargetDate,
			ConversionSupportGrantAmount = 25000,
			PublishedAdmissionNumber = school.CapacityPublishedAdmissionsNumber.ToString(),
			PartOfPfiScheme = ToYesNoString(school.LandAndBuildings?.PartOfPfiScheme),
			FinancialDeficit = ToYesNoString(IsDeficit(school.CurrentFinancialYear?.CapitalCarryForwardStatus)),
			RationaleForTrust = school.SchoolConversionReasonsForJoining,
			EndOfCurrentFinancialYear = school.CurrentFinancialYear?.FinancialYearEndDate,
			EndOfNextFinancialYear = school.NextFinancialYear?.FinancialYearEndDate,
			RevenueCarryForwardAtEndMarchCurrentYear = ConvertDeficitAmountToNegative(school.CurrentFinancialYear?.Revenue, IsDeficit(school.CurrentFinancialYear?.RevenueStatus)),
			ProjectedRevenueBalanceAtEndMarchNextYear = ConvertDeficitAmountToNegative(school.NextFinancialYear?.Revenue, IsDeficit(school.NextFinancialYear?.RevenueStatus)),
			CapitalCarryForwardAtEndMarchCurrentYear = ConvertDeficitAmountToNegative(school.CurrentFinancialYear?.CapitalCarryForward, IsDeficit(school.CurrentFinancialYear?.CapitalCarryForwardStatus)),
			CapitalCarryForwardAtEndMarchNextYear = ConvertDeficitAmountToNegative(school.NextFinancialYear?.CapitalCarryForward, IsDeficit(school.NextFinancialYear?.CapitalCarryForwardStatus)),
			YearOneProjectedPupilNumbers = school.ProjectedPupilNumbersYear1,
			YearTwoProjectedPupilNumbers = school.ProjectedPupilNumbersYear2,
			YearThreeProjectedPupilNumbers = school.ProjectedPupilNumbersYear3
		};

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
	}

	public CommandResult Update(ProjectDetails detailsToUpdate)
	{
		if (Details.Urn != detailsToUpdate.Urn)
		{
			return new CommandValidationErrorResult(new List<ValidationError>
			{
				new ValidationError("Urn", "Urn in update model must match existing record")
			});
		}

		Details = new ProjectDetails(detailsToUpdate.Urn)
		{
			Urn = detailsToUpdate.Urn,
			IfdPipelineId = detailsToUpdate.IfdPipelineId,
			SchoolName = detailsToUpdate.SchoolName,
			ApplicationReferenceNumber = detailsToUpdate.ApplicationReferenceNumber,
			ProjectStatus = detailsToUpdate.ProjectStatus,
			ApplicationReceivedDate = detailsToUpdate.ApplicationReceivedDate,
			AssignedDate = detailsToUpdate.AssignedDate,
			HeadTeacherBoardDate = detailsToUpdate.HeadTeacherBoardDate,
			OpeningDate = detailsToUpdate.OpeningDate,
			BaselineDate = detailsToUpdate.BaselineDate,

			// la summary page
			LocalAuthorityInformationTemplateSentDate = detailsToUpdate.LocalAuthorityInformationTemplateSentDate,
			LocalAuthorityInformationTemplateReturnedDate = detailsToUpdate.LocalAuthorityInformationTemplateReturnedDate,
			LocalAuthorityInformationTemplateComments = detailsToUpdate.LocalAuthorityInformationTemplateComments,
			LocalAuthorityInformationTemplateLink = detailsToUpdate.LocalAuthorityInformationTemplateLink,
			LocalAuthorityInformationTemplateSectionComplete = detailsToUpdate.LocalAuthorityInformationTemplateSectionComplete,

			// school/trust info
			RecommendationForProject = detailsToUpdate.RecommendationForProject,
			Author = detailsToUpdate.Author,
			Version = detailsToUpdate.Version,
			ClearedBy = detailsToUpdate.ClearedBy,
			AcademyOrderRequired = detailsToUpdate.AcademyOrderRequired,
			PreviousHeadTeacherBoardDateQuestion = detailsToUpdate.PreviousHeadTeacherBoardDateQuestion,
			PreviousHeadTeacherBoardDate = detailsToUpdate.PreviousHeadTeacherBoardDate,
			PreviousHeadTeacherBoardLink = detailsToUpdate.PreviousHeadTeacherBoardLink,
			TrustReferenceNumber = detailsToUpdate.TrustReferenceNumber,
			NameOfTrust = detailsToUpdate.NameOfTrust,
			SponsorReferenceNumber = detailsToUpdate.SponsorReferenceNumber,
			SponsorName = detailsToUpdate.SponsorName,
			AcademyTypeAndRoute = detailsToUpdate.AcademyTypeAndRoute,
			ProposedAcademyOpeningDate = detailsToUpdate.ProposedAcademyOpeningDate,
			SchoolAndTrustInformationSectionComplete = detailsToUpdate.SchoolAndTrustInformationSectionComplete,
			ConversionSupportGrantAmount = detailsToUpdate.ConversionSupportGrantAmount,
			ConversionSupportGrantChangeReason = detailsToUpdate.ConversionSupportGrantChangeReason,

			// general info
			SchoolPhase = detailsToUpdate.SchoolPhase,
			AgeRange = detailsToUpdate.AgeRange,
			SchoolType = detailsToUpdate.SchoolType,
			ActualPupilNumbers = detailsToUpdate.ActualPupilNumbers,
			Capacity = detailsToUpdate.Capacity,
			PublishedAdmissionNumber = detailsToUpdate.PublishedAdmissionNumber,
			PercentageFreeSchoolMeals = detailsToUpdate.PercentageFreeSchoolMeals,
			PartOfPfiScheme = detailsToUpdate.PartOfPfiScheme,
			ViabilityIssues = detailsToUpdate.ViabilityIssues,
			FinancialDeficit = detailsToUpdate.FinancialDeficit,
			DiocesanTrust = detailsToUpdate.DiocesanTrust,
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = detailsToUpdate.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust,
			DistanceFromSchoolToTrustHeadquarters = detailsToUpdate.DistanceFromSchoolToTrustHeadquarters,
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = detailsToUpdate.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
			MemberOfParliamentParty = detailsToUpdate.MemberOfParliamentParty,
			MemberOfParliamentName = detailsToUpdate.MemberOfParliamentName,

			GeneralInformationSectionComplete = detailsToUpdate.GeneralInformationSectionComplete,

			// school performance ofsted information
			SchoolPerformanceAdditionalInformation = detailsToUpdate.SchoolPerformanceAdditionalInformation,

			// rationale
			RationaleForProject = detailsToUpdate.RationaleForProject,
			RationaleForTrust = detailsToUpdate.RationaleForTrust,
			RationaleSectionComplete = detailsToUpdate.RationaleSectionComplete,

			// risk and issues
			RisksAndIssues = detailsToUpdate.RisksAndIssues,
			EqualitiesImpactAssessmentConsidered = detailsToUpdate.EqualitiesImpactAssessmentConsidered,
			RisksAndIssuesSectionComplete = detailsToUpdate.RisksAndIssuesSectionComplete,

			// legal requirements
			GoverningBodyResolution = detailsToUpdate.GoverningBodyResolution,
			Consultation = detailsToUpdate.Consultation,
			DiocesanConsent = detailsToUpdate.DiocesanConsent,
			FoundationConsent = detailsToUpdate.FoundationConsent,
			LegalRequirementsSectionComplete = detailsToUpdate.LegalRequirementsSectionComplete,

			// school budget info
			EndOfCurrentFinancialYear = detailsToUpdate.EndOfCurrentFinancialYear,
			EndOfNextFinancialYear = detailsToUpdate.EndOfNextFinancialYear,
			RevenueCarryForwardAtEndMarchCurrentYear = detailsToUpdate.RevenueCarryForwardAtEndMarchCurrentYear,
			ProjectedRevenueBalanceAtEndMarchNextYear = detailsToUpdate.ProjectedRevenueBalanceAtEndMarchNextYear,
			CapitalCarryForwardAtEndMarchCurrentYear = detailsToUpdate.CapitalCarryForwardAtEndMarchCurrentYear,
			CapitalCarryForwardAtEndMarchNextYear = detailsToUpdate.CapitalCarryForwardAtEndMarchNextYear,
			SchoolBudgetInformationAdditionalInformation = detailsToUpdate.SchoolBudgetInformationAdditionalInformation,
			SchoolBudgetInformationSectionComplete = detailsToUpdate.SchoolBudgetInformationSectionComplete,

			// pupil schools forecast
			YearOneProjectedCapacity = detailsToUpdate.YearOneProjectedCapacity,
			YearOneProjectedPupilNumbers = detailsToUpdate.YearOneProjectedPupilNumbers,
			YearTwoProjectedCapacity = detailsToUpdate.YearTwoProjectedCapacity,
			YearTwoProjectedPupilNumbers = detailsToUpdate.YearTwoProjectedPupilNumbers,
			YearThreeProjectedCapacity = detailsToUpdate.YearThreeProjectedCapacity,
			YearThreeProjectedPupilNumbers = detailsToUpdate.YearThreeProjectedPupilNumbers,
			SchoolPupilForecastsAdditionalInformation = detailsToUpdate.SchoolPupilForecastsAdditionalInformation,

			// key stage performance tables
			KeyStage2PerformanceAdditionalInformation = detailsToUpdate.KeyStage2PerformanceAdditionalInformation,
			KeyStage4PerformanceAdditionalInformation = detailsToUpdate.KeyStage4PerformanceAdditionalInformation,
			KeyStage5PerformanceAdditionalInformation = detailsToUpdate.KeyStage5PerformanceAdditionalInformation,

			// assigned users
			AssignedUser = MapUser(detailsToUpdate.AssignedUser)
		};

		return new CommandSuccessResult();
	}

	private static bool IsDeficit(RevenueType? revenueType)
	{
		return revenueType.HasValue && revenueType == RevenueType.Deficit;
	}

	private static decimal? ConvertDeficitAmountToNegative(decimal? amount, bool? isDeficit)
	{
		if (isDeficit.HasValue)
		{
			return isDeficit.Value ? amount * -1.0M : amount;
		}
		return null;
	}

	private static string? ToYesNoString(bool? value)
	{
		if (!value.HasValue) return null;
		return value == true ? "Yes" : "No";
	}

	private static User? MapUser(User? user)
	{
		if (user == null) return null;
		return new User(user.Id, user.FullName, user.EmailAddress);
	}
}
