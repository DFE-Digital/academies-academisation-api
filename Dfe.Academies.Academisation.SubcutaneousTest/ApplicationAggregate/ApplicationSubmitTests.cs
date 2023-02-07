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
using Dfe.Academies.Academisation.Service.Commands.Project;
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

	private readonly IProjectFactory _projectFactory = new ProjectFactory();
	private readonly IApplicationSubmissionService _applicationSubmissionService;
	private readonly IApplicationCreateCommand _applicationCreateCommand;
	private readonly IApplicationQueryService _applicationQueryService;
	private readonly IApplicationUpdateCommand _applicationUpdateCommand;
	private readonly ITrustQueryService _trustQueryService;
	private readonly IApplicationListByUserQuery _applicationsListByUserQuery;
	private readonly ILogger<ApplicationController> _applicationLogger;
	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();
	private readonly IApplicationRepository _repo;
	private readonly IProjectCreateDataCommand _projectCreateDataCommand;
	private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
	private readonly Mock<IDateTimeProvider> _DateTimeProvider = new Mock<IDateTimeProvider>();
	private readonly Mock<IMediator> _mediator;
	public ApplicationSubmitTests()
	{
		_context = new TestApplicationContext().CreateContext();

		_applicationSubmissionService = new ApplicationSubmissionService(_projectFactory, _DateTimeProvider.Object);
		_repo = new ApplicationRepository(_context, _mapper.Object);
		_applicationQueryService = new ApplicationQueryService(_repo, _mapper.Object);
		_projectCreateDataCommand = new ProjectCreateDataCommand(_context);
		_trustQueryService = new TrustQueryService(_context, _mapper.Object);
		_applicationsListByUserQuery = new Mock<IApplicationListByUserQuery>().Object;
		_applicationLogger = new Mock<ILogger<ApplicationController>>().Object;
		_applicationUpdateCommand = new ApplicationUpdateCommand(_repo);
		_applicationCreateCommand = new ApplicationCreateCommand(_applicationFactory, _repo, _mapper.Object);
		_mediator = new Mock<IMediator>();

		var submitApplicationHandler = new ApplicationSubmitCommandHandler(_repo, _projectCreateDataCommand, _applicationSubmissionService);

		_mediator.Setup(x => x.Send(It.IsAny<SubmitApplicationCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CommandOrCreateResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await submitApplicationHandler.Handle((SubmitApplicationCommand)cmd, ct);
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
			_applicationCreateCommand,
			_applicationQueryService,
			_applicationUpdateCommand,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		ApplicationCreateRequestModel applicationCreateRequestModel = _fixture
			.Create<ApplicationCreateRequestModel>();

		var createResult = await applicationController.Post(applicationCreateRequestModel);

		(_, var createdPayload) = DfeAssert.CreatedAtRoute(createResult, "GetApplication");

		var school = _fixture.Create<ApplicationSchoolServiceModel>();

		var updateRequest = new ApplicationUpdateRequestModel(createdPayload.ApplicationId, createdPayload.ApplicationType, createdPayload.ApplicationStatus, createdPayload.Contributors, new List<ApplicationSchoolServiceModel> { school });

		var updateResult = await applicationController.Update(updateRequest.ApplicationId, updateRequest);
		DfeAssert.OkResult(updateResult);

		// Act
		var submissionResult = await applicationController.Submit(createdPayload.ApplicationId);

		// Assert
		Assert.IsType<CreatedAtRouteResult>(submissionResult);

		var applicationGetResult = await applicationController.Get(createdPayload.ApplicationId);
		(_, ApplicationServiceModel getPayload) = DfeAssert.OkObjectResult(applicationGetResult);

		Assert.Equal(ApplicationStatus.Submitted, getPayload.ApplicationStatus);

		var projectController = new LegacyProjectController(new LegacyProjectGetQuery(new ProjectGetDataQuery(_context)), Mock.Of<ILegacyProjectListGetQuery>(),
			Mock.Of<IProjectGetStatusesQuery>(), Mock.Of<ILegacyProjectUpdateCommand>(), Mock.Of<ILegacyProjectAddNoteCommand>(), Mock.Of<ILegacyProjectDeleteNoteCommand>(), Mock.Of<ICreateInvoluntaryProjectCommand>());
		var projectResult = await projectController.Get(1);

		(_, LegacyProjectServiceModel project) = DfeAssert.OkObjectResult(projectResult);

		Assert.Multiple(
			() => Assert.Equal("Converter Pre-AO (C)", project.ProjectStatus),
			() => Assert.Equal(DateTime.Today.AddMonths(6), project.OpeningDate),
			() => Assert.Equal("Converter", project.AcademyTypeAndRoute),
			() => Assert.Equal(school.SchoolConversionTargetDate, project.ProposedAcademyOpeningDate),
			() => Assert.Equal(25000.0m, project.ConversionSupportGrantAmount),
			() => Assert.Equal(school.SchoolCapacityPublishedAdmissionsNumber.ToString(), project.PublishedAdmissionNumber),
			() => Assert.Equal(ToYesNoString(school.LandAndBuildings!.PartOfPfiScheme), project.PartOfPfiScheme),
			() => Assert.Equal(school.ProjectedPupilNumbersYear1, project.YearOneProjectedPupilNumbers),
			() => Assert.Equal(school.ProjectedPupilNumbersYear2, project.YearTwoProjectedPupilNumbers),
			() => Assert.Equal(school.ProjectedPupilNumbersYear3, project.YearThreeProjectedPupilNumbers)
		);
	}

	private static string ToYesNoString(bool? value)
	{
		if (!value.HasValue) return string.Empty;
		return value == true ? "Yes" : "No";
	}
}
