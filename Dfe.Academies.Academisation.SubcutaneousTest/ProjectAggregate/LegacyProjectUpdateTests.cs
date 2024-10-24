using System.Reflection;
using AutoFixture;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate;

public class ProjectUpdateTests
{
	private readonly Fixture _fixture = new();
	private readonly AcademisationContext _context;

	// data

	// service
	private readonly IConversionProjectQueryService _legacyProjectGetQuery;
	private IMediator _mediatr;
	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository;


	public ProjectUpdateTests()
	{
		_mockAdvisoryBoardDecisionRepository = new();
		_context = new TestProjectContext(_mediatr).CreateContext();


		// data
		IConversionProjectRepository conversionProjectRepository = new ConversionProjectRepository(_context);
		IFormAMatProjectRepository formAMatProjectRepository = new FormAMatProjectRepository(_context);
		IProjectUpdateDataCommand projectUpdateDataCommand = new ProjectUpdateDataCommand(_context);

		// service
		_legacyProjectGetQuery = new ConversionProjectQueryService(conversionProjectRepository, formAMatProjectRepository, _mockAdvisoryBoardDecisionRepository.Object);
		var services = new ServiceCollection();

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(ConversionProjectUpdateCommandHandler))!));
		services.AddScoped(x => conversionProjectRepository);
		services.AddScoped(x => projectUpdateDataCommand);

		_mediatr = services.BuildServiceProvider().GetService<IMediator>()!;
	}


	[Fact]
	public async Task ProjectExists___FullProjectIsUpdated()
	{
		// Arrange
		var legacyProjectController = new ProjectController(_legacyProjectGetQuery, _mediatr);
		// had to do this to make the equality operator happy, weirdly the annex b form is missing from equality
		var existingProjectDetails = _fixture.Build<ProjectDetails>()
			.With(x => x.ExternalApplicationFormSaved, true)
			.With(x => x.ExternalApplicationFormUrl, "test//url")
			.Create();

		var existingProject = new Project(101, existingProjectDetails);

		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = _fixture.Build<ConversionProjectServiceModel>()
			.With(p => p.Id, existingProject.Id)
			.With(p => p.Urn, existingProject.Details.Urn)
			.With(p => p.ExternalApplicationFormSaved, existingProject.Details.ExternalApplicationFormSaved)
			.With(p => p.IsReadOnly, existingProject.ReadOnlyDate.HasValue)
			.With(p => p.ProjectSentToCompleteDate, existingProject.ReadOnlyDate)
			// excluded from update so need to be set for equality to assert
			.With(x => x.KeyStage2PerformanceAdditionalInformation, existingProject.Details.KeyStage2PerformanceAdditionalInformation)
			.With(x => x.KeyStage4PerformanceAdditionalInformation, existingProject.Details.KeyStage4PerformanceAdditionalInformation)
			.With(x => x.KeyStage5PerformanceAdditionalInformation, existingProject.Details.KeyStage5PerformanceAdditionalInformation)
			.With(x => x.EducationalAttendanceAdditionalInformation, existingProject.Details.EducationalAttendanceAdditionalInformation)
			.With(x => x.ProposedConversionDate, existingProject.Details.ProposedConversionDate)
			.Create();

		updatedProject.Notes?.Clear();

		// Act
		var updateResult = await legacyProjectController.Patch(updatedProject.Id, updatedProject, default);

		// Assert
		(_, ConversionProjectServiceModel project) = DfeAssert.OkObjectResult(updateResult);

		Assert.True(updatedProject.Equals(project));
	}


	[Fact]
	public async Task ProjectExists_FullProjectIsReturnedOnGet()
	{
		// Arrange
		var legacyProjectController = new ProjectController(_legacyProjectGetQuery, _mediatr);
		var existingProject = _fixture.Create<Project>();
		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = _fixture.Build<ConversionProjectServiceModel>()
			.With(p => p.Id, existingProject.Id)
			.With(p => p.Urn, existingProject.Details.Urn)
			.Create();

		updatedProject.Notes?.Clear();

		// Act
		var updateResult = await legacyProjectController.Patch(updatedProject.Id, updatedProject, default);
		DfeAssert.OkObjectResult(updateResult);

		var getResult = await legacyProjectController.Get(updatedProject.Id, default);

		// Assert
		(_, ConversionProjectServiceModel project) = DfeAssert.OkObjectResult(getResult);

		updatedProject.Equals(project);
	}

	[Fact]
	public async Task ProjectExists___PartialProjectIsUpdated()
	{
		// Arrange
		var legacyProjectController = new ProjectController(_legacyProjectGetQuery, _mediatr);
		var existingProject = _fixture.Create<Project>();

		await _context.Projects.AddAsync(existingProject);
		await _context.SaveChangesAsync();

		var updatedProject = new ConversionProjectServiceModel(existingProject.Id, existingProject.Details.Urn)
		{
			ProjectStatus = "TestStatus"
		};

		// Act
		var updateResult = await legacyProjectController.Patch(updatedProject.Id, updatedProject, default);

		// Assert
		DfeAssert.OkObjectResult(updateResult);

		var getResult = await legacyProjectController.Get(updatedProject.Id, default);

		(_, ConversionProjectServiceModel getProject) = DfeAssert.OkObjectResult(getResult);

		//existingProject.Details.ProjectStatus = updatedProject.Details.ProjectStatus;

		Assert.Multiple(
					() => Assert.Equal(existingProject.Details.HeadTeacherBoardDate, getProject.HeadTeacherBoardDate),
					() => Assert.Equal(existingProject.Details.PartOfPfiScheme, getProject.PartOfPfiScheme),
					() => Assert.Equal(existingProject.Details.PfiSchemeDetails, getProject.PfiSchemeDetails),
					() => Assert.Equal(existingProject.Details.Author, getProject.Author),
					() => Assert.Equal(existingProject.Details.ClearedBy, getProject.ClearedBy),
					() => Assert.Equal(existingProject.Details.ProposedConversionDate, getProject.ProposedConversionDate),
					() => Assert.Equal(existingProject.Details.PublishedAdmissionNumber, getProject.PublishedAdmissionNumber),
					() => Assert.Equal(existingProject.Details.ViabilityIssues, getProject.ViabilityIssues),
					() => Assert.Equal(existingProject.Details.FinancialDeficit, getProject.FinancialDeficit),
					() => Assert.Equal(existingProject.Details.RationaleForProject, getProject.RationaleForProject),
					() => Assert.Equal(existingProject.Details.RisksAndIssues, getProject.RisksAndIssues),
					() => Assert.Equal(existingProject.Details.EndOfCurrentFinancialYear, getProject.EndOfCurrentFinancialYear),
					() => Assert.Equal(existingProject.Details.EndOfNextFinancialYear, getProject.EndOfNextFinancialYear),
					() => Assert.Equal(existingProject.Details.RevenueCarryForwardAtEndMarchCurrentYear, getProject.RevenueCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(existingProject.Details.ProjectedRevenueBalanceAtEndMarchNextYear, getProject.ProjectedRevenueBalanceAtEndMarchNextYear),
					() => Assert.Equal(existingProject.Details.RationaleSectionComplete, getProject.RationaleSectionComplete),
					() => Assert.Equal(existingProject.Details.LocalAuthorityInformationTemplateSentDate, getProject.LocalAuthorityInformationTemplateSentDate),
					() => Assert.Equal(existingProject.Details.LocalAuthorityInformationTemplateReturnedDate, getProject.LocalAuthorityInformationTemplateReturnedDate),
					() => Assert.Equal(existingProject.Details.LocalAuthorityInformationTemplateComments, getProject.LocalAuthorityInformationTemplateComments),
					() => Assert.Equal(existingProject.Details.LocalAuthorityInformationTemplateLink, getProject.LocalAuthorityInformationTemplateLink),
					() => Assert.Equal(existingProject.Details.LocalAuthorityInformationTemplateSectionComplete, getProject.LocalAuthorityInformationTemplateSectionComplete),
					() => Assert.Equal(existingProject.Details.RecommendationForProject, getProject.RecommendationForProject),
					() => Assert.Equal(existingProject.Details.SchoolAndTrustInformationSectionComplete, getProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(existingProject.Details.DistanceFromSchoolToTrustHeadquarters, getProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(existingProject.Details.DistanceFromSchoolToTrustHeadquarters, getProject.DistanceFromSchoolToTrustHeadquarters),
					() => Assert.Equal(existingProject.Details.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, getProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
					() => Assert.Equal(existingProject.Details.MemberOfParliamentNameAndParty, getProject.MemberOfParliamentNameAndParty),
					() => Assert.Equal(existingProject.Details.SchoolOverviewSectionComplete, getProject.SchoolOverviewSectionComplete),
					() => Assert.Equal(existingProject.Details.RisksAndIssuesSectionComplete, getProject.RisksAndIssuesSectionComplete),
					() => Assert.Equal(existingProject.Details.Consultation, getProject.Consultation),
					() => Assert.Equal(existingProject.Details.DiocesanTrust, getProject.DiocesanTrust),
					() => Assert.Equal(existingProject.Details.FoundationConsent, getProject.FoundationConsent),
					() => Assert.Equal(existingProject.Details.GoverningBodyResolution, getProject.GoverningBodyResolution),
					() => Assert.Equal(existingProject.Details.LegalRequirementsSectionComplete, getProject.LegalRequirementsSectionComplete),
					() => Assert.Equal(existingProject.Details.SchoolPerformanceAdditionalInformation, getProject.SchoolPerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.Details.CapitalCarryForwardAtEndMarchCurrentYear, getProject.CapitalCarryForwardAtEndMarchCurrentYear),
					() => Assert.Equal(existingProject.Details.CapitalCarryForwardAtEndMarchNextYear, getProject.CapitalCarryForwardAtEndMarchNextYear),
					() => Assert.Equal(existingProject.Details.SchoolBudgetInformationAdditionalInformation, getProject.SchoolBudgetInformationAdditionalInformation),
					() => Assert.Equal(existingProject.Details.SchoolAndTrustInformationSectionComplete, getProject.SchoolAndTrustInformationSectionComplete),
					() => Assert.Equal(existingProject.Details.SchoolPupilForecastsAdditionalInformation, getProject.SchoolPupilForecastsAdditionalInformation),
					() => Assert.Equal(existingProject.Details.YearOneProjectedCapacity, getProject.YearOneProjectedCapacity),
					() => Assert.Equal(existingProject.Details.YearOneProjectedPupilNumbers, getProject.YearOneProjectedPupilNumbers),
					() => Assert.Equal(existingProject.Details.YearTwoProjectedCapacity, getProject.YearTwoProjectedCapacity),
					() => Assert.Equal(existingProject.Details.YearTwoProjectedPupilNumbers, getProject.YearTwoProjectedPupilNumbers),
					() => Assert.Equal(existingProject.Details.YearThreeProjectedCapacity, getProject.YearThreeProjectedCapacity),
					() => Assert.Equal(existingProject.Details.YearThreeProjectedPupilNumbers, getProject.YearThreeProjectedPupilNumbers),
					() => Assert.Equal(existingProject.Details.KeyStage2PerformanceAdditionalInformation, getProject.KeyStage2PerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.Details.KeyStage4PerformanceAdditionalInformation, getProject.KeyStage4PerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.Details.KeyStage5PerformanceAdditionalInformation, getProject.KeyStage5PerformanceAdditionalInformation),
					() => Assert.Equal(existingProject.Details.PreviousHeadTeacherBoardDateQuestion, getProject.PreviousHeadTeacherBoardDateQuestion),
					() => Assert.Equal(existingProject.Details.ConversionSupportGrantAmount, getProject.ConversionSupportGrantAmount),
					() => Assert.Equal(existingProject.Details.ConversionSupportGrantChangeReason, getProject.ConversionSupportGrantChangeReason),
					() => Assert.Equal(existingProject.Details.ConversionSupportGrantType, getProject.ConversionSupportGrantType),
					() => Assert.Equal(existingProject.Details.ConversionSupportGrantEnvironmentalImprovementGrant, getProject.ConversionSupportGrantEnvironmentalImprovementGrant),
					() => Assert.Equal(existingProject.Details.ConversionSupportGrantAmountChanged, getProject.ConversionSupportGrantAmountChanged),
					() => Assert.Equal(existingProject.Details.ProjectStatus, getProject.ProjectStatus)
		);
	}
}
