using AutoFixture;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class ProjectUpdateTests
{
	private readonly Fixture _fixture = new();
	private readonly AcademisationContext _context;

	// data

	// service
	private readonly ILegacyProjectGetQuery _legacyProjectGetQuery;
	private readonly ILegacyProjectUpdateCommand _legacyProjectUpdateCommand;


	public ProjectUpdateTests()
	{
		_context = new TestProjectContext().CreateContext();

		// data
		IProjectGetDataQuery projectGetDataQuery = new ProjectGetDataQuery(_context);
		IProjectUpdateDataCommand projectUpdateDataCommand = new ProjectUpdateDataCommand(_context);

		// service
		_legacyProjectGetQuery = new LegacyProjectGetQuery(projectGetDataQuery);
		_legacyProjectUpdateCommand = new LegacyProjectUpdateCommand(projectGetDataQuery, projectUpdateDataCommand);
	}


	[Fact]
	public async Task ProjectExists___FullProjectIsUpdated()
	{
		// Arrange
		var legacyProjectController = new LegacyProjectController(_legacyProjectGetQuery, Mock.Of<ILegacyProjectListGetQuery>(),
			Mock.Of<IProjectGetStatusesQuery>(), _legacyProjectUpdateCommand, Mock.Of<ILegacyProjectAddNoteCommand>());
		var existingProject = _fixture.Create<ProjectState>();
		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = _fixture.Build<LegacyProjectServiceModel>()
			.With(p => p.Id, existingProject.Id)
			.With(p => p.Urn, existingProject.Urn)
			.Create();

		updatedProject.Notes?.Clear();

		// Act
		var updateResult = await legacyProjectController.Patch(updatedProject.Id, updatedProject);

		// Assert
		(_, LegacyProjectServiceModel project) = DfeAssert.OkObjectResult(updateResult);

		Assert.Equivalent(updatedProject, project);
	}


	[Fact]
	public async Task ProjectExists_FullProjectIsReturnedOnGet()
	{
		// Arrange
		var legacyProjectController = new LegacyProjectController(_legacyProjectGetQuery, Mock.Of<ILegacyProjectListGetQuery>(),
			Mock.Of<IProjectGetStatusesQuery>(), _legacyProjectUpdateCommand, Mock.Of<ILegacyProjectAddNoteCommand>());
		var existingProject = _fixture.Create<ProjectState>();
		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = _fixture.Build<LegacyProjectServiceModel>()
			.With(p => p.Id, existingProject.Id)
			.With(p => p.Urn, existingProject.Urn)
			.Create();

		updatedProject.Notes?.Clear();

		// Act
		var updateResult = await legacyProjectController.Patch(updatedProject.Id, updatedProject);
		DfeAssert.OkObjectResult(updateResult);

		var getResult = await legacyProjectController.Get(updatedProject.Id);

		// Assert
		(_, LegacyProjectServiceModel project) = DfeAssert.OkObjectResult(getResult);

		Assert.Equivalent(updatedProject, project);
	}

	[Fact]
	public async Task ProjectExists___PartialProjectIsUpdated()
	{
		// Arrange
		var legacyProjectController = new LegacyProjectController(_legacyProjectGetQuery, Mock.Of<ILegacyProjectListGetQuery>(),
			Mock.Of<IProjectGetStatusesQuery>(), _legacyProjectUpdateCommand, Mock.Of<ILegacyProjectAddNoteCommand>());
		var existingProject = _fixture.Create<ProjectState>();

		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = new LegacyProjectServiceModel
		{
			Id = existingProject.Id, ProjectStatus = "TestStatus", EqualitiesImpactAssessmentConsidered = "Yes sir"
		};

		// Act
		var updateResult = await legacyProjectController.Patch(updatedProject.Id, updatedProject);

		// Assert
		DfeAssert.OkObjectResult(updateResult);

		var getResult = await legacyProjectController.Get(updatedProject.Id);

		(_, var getProject) = DfeAssert.OkObjectResult(getResult);

		existingProject.ProjectStatus = updatedProject.ProjectStatus;
		existingProject.EqualitiesImpactAssessmentConsidered = updatedProject.EqualitiesImpactAssessmentConsidered;

		Assert.Multiple(
					() => Assert.Equal(existingProject.HeadTeacherBoardDate, getProject.HeadTeacherBoardDate),
					() => Assert.Equal(existingProject.Author, getProject.Author),
					() => Assert.Equal(existingProject.ClearedBy, getProject.ClearedBy),
					() => Assert.Equal(existingProject.ProposedAcademyOpeningDate, getProject.ProposedAcademyOpeningDate),
					() => Assert.Equal(existingProject.PublishedAdmissionNumber, getProject.PublishedAdmissionNumber),
					() => Assert.Equal(existingProject.ViabilityIssues, getProject.ViabilityIssues),
					() => Assert.Equal(existingProject.FinancialDeficit, getProject.FinancialDeficit),
					() => Assert.Equal(existingProject.RationaleForProject, getProject.RationaleForProject),
					() => Assert.Equal(existingProject.RisksAndIssues, getProject.RisksAndIssues),
					() => Assert.Equal(existingProject.EndOfCurrentFinancialYear, getProject.EndOfCurrentFinancialYear),
					() => Assert.Equal(existingProject.EndOfNextFinancialYear, getProject.EndOfNextFinancialYear),
					() => Assert.Equal(existingProject.RevenueCarryForwardAtEndMarchCurrentYear, getProject.RevenueCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(existingProject.ProjectedRevenueBalanceAtEndMarchNextYear, getProject.ProjectedRevenueBalanceAtEndMarchNextYear),
					() => Assert.Equal(existingProject.RationaleSectionComplete, getProject.RationaleSectionComplete),
					() => Assert.Equal(existingProject.LocalAuthorityInformationTemplateSentDate, getProject.LocalAuthorityInformationTemplateSentDate),
					() => Assert.Equal(existingProject.LocalAuthorityInformationTemplateReturnedDate, getProject.LocalAuthorityInformationTemplateReturnedDate),
					() => Assert.Equal(existingProject.LocalAuthorityInformationTemplateComments, getProject.LocalAuthorityInformationTemplateComments),
					() => Assert.Equal(existingProject.LocalAuthorityInformationTemplateLink, getProject.LocalAuthorityInformationTemplateLink),
					() => Assert.Equal(existingProject.LocalAuthorityInformationTemplateSectionComplete, getProject.LocalAuthorityInformationTemplateSectionComplete),
					() => Assert.Equal(existingProject.RecommendationForProject, getProject.RecommendationForProject),
					() => Assert.Equal(existingProject.AcademyOrderRequired, getProject.AcademyOrderRequired),
					() => Assert.Equal(existingProject.SchoolAndTrustInformationSectionComplete, getProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(existingProject.DistanceFromSchoolToTrustHeadquarters, getProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(existingProject.DistanceFromSchoolToTrustHeadquarters, getProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(existingProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, getProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
					() => Assert.Equal(existingProject.MemberOfParliamentName, getProject.MemberOfParliamentName),
					() => Assert.Equal(existingProject.MemberOfParliamentParty, getProject.MemberOfParliamentParty),
					() => Assert.Equal(existingProject.GeneralInformationSectionComplete, getProject.GeneralInformationSectionComplete),
					() => Assert.Equal(existingProject.RisksAndIssuesSectionComplete, getProject.RisksAndIssuesSectionComplete),
					() => Assert.Equal(existingProject.Consultation, getProject.Consultation),
					() => Assert.Equal(existingProject.DiocesanTrust, getProject.DiocesanTrust),
					() => Assert.Equal(existingProject.FoundationConsent, getProject.FoundationConsent),
					() => Assert.Equal(existingProject.GoverningBodyResolution, getProject.GoverningBodyResolution),
					() => Assert.Equal(existingProject.LegalRequirementsSectionComplete, getProject.LegalRequirementsSectionComplete),
					() => Assert.Equal(existingProject.SchoolPerformanceAdditionalInformation, getProject.SchoolPerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.CapitalCarryForwardAtEndMarchCurrentYear, getProject.CapitalCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(existingProject.CapitalCarryForwardAtEndMarchNextYear, getProject.CapitalCarryForwardAtEndMarchNextYear),
					() => Assert.Equal(existingProject.SchoolBudgetInformationAdditionalInformation, getProject.SchoolBudgetInformationAdditionalInformation),
					() => Assert.Equal(existingProject.SchoolAndTrustInformationSectionComplete, getProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(existingProject.SchoolPupilForecastsAdditionalInformation, getProject.SchoolPupilForecastsAdditionalInformation),
					() => Assert.Equal(existingProject.YearOneProjectedCapacity, getProject.YearOneProjectedCapacity),
					() => Assert.Equal(existingProject.YearOneProjectedPupilNumbers, getProject.YearOneProjectedPupilNumbers),
					() => Assert.Equal(existingProject.YearTwoProjectedCapacity, getProject.YearTwoProjectedCapacity),
					() => Assert.Equal(existingProject.YearTwoProjectedPupilNumbers, getProject.YearTwoProjectedPupilNumbers),
					() => Assert.Equal(existingProject.YearThreeProjectedCapacity, getProject.YearThreeProjectedCapacity),
					() => Assert.Equal(existingProject.YearThreeProjectedPupilNumbers, getProject.YearThreeProjectedPupilNumbers),
					() => Assert.Equal(existingProject.KeyStage2PerformanceAdditionalInformation, getProject.KeyStage2PerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.KeyStage4PerformanceAdditionalInformation, getProject.KeyStage4PerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.KeyStage5PerformanceAdditionalInformation, getProject.KeyStage5PerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.PreviousHeadTeacherBoardDateQuestion, getProject.PreviousHeadTeacherBoardDateQuestion),
					() => Assert.Equal(existingProject.ConversionSupportGrantAmount, getProject.ConversionSupportGrantAmount),
					() => Assert.Equal(existingProject.ConversionSupportGrantChangeReason, getProject.ConversionSupportGrantChangeReason),
					() => Assert.Equal(existingProject.ProjectStatus, getProject.ProjectStatus)
		);
	}
}
