using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
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
		var result = sut.UpdatePatch(updatedProject);

		// Assert
		Assert.IsType<CommandSuccessResult>(result);
		AssertProjectModelIsEqual(updatedProject, sut);
	}

	[Fact]
	public void Update_WithEmptyProject__ReturnsUpdateSuccessResult_AndDoesNotUpdateProjectDetails()
	{
		// Arrange
		var existingProject = _fixture.Create<ProjectDetails>();
		var sut = new Project(1, existingProject);
		var updatedProject = new ProjectDetails(1, 1);

		// Act
		var result = sut.UpdatePatch(updatedProject);

		// Assert
		Assert.IsType<CommandSuccessResult>(result);
		AssertProjectModelIsEqual(existingProject, sut);
	}

	[Fact]
	public void Update_WithDifferentUrn__ReturnsCommandValidationErrorResult_AndDoesNotUpdateProjectDetails()
	{
		// Arrange
		var existingProject = _fixture.Create<ProjectDetails>();
		var sut = new Project(1, existingProject);
		var updatedProject = new ProjectDetails(1, 1);

		// Act
		var result = sut.UpdatePatch(updatedProject);

		// Assert
		var validationErrors = Assert.IsType<CommandValidationErrorResult>(result).ValidationErrors;
		Assert.Equal("Urn", validationErrors.First().PropertyName);
		Assert.Equal("Urn in update model must match existing record", validationErrors.First().ErrorMessage);
		AssertProjectModelIsEqual(existingProject, sut);
	}

	private static void AssertProjectModelIsEqual(ProjectDetails expectedProject, Project sut)
	{
		Assert.Multiple(
					() => Assert.Equal(expectedProject.HeadTeacherBoardDate, sut.Details.HeadTeacherBoardDate),
					() => Assert.Equal(expectedProject.Author, sut.Details.Author),
					() => Assert.Equal(expectedProject.ClearedBy, sut.Details.ClearedBy),
					() => Assert.Equal(expectedProject.ProposedAcademyOpeningDate, sut.Details.ProposedAcademyOpeningDate),
					() => Assert.Equal(expectedProject.PublishedAdmissionNumber, sut.Details.PublishedAdmissionNumber),
					() => Assert.Equal(expectedProject.ViabilityIssues, sut.Details.ViabilityIssues),
					() => Assert.Equal(expectedProject.FinancialDeficit, sut.Details.FinancialDeficit),
					() => Assert.Equal(expectedProject.RationaleForProject, sut.Details.RationaleForProject),
					() => Assert.Equal(expectedProject.RisksAndIssues, sut.Details.RisksAndIssues),
					() => Assert.Equal(expectedProject.RevenueCarryForwardAtEndMarchCurrentYear, sut.Details.RevenueCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(expectedProject.ProjectedRevenueBalanceAtEndMarchNextYear, sut.Details.ProjectedRevenueBalanceAtEndMarchNextYear),
					() => Assert.Equal(expectedProject.RationaleSectionComplete, sut.Details.RationaleSectionComplete),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateSentDate, sut.Details.LocalAuthorityInformationTemplateSentDate),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateReturnedDate, sut.Details.LocalAuthorityInformationTemplateReturnedDate),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateComments, sut.Details.LocalAuthorityInformationTemplateComments),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateLink, sut.Details.LocalAuthorityInformationTemplateLink),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateSectionComplete, sut.Details.LocalAuthorityInformationTemplateSectionComplete),
					() => Assert.Equal(expectedProject.RecommendationForProject, sut.Details.RecommendationForProject),
					() => Assert.Equal(expectedProject.AcademyOrderRequired, sut.Details.AcademyOrderRequired),
					() => Assert.Equal(expectedProject.SchoolAndTrustInformationSectionComplete, sut.Details.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(expectedProject.DistanceFromSchoolToTrustHeadquarters, sut.Details.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(expectedProject.DistanceFromSchoolToTrustHeadquarters, sut.Details.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(expectedProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, sut.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
					() => Assert.Equal(expectedProject.MemberOfParliamentName, sut.Details.MemberOfParliamentName),
					() => Assert.Equal(expectedProject.MemberOfParliamentParty, sut.Details.MemberOfParliamentParty),
					() => Assert.Equal(expectedProject.GeneralInformationSectionComplete, sut.Details.GeneralInformationSectionComplete),
					() => Assert.Equal(expectedProject.RisksAndIssuesSectionComplete, sut.Details.RisksAndIssuesSectionComplete),
					() => Assert.Equal(expectedProject.SchoolPerformanceAdditionalInformation, sut.Details.SchoolPerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.CapitalCarryForwardAtEndMarchCurrentYear, sut.Details.CapitalCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(expectedProject.CapitalCarryForwardAtEndMarchNextYear, sut.Details.CapitalCarryForwardAtEndMarchNextYear),
					() => Assert.Equal(expectedProject.SchoolBudgetInformationAdditionalInformation, sut.Details.SchoolBudgetInformationAdditionalInformation),
					() => Assert.Equal(expectedProject.SchoolAndTrustInformationSectionComplete, sut.Details.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(expectedProject.SchoolPupilForecastsAdditionalInformation, sut.Details.SchoolPupilForecastsAdditionalInformation),
					() => Assert.Equal(expectedProject.YearOneProjectedCapacity, sut.Details.YearOneProjectedCapacity),
					() => Assert.Equal(expectedProject.YearOneProjectedPupilNumbers, sut.Details.YearOneProjectedPupilNumbers),
					() => Assert.Equal(expectedProject.YearTwoProjectedCapacity, sut.Details.YearTwoProjectedCapacity),
					() => Assert.Equal(expectedProject.YearTwoProjectedPupilNumbers, sut.Details.YearTwoProjectedPupilNumbers),
					() => Assert.Equal(expectedProject.YearThreeProjectedCapacity, sut.Details.YearThreeProjectedCapacity),
					() => Assert.Equal(expectedProject.YearThreeProjectedPupilNumbers, sut.Details.YearThreeProjectedPupilNumbers),
					() => Assert.Equal(expectedProject.KeyStage2PerformanceAdditionalInformation, sut.Details.KeyStage2PerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.KeyStage4PerformanceAdditionalInformation, sut.Details.KeyStage4PerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.KeyStage5PerformanceAdditionalInformation, sut.Details.KeyStage5PerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.PreviousHeadTeacherBoardDateQuestion, sut.Details.PreviousHeadTeacherBoardDateQuestion),
					() => Assert.Equal(expectedProject.ConversionSupportGrantAmount, sut.Details.ConversionSupportGrantAmount),
					() => Assert.Equal(expectedProject.ConversionSupportGrantChangeReason, sut.Details.ConversionSupportGrantChangeReason),
					() => Assert.Equal(expectedProject.ProjectStatus, sut.Details.ProjectStatus)
				);
	}
}
