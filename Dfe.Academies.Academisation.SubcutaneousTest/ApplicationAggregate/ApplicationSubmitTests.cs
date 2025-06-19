using AutoFixture;
using AutoMapper;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace Dfe.Academies.Academisation.SubcutaneousTest.ApplicationAggregate;

public class ApplicationSubmitTests
{
	private readonly Fixture _fixture = new();
	private readonly Faker _faker = new();
	private readonly AcademisationContext _context;
	private const string Converter = "Converter";

	private readonly IProjectFactory _projectFactory = new ProjectFactory();
	private readonly IApplicationSubmissionService _applicationSubmissionService;
	private readonly IApplicationQueryService _applicationQueryService;
	private readonly ITrustQueryService _trustQueryService;
	private readonly ILogger<ApplicationController> _applicationLogger;
	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();
	private readonly IApplicationRepository _applicationRepo;
	private readonly IConversionProjectRepository _conversionRepo;
	private readonly Mock<IMapper> _mapper = new();
	private readonly Mock<IDateTimeProvider> _DateTimeProvider = new();
	private readonly Mock<IMediator> _mediator = new();
	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository = new();
	public ApplicationSubmitTests()
	{
		_context = new TestApplicationContext(_mediator.Object).CreateContext();

		_applicationSubmissionService = new ApplicationSubmissionService(_projectFactory, _DateTimeProvider.Object);
		_applicationRepo = new ApplicationRepository(_context, _mapper.Object);
		_conversionRepo = new ConversionProjectRepository(_context);
		_applicationQueryService = new ApplicationQueryService(_applicationRepo, _mapper.Object);
		_trustQueryService = new TrustQueryService(_context, _mapper.Object);
		_applicationLogger = new Mock<ILogger<ApplicationController>>().Object;
		_mediator = new Mock<IMediator>();

		var submitApplicationHandler = new ApplicationSubmitCommandHandler(_applicationRepo, _conversionRepo, _applicationSubmissionService);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationSubmitCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CommandOrCreateResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await submitApplicationHandler.Handle((ApplicationSubmitCommand)cmd, ct);
			});

		var applicationCreateCommandHandler = new ApplicationCreateCommandHandler(_applicationFactory, _applicationRepo, _mapper.Object);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationCreateCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CreateResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await applicationCreateCommandHandler.Handle((ApplicationCreateCommand)cmd, ct);
			});

		var applicationUpdateCommandHandler = new ApplicationUpdateCommandHandler(_applicationRepo);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationUpdateCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CommandResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await applicationUpdateCommandHandler.Handle((ApplicationUpdateCommand)cmd, ct);
			});

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Internet.Email()));

		_fixture.Customize<ApplicationSchoolServiceModel>(sd =>
			sd.With(s => s.SchoolConversionApproverContactEmail, _faker.Internet.Email())
			.With(s => s.Id, 0)
			.With(s => s.SchoolConversionContactChairEmail, _faker.Internet.Email())
			.With(s => s.SchoolConversionContactHeadEmail, _faker.Internet.Email())
			.With(s => s.SchoolConversionMainContactOtherEmail, _faker.Internet.Email()));

		_fixture.Customize<LoanServiceModel>(composer =>
			composer
				.With(s => s.LoanId, 0));

		_fixture.Customize<LeaseServiceModel>(composer =>
			composer
				.With(s => s.LeaseId, 0));
	}

	[Fact]
	public async Task JoinAMatApplicationExists___ApplicationIsSubmitted_And_ProjectIsCreated()
	{
		// Arrange
		var applicationController = new ApplicationController(
			_applicationQueryService,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		ApplicationCreateCommand applicationCreateRequestModel = _fixture
			.Create<ApplicationCreateCommand>();

		var createResult = await applicationController.Post(applicationCreateRequestModel, default);

		(_, var createdPayload) = DfeAssert.CreatedAtRoute(createResult, "GetApplication");

		var school = _fixture.Create<ApplicationSchoolServiceModel>();

		var updateRequest = new ApplicationUpdateCommand(createdPayload.ApplicationId, createdPayload.ApplicationType, createdPayload.ApplicationStatus, createdPayload.Contributors,
			new List<ApplicationSchoolServiceModel> { school });

		var updateResult = await applicationController.Update(updateRequest.ApplicationId, updateRequest, default);
		DfeAssert.OkResult(updateResult);

		// Act
		var submissionResult = await applicationController.Submit(createdPayload.ApplicationId);

		// Assert
		Assert.IsType<CreatedAtRouteResult>(submissionResult);

		var applicationGetResult = await applicationController.Get(createdPayload.ApplicationId);
		(_, ApplicationServiceModel getPayload) = DfeAssert.OkObjectResult(applicationGetResult);

		Assert.Equal(ApplicationStatus.Submitted, getPayload.ApplicationStatus);

		var projectController = new ProjectController(new ConversionProjectQueryService(new ConversionProjectRepository(_context), new FormAMatProjectRepository(_context), _mockAdvisoryBoardDecisionRepository.Object), Mock.Of<IMediator>());
		var projectResult = await projectController.Get(1, default);

		(_, ConversionProjectServiceModel project) = DfeAssert.OkObjectResult(projectResult);

		AssertProject(school, project, Converter);
	}

	[Fact]
	public async Task FormAMatApplicationExists___ApplicationIsSubmitted_And_ProjectsAreCreated()
	{
		// Arrange
		var applicationController = new ApplicationController(
			_applicationQueryService,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		ApplicationCreateCommand applicationCreateRequestModel = new(
			ApplicationType.FormAMat,
			_fixture.Create<ContributorRequestModel>());
		var createResult = await applicationController.Post(applicationCreateRequestModel, default);

		(_, var createdPayload) = DfeAssert.CreatedAtRoute(createResult, "GetApplication");

		var firstSchool = _fixture.Create<ApplicationSchoolServiceModel>();
		var secondSchool = _fixture.Create<ApplicationSchoolServiceModel>();
		var thirdSchool = _fixture.Create<ApplicationSchoolServiceModel>();

		var updateRequest = new ApplicationUpdateCommand(
			createdPayload.ApplicationId,
			createdPayload.ApplicationType,
			createdPayload.ApplicationStatus,
			createdPayload.Contributors,
			new List<ApplicationSchoolServiceModel>
				{ firstSchool, secondSchool, thirdSchool });

		var updateResult = await applicationController.Update(updateRequest.ApplicationId, updateRequest, default);
		DfeAssert.OkResult(updateResult);

		// Act
		var submissionResult = await applicationController.Submit(createdPayload.ApplicationId);

		// Assert
		Assert.IsType<CreatedAtRouteResult>(submissionResult);

		var applicationGetResultUsingId = await applicationController.Get(createdPayload.ApplicationId!);
		(_, ApplicationServiceModel getPayload) = DfeAssert.OkObjectResult(applicationGetResultUsingId);

		Assert.Equal(ApplicationStatus.Submitted, getPayload.ApplicationStatus);

		var projectController = new ProjectController(new ConversionProjectQueryService(new ConversionProjectRepository(_context), new FormAMatProjectRepository(_context), _mockAdvisoryBoardDecisionRepository.Object), Mock.Of<IMediator>());
		var projectResults = await projectController.GetProjects(new GetProjectSearchModel(1, 3, null, null, null, null, new[] { $"A2B_{createdPayload.ApplicationReference!}" }));

		(_, PagedDataResponse<ConversionProjectServiceModel> projects) = DfeAssert.OkObjectResult(projectResults);

		AssertProject(firstSchool, projects.Data.FirstOrDefault(x => x.SchoolName == firstSchool.SchoolName)!, Converter, true);
		AssertProject(secondSchool, projects.Data.FirstOrDefault(x => x.SchoolName == secondSchool.SchoolName)!, Converter, true);
		AssertProject(thirdSchool, projects.Data.FirstOrDefault(x => x.SchoolName == thirdSchool.SchoolName)!, Converter, true);
	}

	private static void AssertProject(ApplicationSchoolServiceModel school, ConversionProjectServiceModel project, string type, bool isFormAMat = false)
	{
		Assert.Multiple(
		() => Assert.Equal("Converter Pre-AO (C)", project.ProjectStatus),
		() => Assert.Equal(type, project.AcademyTypeAndRoute),
		() => Assert.Equal(isFormAMat, project.IsFormAMat),
		() => Assert.Null(project.ProposedConversionDate),
		() => Assert.Equal(school.SchoolCapacityPublishedAdmissionsNumber.ToString(), project.PublishedAdmissionNumber),
		() => Assert.Equal(ToYesNoString(school.LandAndBuildings!.PartOfPfiScheme), project.PartOfPfiScheme),
		() => Assert.Equal(school.ProjectedPupilNumbersYear1, project.YearOneProjectedPupilNumbers),
		() => Assert.Equal(school.ProjectedPupilNumbersYear2, project.YearTwoProjectedPupilNumbers),
		() => Assert.Equal(school.ProjectedPupilNumbersYear3, project.YearThreeProjectedPupilNumbers));
	}

	private static string ToYesNoString(bool? value)
	{
		if (!value.HasValue) return string.Empty;
		return value == true ? "Yes" : "No";
	}
}
