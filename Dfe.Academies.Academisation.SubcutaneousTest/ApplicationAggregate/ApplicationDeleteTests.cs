using AutoFixture;
using AutoMapper;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
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

public class ApplicationDeleteTests
{
	private readonly Fixture _fixture = new();
	private readonly Faker _faker = new();
	private readonly AcademisationContext _context;
	private const string Converter = "Converter";

	private readonly IProjectFactory _projectFactory = new ProjectFactory();
	private readonly IApplicationSubmissionService _applicationSubmissionService;
	private readonly IApplicationQueryService _applicationQueryService;
	private readonly ITrustQueryService _trustQueryService;
	private readonly IApplicationListByUserQuery _applicationsListByUserQuery;
	private readonly ILogger<ApplicationController> _applicationLogger;
	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();
	private readonly IApplicationRepository _repo;
	private readonly IProjectCreateDataCommand _projectCreateDataCommand;
	private readonly Mock<IMapper> _mapper = new();
	private readonly Mock<IDateTimeProvider> _DateTimeProvider = new();
	private readonly Mock<IMediator> _mediator;
	public ApplicationDeleteTests()
	{
		_context = new TestApplicationContext().CreateContext();

		_applicationSubmissionService = new ApplicationSubmissionService(_projectFactory, _DateTimeProvider.Object);
		_repo = new ApplicationRepository(_context, _mapper.Object);
		_applicationQueryService = new ApplicationQueryService(_repo, _mapper.Object);
		_projectCreateDataCommand = new ProjectCreateDataCommand(_context);
		_trustQueryService = new TrustQueryService(_context, _mapper.Object);
		_applicationsListByUserQuery = new Mock<IApplicationListByUserQuery>().Object;
		_applicationLogger = new Mock<ILogger<ApplicationController>>().Object;
		_mediator = new Mock<IMediator>();

		var submitApplicationHandler = new ApplicationSubmitCommandHandler(_repo, _projectCreateDataCommand, _applicationSubmissionService);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationSubmitCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CommandOrCreateResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await submitApplicationHandler.Handle((ApplicationSubmitCommand)cmd, ct);
			});

		var applicationCreateCommandHandler = new ApplicationCreateCommandHandler(_applicationFactory, _repo, _mapper.Object);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationCreateCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CreateResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await applicationCreateCommandHandler.Handle((ApplicationCreateCommand)cmd, ct);
			});

		var applicationUpdateCommandHandler = new ApplicationUpdateCommandHandler(_repo);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationUpdateCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CommandResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await applicationUpdateCommandHandler.Handle((ApplicationUpdateCommand)cmd, ct);
			});

        	var applicationDeleteCommandHandler = new ApplicationDeleteCommandHandler(_repo);

        _mediator.Setup(x => x.Send(It.IsAny<ApplicationDeleteCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CommandResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await applicationDeleteCommandHandler.Handle((ApplicationDeleteCommand)cmd, ct);
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
	public async Task JoinAMatApplicationExists___ApplicationIsDeleted()
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

		// Act
		var deleteResult = await applicationController.DeleteApplication(createdPayload.ApplicationId);

        //Assert
        Assert.IsAssignableFrom<OkObjectResult>(deleteResult);	
	}

	[Fact]
	public async Task FormAMatApplicationExists___ApplicationIsDeleted()
	{
		// Arrange
		var applicationController = new ApplicationController(
			_applicationQueryService,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		ApplicationCreateCommand applicationCreateRequestModel =  new(
			ApplicationType.FormAMat,
			_fixture.Create<ContributorRequestModel>());
		var createResult = await applicationController.Post(applicationCreateRequestModel, default);

		(_, var createdPayload) = DfeAssert.CreatedAtRoute(createResult, "GetApplication");

		
		// Act
		var deleteResult = await applicationController.DeleteApplication(createdPayload.ApplicationId);

        //Assert
        Assert.IsAssignableFrom<OkObjectResult>(deleteResult);	;
	}

    [Fact]
	public async Task ApplicationNotFound()
	{
		// Arrange
		var applicationController = new ApplicationController(
			_applicationQueryService,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		ApplicationCreateCommand applicationCreateRequestModel =  new(
			ApplicationType.FormAMat,
			_fixture.Create<ContributorRequestModel>());
		var createResult = await applicationController.Post(applicationCreateRequestModel, default);

		(_, var createdPayload) = DfeAssert.CreatedAtRoute(createResult, "GetApplication");

		
		// Act
		var deleteResult = await applicationController.DeleteApplication(123);

        //Assert
        Assert.IsAssignableFrom<NotFoundResult>(deleteResult);	;
	}

        
}
