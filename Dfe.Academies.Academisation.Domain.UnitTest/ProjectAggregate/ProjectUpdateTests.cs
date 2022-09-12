using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ProjectAggregate;

public class ProjectUpdateTests
{
	private readonly Fixture _fixture = new Fixture();

	[Fact]
	public void Update___ReturnsUpdateSuccessResult_AndSetsProjectDetails()
	{
		// Arrange
		var initialProject = _fixture.Create<ProjectDetails>();
		var sut = new Project(1, initialProject);
		var updatedProject = _fixture.Create<ProjectDetails>();

		// Act
		sut.UpdatePatch(updatedProject);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(updatedProject.HeadTeacherBoardDate, sut.Details.HeadTeacherBoardDate),
			() => Assert.Equal(updatedProject.Author, sut.Details.Author),
			() => Assert.Equal(updatedProject.ClearedBy, sut.Details.ClearedBy),
			() => Assert.Equal(updatedProject.ProposedAcademyOpeningDate, sut.Details.ProposedAcademyOpeningDate),
			() => Assert.Equal(updatedProject.PublishedAdmissionNumber, sut.Details.PublishedAdmissionNumber),
			() => Assert.Equal(updatedProject.ViabilityIssues, sut.Details.ViabilityIssues),
			() => Assert.Equal(updatedProject.FinancialDeficit, sut.Details.FinancialDeficit),
			() => Assert.Equal(updatedProject.RationaleForProject, sut.Details.RationaleForProject),
			() => Assert.Equal(updatedProject.RisksAndIssues, sut.Details.RisksAndIssues),
			() => Assert.Equal(updatedProject.RevenueCarryForwardAtEndMarchCurrentYear, sut.Details.RevenueCarryForwardAtEndMarchCurrentYear),
			() => Assert.Equal(updatedProject.ProjectedRevenueBalanceAtEndMarchNextYear, sut.Details.ProjectedRevenueBalanceAtEndMarchNextYear),
			() => Assert.Equal(updatedProject.RationaleSectionComplete, sut.Details.RationaleSectionComplete),
			() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateSentDate, sut.Details.LocalAuthorityInformationTemplateSentDate),
			() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateReturnedDate, sut.Details.LocalAuthorityInformationTemplateReturnedDate),
			() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateComments, sut.Details.LocalAuthorityInformationTemplateComments),
			() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateLink, sut.Details.LocalAuthorityInformationTemplateLink),
			() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateSectionComplete, sut.Details.LocalAuthorityInformationTemplateSectionComplete),
			() => Assert.Equal(updatedProject.RecommendationForProject, sut.Details.RecommendationForProject),
			() => Assert.Equal(updatedProject.AcademyOrderRequired, sut.Details.AcademyOrderRequired),
			() => Assert.Equal(updatedProject.SchoolAndTrustInformationSectionComplete, sut.Details.SchoolAndTrustInformationSectionComplete),
			() => Assert.Equal(updatedProject.DistanceFromSchoolToTrustHeadquarters, sut.Details.DistanceFromSchoolToTrustHeadquarters),
			() => Assert.Equal(updatedProject.DistanceFromSchoolToTrustHeadquarters, sut.Details.DistanceFromSchoolToTrustHeadquarters),
			() => Assert.Equal(updatedProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, sut.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
			() => Assert.Equal(updatedProject.MemberOfParliamentName, sut.Details.MemberOfParliamentName),
			() => Assert.Equal(updatedProject.MemberOfParliamentParty, sut.Details.MemberOfParliamentParty),
			() => Assert.Equal(updatedProject.GeneralInformationSectionComplete, sut.Details.GeneralInformationSectionComplete),
			() => Assert.Equal(updatedProject.RisksAndIssuesSectionComplete, sut.Details.RisksAndIssuesSectionComplete),
			() => Assert.Equal(updatedProject.SchoolPerformanceAdditionalInformation, sut.Details.SchoolPerformanceAdditionalInformation),
			() => Assert.Equal(updatedProject.CapitalCarryForwardAtEndMarchCurrentYear, sut.Details.CapitalCarryForwardAtEndMarchCurrentYear),
			() => Assert.Equal(updatedProject.CapitalCarryForwardAtEndMarchNextYear, sut.Details.CapitalCarryForwardAtEndMarchNextYear),
			() => Assert.Equal(updatedProject.SchoolBudgetInformationAdditionalInformation, sut.Details.SchoolBudgetInformationAdditionalInformation),
			() => Assert.Equal(updatedProject.SchoolAndTrustInformationSectionComplete, sut.Details.SchoolAndTrustInformationSectionComplete),
			() => Assert.Equal(updatedProject.SchoolPupilForecastsAdditionalInformation, sut.Details.SchoolPupilForecastsAdditionalInformation),
			() => Assert.Equal(updatedProject.YearOneProjectedCapacity, sut.Details.YearOneProjectedCapacity),
			() => Assert.Equal(updatedProject.YearOneProjectedPupilNumbers, sut.Details.YearOneProjectedPupilNumbers),
			() => Assert.Equal(updatedProject.YearTwoProjectedCapacity, sut.Details.YearTwoProjectedCapacity),
			() => Assert.Equal(updatedProject.YearTwoProjectedPupilNumbers, sut.Details.YearTwoProjectedPupilNumbers),
			() => Assert.Equal(updatedProject.YearThreeProjectedCapacity, sut.Details.YearThreeProjectedCapacity),
			() => Assert.Equal(updatedProject.YearThreeProjectedPupilNumbers, sut.Details.YearThreeProjectedPupilNumbers),
			() => Assert.Equal(updatedProject.KeyStage2PerformanceAdditionalInformation, sut.Details.KeyStage2PerformanceAdditionalInformation),
			() => Assert.Equal(updatedProject.KeyStage4PerformanceAdditionalInformation, sut.Details.KeyStage4PerformanceAdditionalInformation),
			() => Assert.Equal(updatedProject.KeyStage5PerformanceAdditionalInformation, sut.Details.KeyStage5PerformanceAdditionalInformation),
			() => Assert.Equal(updatedProject.PreviousHeadTeacherBoardDateQuestion, sut.Details.PreviousHeadTeacherBoardDateQuestion),
			() => Assert.Equal(updatedProject.ConversionSupportGrantAmount, sut.Details.ConversionSupportGrantAmount),
			() => Assert.Equal(updatedProject.ConversionSupportGrantChangeReason, sut.Details.ConversionSupportGrantChangeReason),
			() => Assert.Equal(updatedProject.ProjectStatus, sut.Details.ProjectStatus)
		);
	}

	[Fact]
	public void Update_WithEmptyProject__ReturnsUpdateSuccessResult_AndSetsProjectDetails()
	{
		// Arrange
		var initialProject = _fixture.Create<ProjectDetails>();
		var sut = new Project(1, initialProject);
		var updatedProject = new ProjectDetails(1, 1);

		// Act
		sut.UpdatePatch(updatedProject);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(initialProject.HeadTeacherBoardDate, sut.Details.HeadTeacherBoardDate),
			() => Assert.Equal(initialProject.Author, sut.Details.Author),
			() => Assert.Equal(initialProject.ClearedBy, sut.Details.ClearedBy),
			() => Assert.Equal(initialProject.ProposedAcademyOpeningDate, sut.Details.ProposedAcademyOpeningDate),
			() => Assert.Equal(initialProject.PublishedAdmissionNumber, sut.Details.PublishedAdmissionNumber),
			() => Assert.Equal(initialProject.ViabilityIssues, sut.Details.ViabilityIssues),
			() => Assert.Equal(initialProject.FinancialDeficit, sut.Details.FinancialDeficit),
			() => Assert.Equal(initialProject.RationaleForProject, sut.Details.RationaleForProject),
			() => Assert.Equal(initialProject.RisksAndIssues, sut.Details.RisksAndIssues),
			() => Assert.Equal(initialProject.RevenueCarryForwardAtEndMarchCurrentYear, sut.Details.RevenueCarryForwardAtEndMarchCurrentYear),
			() => Assert.Equal(initialProject.ProjectedRevenueBalanceAtEndMarchNextYear, sut.Details.ProjectedRevenueBalanceAtEndMarchNextYear),
			() => Assert.Equal(initialProject.RationaleSectionComplete, sut.Details.RationaleSectionComplete),
			() => Assert.Equal(initialProject.LocalAuthorityInformationTemplateSentDate, sut.Details.LocalAuthorityInformationTemplateSentDate),
			() => Assert.Equal(initialProject.LocalAuthorityInformationTemplateReturnedDate, sut.Details.LocalAuthorityInformationTemplateReturnedDate),
			() => Assert.Equal(initialProject.LocalAuthorityInformationTemplateComments, sut.Details.LocalAuthorityInformationTemplateComments),
			() => Assert.Equal(initialProject.LocalAuthorityInformationTemplateLink, sut.Details.LocalAuthorityInformationTemplateLink),
			() => Assert.Equal(initialProject.LocalAuthorityInformationTemplateSectionComplete, sut.Details.LocalAuthorityInformationTemplateSectionComplete),
			() => Assert.Equal(initialProject.RecommendationForProject, sut.Details.RecommendationForProject),
			() => Assert.Equal(initialProject.AcademyOrderRequired, sut.Details.AcademyOrderRequired),
			() => Assert.Equal(initialProject.SchoolAndTrustInformationSectionComplete, sut.Details.SchoolAndTrustInformationSectionComplete),
			() => Assert.Equal(initialProject.DistanceFromSchoolToTrustHeadquarters, sut.Details.DistanceFromSchoolToTrustHeadquarters),
			() => Assert.Equal(initialProject.DistanceFromSchoolToTrustHeadquarters, sut.Details.DistanceFromSchoolToTrustHeadquarters),
			() => Assert.Equal(initialProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, sut.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
			() => Assert.Equal(initialProject.MemberOfParliamentName, sut.Details.MemberOfParliamentName),
			() => Assert.Equal(initialProject.MemberOfParliamentParty, sut.Details.MemberOfParliamentParty),
			() => Assert.Equal(initialProject.GeneralInformationSectionComplete, sut.Details.GeneralInformationSectionComplete),
			() => Assert.Equal(initialProject.RisksAndIssuesSectionComplete, sut.Details.RisksAndIssuesSectionComplete),
			() => Assert.Equal(initialProject.SchoolPerformanceAdditionalInformation, sut.Details.SchoolPerformanceAdditionalInformation),
			() => Assert.Equal(initialProject.CapitalCarryForwardAtEndMarchCurrentYear, sut.Details.CapitalCarryForwardAtEndMarchCurrentYear),
			() => Assert.Equal(initialProject.CapitalCarryForwardAtEndMarchNextYear, sut.Details.CapitalCarryForwardAtEndMarchNextYear),
			() => Assert.Equal(initialProject.SchoolBudgetInformationAdditionalInformation, sut.Details.SchoolBudgetInformationAdditionalInformation),
			() => Assert.Equal(initialProject.SchoolAndTrustInformationSectionComplete, sut.Details.SchoolAndTrustInformationSectionComplete),
			() => Assert.Equal(initialProject.SchoolPupilForecastsAdditionalInformation, sut.Details.SchoolPupilForecastsAdditionalInformation),
			() => Assert.Equal(initialProject.YearOneProjectedCapacity, sut.Details.YearOneProjectedCapacity),
			() => Assert.Equal(initialProject.YearOneProjectedPupilNumbers, sut.Details.YearOneProjectedPupilNumbers),
			() => Assert.Equal(initialProject.YearTwoProjectedCapacity, sut.Details.YearTwoProjectedCapacity),
			() => Assert.Equal(initialProject.YearTwoProjectedPupilNumbers, sut.Details.YearTwoProjectedPupilNumbers),
			() => Assert.Equal(initialProject.YearThreeProjectedCapacity, sut.Details.YearThreeProjectedCapacity),
			() => Assert.Equal(initialProject.YearThreeProjectedPupilNumbers, sut.Details.YearThreeProjectedPupilNumbers),
			() => Assert.Equal(initialProject.KeyStage2PerformanceAdditionalInformation, sut.Details.KeyStage2PerformanceAdditionalInformation),
			() => Assert.Equal(initialProject.KeyStage4PerformanceAdditionalInformation, sut.Details.KeyStage4PerformanceAdditionalInformation),
			() => Assert.Equal(initialProject.KeyStage5PerformanceAdditionalInformation, sut.Details.KeyStage5PerformanceAdditionalInformation),
			() => Assert.Equal(initialProject.PreviousHeadTeacherBoardDateQuestion, sut.Details.PreviousHeadTeacherBoardDateQuestion),
			() => Assert.Equal(initialProject.ConversionSupportGrantAmount, sut.Details.ConversionSupportGrantAmount),
			() => Assert.Equal(initialProject.ConversionSupportGrantChangeReason, sut.Details.ConversionSupportGrantChangeReason),
			() => Assert.Equal(initialProject.ProjectStatus, sut.Details.ProjectStatus)
		);
	}
}
