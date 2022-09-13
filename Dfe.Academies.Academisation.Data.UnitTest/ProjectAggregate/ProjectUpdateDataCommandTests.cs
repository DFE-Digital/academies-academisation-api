using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate;

public class ProjectUpdateDataCommandTests
{
	private readonly Fixture _fixture = new();

	private readonly ProjectUpdateDataCommand _subject;
	private readonly AcademisationContext _context;

	public ProjectUpdateDataCommandTests()
	{
		_context = new TestProjectContext().CreateContext();
		_subject = new ProjectUpdateDataCommand(_context);
	}

	[Fact]
	public async Task ProjectExists___ProjectUpdated()
	{
		// arrange
		ProjectDetails projectDetails = _fixture.Create<ProjectDetails>();		

		IProject project = new Project(1, projectDetails);
		await _context.Projects.LoadAsync();

		// act
		await _subject.Execute(project);

		_context.ChangeTracker.Clear();

		var updatedProject = await _context.Projects.SingleAsync(p => p.Id == project.Id);

		// assert
		AssertProjectModelIsEqual(project.Details, updatedProject);
	}

	private static void AssertProjectModelIsEqual(ProjectDetails expectedProject, ProjectState actualProject)
	{
		Assert.Multiple(
					() => Assert.Equal(expectedProject.HeadTeacherBoardDate, actualProject.HeadTeacherBoardDate),
					() => Assert.Equal(expectedProject.Author, actualProject.Author),
					() => Assert.Equal(expectedProject.ClearedBy, actualProject.ClearedBy),
					() => Assert.Equal(expectedProject.ProposedAcademyOpeningDate, actualProject.ProposedAcademyOpeningDate),
					() => Assert.Equal(expectedProject.PublishedAdmissionNumber, actualProject.PublishedAdmissionNumber),
					() => Assert.Equal(expectedProject.ViabilityIssues, actualProject.ViabilityIssues),
					() => Assert.Equal(expectedProject.FinancialDeficit, actualProject.FinancialDeficit),
					() => Assert.Equal(expectedProject.RationaleForProject, actualProject.RationaleForProject),
					() => Assert.Equal(expectedProject.RisksAndIssues, actualProject.RisksAndIssues),
					() => Assert.Equal(expectedProject.RevenueCarryForwardAtEndMarchCurrentYear, actualProject.RevenueCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(expectedProject.ProjectedRevenueBalanceAtEndMarchNextYear, actualProject.ProjectedRevenueBalanceAtEndMarchNextYear),
					() => Assert.Equal(expectedProject.RationaleSectionComplete, actualProject.RationaleSectionComplete),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateSentDate, actualProject.LocalAuthorityInformationTemplateSentDate),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateReturnedDate, actualProject.LocalAuthorityInformationTemplateReturnedDate),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateComments, actualProject.LocalAuthorityInformationTemplateComments),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateLink, actualProject.LocalAuthorityInformationTemplateLink),
					() => Assert.Equal(expectedProject.LocalAuthorityInformationTemplateSectionComplete, actualProject.LocalAuthorityInformationTemplateSectionComplete),
					() => Assert.Equal(expectedProject.RecommendationForProject, actualProject.RecommendationForProject),
					() => Assert.Equal(expectedProject.AcademyOrderRequired, actualProject.AcademyOrderRequired),
					() => Assert.Equal(expectedProject.SchoolAndTrustInformationSectionComplete, actualProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(expectedProject.DistanceFromSchoolToTrustHeadquarters, actualProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(expectedProject.DistanceFromSchoolToTrustHeadquarters, actualProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(expectedProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, actualProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
					() => Assert.Equal(expectedProject.MemberOfParliamentName, actualProject.MemberOfParliamentName),
					() => Assert.Equal(expectedProject.MemberOfParliamentParty, actualProject.MemberOfParliamentParty),
					() => Assert.Equal(expectedProject.GeneralInformationSectionComplete, actualProject.GeneralInformationSectionComplete),
					() => Assert.Equal(expectedProject.RisksAndIssuesSectionComplete, actualProject.RisksAndIssuesSectionComplete),
					() => Assert.Equal(expectedProject.SchoolPerformanceAdditionalInformation, actualProject.SchoolPerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.CapitalCarryForwardAtEndMarchCurrentYear, actualProject.CapitalCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(expectedProject.CapitalCarryForwardAtEndMarchNextYear, actualProject.CapitalCarryForwardAtEndMarchNextYear),
					() => Assert.Equal(expectedProject.SchoolBudgetInformationAdditionalInformation, actualProject.SchoolBudgetInformationAdditionalInformation),
					() => Assert.Equal(expectedProject.SchoolAndTrustInformationSectionComplete, actualProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(expectedProject.SchoolPupilForecastsAdditionalInformation, actualProject.SchoolPupilForecastsAdditionalInformation),
					() => Assert.Equal(expectedProject.YearOneProjectedCapacity, actualProject.YearOneProjectedCapacity),
					() => Assert.Equal(expectedProject.YearOneProjectedPupilNumbers, actualProject.YearOneProjectedPupilNumbers),
					() => Assert.Equal(expectedProject.YearTwoProjectedCapacity, actualProject.YearTwoProjectedCapacity),
					() => Assert.Equal(expectedProject.YearTwoProjectedPupilNumbers, actualProject.YearTwoProjectedPupilNumbers),
					() => Assert.Equal(expectedProject.YearThreeProjectedCapacity, actualProject.YearThreeProjectedCapacity),
					() => Assert.Equal(expectedProject.YearThreeProjectedPupilNumbers, actualProject.YearThreeProjectedPupilNumbers),
					() => Assert.Equal(expectedProject.KeyStage2PerformanceAdditionalInformation, actualProject.KeyStage2PerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.KeyStage4PerformanceAdditionalInformation, actualProject.KeyStage4PerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.KeyStage5PerformanceAdditionalInformation, actualProject.KeyStage5PerformanceAdditionalInformation),
					() => Assert.Equal(expectedProject.PreviousHeadTeacherBoardDateQuestion, actualProject.PreviousHeadTeacherBoardDateQuestion),
					() => Assert.Equal(expectedProject.ConversionSupportGrantAmount, actualProject.ConversionSupportGrantAmount),
					() => Assert.Equal(expectedProject.ConversionSupportGrantChangeReason, actualProject.ConversionSupportGrantChangeReason),
					() => Assert.Equal(expectedProject.ProjectStatus, actualProject.ProjectStatus)
				);
	}
}
