using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Commands.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.Project;
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
		var legacyProjectController = new LegacyProjectController(_legacyProjectGetQuery, _legacyProjectUpdateCommand);
		var existingProject = _fixture.Create<ProjectState>();
		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = _fixture.Build<LegacyProjectServiceModel>()
			.With(p => p.Id, existingProject.Id)
			.With(p => p.Urn, existingProject.Urn)
			.Create();

		// Act		
		var updateResult = await legacyProjectController.Patch(updatedProject);

		// Assert
		DfeAssert.OkObjectResult(updateResult);

		var getResult = await legacyProjectController.Get(updatedProject.Id);

		(_, var getProject) = DfeAssert.OkObjectResult(getResult);

		Assert.Multiple(
					() => Assert.Equal(updatedProject.HeadTeacherBoardDate, getProject.HeadTeacherBoardDate),
					() => Assert.Equal(updatedProject.Author, getProject.Author),
					() => Assert.Equal(updatedProject.ClearedBy, getProject.ClearedBy),
					() => Assert.Equal(updatedProject.ProposedAcademyOpeningDate, getProject.ProposedAcademyOpeningDate),
					() => Assert.Equal(updatedProject.PublishedAdmissionNumber, getProject.PublishedAdmissionNumber),
					() => Assert.Equal(updatedProject.ViabilityIssues, getProject.ViabilityIssues),
					() => Assert.Equal(updatedProject.FinancialDeficit, getProject.FinancialDeficit),
					() => Assert.Equal(updatedProject.RationaleForProject, getProject.RationaleForProject),
					() => Assert.Equal(updatedProject.RisksAndIssues, getProject.RisksAndIssues),
					() => Assert.Equal(updatedProject.RevenueCarryForwardAtEndMarchCurrentYear, getProject.RevenueCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(updatedProject.ProjectedRevenueBalanceAtEndMarchNextYear, getProject.ProjectedRevenueBalanceAtEndMarchNextYear),
					() => Assert.Equal(updatedProject.RationaleSectionComplete, getProject.RationaleSectionComplete),
					() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateSentDate, getProject.LocalAuthorityInformationTemplateSentDate),
					() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateReturnedDate, getProject.LocalAuthorityInformationTemplateReturnedDate),
					() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateComments, getProject.LocalAuthorityInformationTemplateComments),
					() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateLink, getProject.LocalAuthorityInformationTemplateLink),
					() => Assert.Equal(updatedProject.LocalAuthorityInformationTemplateSectionComplete, getProject.LocalAuthorityInformationTemplateSectionComplete),
					() => Assert.Equal(updatedProject.RecommendationForProject, getProject.RecommendationForProject),
					() => Assert.Equal(updatedProject.AcademyOrderRequired, getProject.AcademyOrderRequired),
					() => Assert.Equal(updatedProject.SchoolAndTrustInformationSectionComplete, getProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(updatedProject.DistanceFromSchoolToTrustHeadquarters, getProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(updatedProject.DistanceFromSchoolToTrustHeadquarters, getProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(updatedProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, getProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
					() => Assert.Equal(updatedProject.MemberOfParliamentName, getProject.MemberOfParliamentName),
					() => Assert.Equal(updatedProject.MemberOfParliamentParty, getProject.MemberOfParliamentParty),
					() => Assert.Equal(updatedProject.GeneralInformationSectionComplete, getProject.GeneralInformationSectionComplete),
					() => Assert.Equal(updatedProject.RisksAndIssuesSectionComplete, getProject.RisksAndIssuesSectionComplete),
					() => Assert.Equal(updatedProject.SchoolPerformanceAdditionalInformation, getProject.SchoolPerformanceAdditionalInformation),
					() => Assert.Equal(updatedProject.CapitalCarryForwardAtEndMarchCurrentYear, getProject.CapitalCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(updatedProject.CapitalCarryForwardAtEndMarchNextYear, getProject.CapitalCarryForwardAtEndMarchNextYear),
					() => Assert.Equal(updatedProject.SchoolBudgetInformationAdditionalInformation, getProject.SchoolBudgetInformationAdditionalInformation),
					() => Assert.Equal(updatedProject.SchoolAndTrustInformationSectionComplete, getProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(updatedProject.SchoolPupilForecastsAdditionalInformation, getProject.SchoolPupilForecastsAdditionalInformation),
					() => Assert.Equal(updatedProject.YearOneProjectedCapacity, getProject.YearOneProjectedCapacity),
					() => Assert.Equal(updatedProject.YearOneProjectedPupilNumbers, getProject.YearOneProjectedPupilNumbers),
					() => Assert.Equal(updatedProject.YearTwoProjectedCapacity, getProject.YearTwoProjectedCapacity),
					() => Assert.Equal(updatedProject.YearTwoProjectedPupilNumbers, getProject.YearTwoProjectedPupilNumbers),
					() => Assert.Equal(updatedProject.YearThreeProjectedCapacity, getProject.YearThreeProjectedCapacity),
					() => Assert.Equal(updatedProject.YearThreeProjectedPupilNumbers, getProject.YearThreeProjectedPupilNumbers),
					() => Assert.Equal(updatedProject.KeyStage2PerformanceAdditionalInformation, getProject.KeyStage2PerformanceAdditionalInformation),
					() => Assert.Equal(updatedProject.KeyStage4PerformanceAdditionalInformation, getProject.KeyStage4PerformanceAdditionalInformation),
					() => Assert.Equal(updatedProject.KeyStage5PerformanceAdditionalInformation, getProject.KeyStage5PerformanceAdditionalInformation),
					() => Assert.Equal(updatedProject.PreviousHeadTeacherBoardDateQuestion, getProject.PreviousHeadTeacherBoardDateQuestion),
					() => Assert.Equal(updatedProject.ConversionSupportGrantAmount, getProject.ConversionSupportGrantAmount),
					() => Assert.Equal(updatedProject.ConversionSupportGrantChangeReason, getProject.ConversionSupportGrantChangeReason),
					() => Assert.Equal(updatedProject.ProjectStatus, getProject.ProjectStatus)
		);
	}

	[Fact]
	public async Task ProjectExists___PartialProjectIsUpdated()
	{
		// Arrange
		var legacyProjectController = new LegacyProjectController(_legacyProjectGetQuery, _legacyProjectUpdateCommand);
		var existingProject = _fixture.Create<ProjectState>();

		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = new LegacyProjectServiceModel(existingProject.Id)
		{
			ProjectStatus = "TestStatus",
			LocalAuthority = "LocalAuthority",
			EqualitiesImpactAssessmentConsidered = "Yes sir"
		};

		// Act		
		var updateResult = await legacyProjectController.Patch(updatedProject);

		// Assert
		DfeAssert.OkObjectResult(updateResult);

		var getResult = await legacyProjectController.Get(updatedProject.Id);

		(_, var getProject) = DfeAssert.OkObjectResult(getResult);

		existingProject.ProjectStatus = updatedProject.ProjectStatus;
		existingProject.LocalAuthority = updatedProject.LocalAuthority;
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
