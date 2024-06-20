using AutoFixture;
using AutoMapper;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ApplicationAggregate;

public class ApplicationUpdateTests
{
	private readonly Fixture _fixture = new();
	private readonly Faker _faker = new();

	private readonly IApplicationQueryService _applicationQueryService;
	private readonly ILogger<ApplicationController> _applicationLogger;
	private readonly Mock<IMediator> _mediator;
	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();

	private readonly AcademisationContext _context;
	private readonly IApplicationRepository _repo;
	private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
	private readonly ApplicationController _applicationController;
	private readonly ITrustQueryService _trustQueryService;

	public ApplicationUpdateTests()
	{
		_context = new TestApplicationContext(_mediator.Object).CreateContext();
		_repo = new ApplicationRepository(_context, _mapper.Object);
		_applicationQueryService = new ApplicationQueryService(_repo, _mapper.Object);
		_trustQueryService = new TrustQueryService(_context, _mapper.Object);
		_applicationLogger = new Mock<ILogger<ApplicationController>>().Object;
		_mediator = new Mock<IMediator>();

		_applicationController = new(
			_applicationQueryService,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		_fixture.Customize<ApplicationContributorServiceModel>(composer =>
			composer
				.With(c => c.ContributorId, 0)
				.With(c => c.EmailAddress, () => _faker.Internet.Email())
				);

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, () => _faker.Internet.Email())
			);

		_fixture.Customize<ApplicationSchoolServiceModel>(composer =>
			composer
				.With(s => s.Id, 0)
				.With(s => s.SchoolConversionContactHeadEmail, _faker.Internet.Email())
				.With(s => s.SchoolConversionContactChairEmail, _faker.Internet.Email())
				.With(s => s.SchoolConversionMainContactOtherEmail, _faker.Internet.Email())
				.With(s => s.SchoolConversionApproverContactEmail, _faker.Internet.Email())
		);

		_fixture.Customize<LoanServiceModel>(composer =>
			composer
				.With(s => s.LoanId, 0));
		_fixture.Customize<LeaseServiceModel>(composer =>
			composer
				.With(s => s.LeaseId, 0));

		var applicationUpdateCommandHandler = new ApplicationUpdateCommandHandler(_repo);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationUpdateCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CommandResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await applicationUpdateCommandHandler.Handle((ApplicationUpdateCommand)cmd, ct);
			});
	}

	[Fact(Skip = "Flaky test that's too volatile to be reliable")]
	public async Task ParametersValid___ApplicationUpdated()
	{
		// arrange
		var existingApplication = await CreateExistingApplication();
		Assert.NotNull(existingApplication);
		Assert.Equal(3, existingApplication.Contributors.Count);
		Assert.Equal(3, existingApplication.Schools.Count);

		var applicationContributorServiceModel = _fixture.Create<ApplicationContributorServiceModel>();
		var applicationSchoolServiceModel = _fixture.Create<ApplicationSchoolServiceModel>();

		var applicationToUpdate = new ApplicationUpdateCommand(
			existingApplication.ApplicationId,
			existingApplication.ApplicationType,
			existingApplication.ApplicationStatus,
			new List<ApplicationContributorServiceModel>()
		{
			existingApplication.Contributors.ToArray()[0],
			existingApplication.Contributors.ToArray()[1],
			applicationContributorServiceModel
		},
			new List<ApplicationSchoolServiceModel> {
				existingApplication.Schools.ToArray()[0],
				applicationSchoolServiceModel,
				existingApplication.Schools.ToArray()[2] });

		// act
		var updateResult = await _applicationController.Update(existingApplication.ApplicationId, applicationToUpdate, default);

		// assert
		DfeAssert.OkResult(updateResult);
		var gotApplication = await _applicationQueryService.GetById(existingApplication.ApplicationId);
		Assert.NotNull(gotApplication);

		var expectedApplication = existingApplication with
		{
			Contributors = new List<ApplicationContributorServiceModel>
			{
				existingApplication.Contributors.ToArray()[0],
				existingApplication.Contributors.ToArray()[1],
				applicationContributorServiceModel with
				{
					ContributorId = gotApplication.Contributors.Single(c => c.EmailAddress == applicationContributorServiceModel.EmailAddress).ContributorId
				}
			},
			Schools = new List<ApplicationSchoolServiceModel>
			{
				existingApplication.Schools.ToArray()[0],
				applicationSchoolServiceModel with
				{
					Id = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Id,
					Loans = new List<LoanServiceModel>()
					{
						applicationSchoolServiceModel.Loans.ToArray()[0] with { LoanId = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Loans.ToArray()[0].LoanId},
						applicationSchoolServiceModel.Loans.ToArray()[1] with { LoanId = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Loans.ToArray()[1].LoanId},
						applicationSchoolServiceModel.Loans.ToArray()[2] with { LoanId = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Loans.ToArray()[2].LoanId}
					},
					Leases = new List<LeaseServiceModel>()
					{
						applicationSchoolServiceModel.Leases.ToArray()[0] with { LeaseId = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Leases.ToArray()[0].LeaseId},
						applicationSchoolServiceModel.Leases.ToArray()[1] with { LeaseId = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Leases.ToArray()[1].LeaseId},
						applicationSchoolServiceModel.Leases.ToArray()[2] with { LeaseId = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Leases.ToArray()[2].LeaseId}
					}
				},
				existingApplication.Schools.ToArray()[2]
			}
		};

		Assert.Equivalent(expectedApplication, gotApplication);
	}

	[Fact]
	public async Task StatusChangedToSubmittid___BadRequest_ApplicationNotUpdated()
	{
		// arrange
		var existingApplication = await CreateExistingApplication();
		Assert.NotNull(existingApplication);

		var applicationToUpdate = new ApplicationUpdateCommand(
			existingApplication.ApplicationId,
			existingApplication.ApplicationType,
			ApplicationStatus.Submitted,
			existingApplication.Contributors,
			existingApplication.Schools);

		// act
		var updateResult = await _applicationController.Update(existingApplication.ApplicationId, applicationToUpdate, default);

		// assert
		DfeAssert.BadRequestObjectResult(updateResult, "ApplicationStatus");
	}

	private async Task<ApplicationServiceModel?> CreateExistingApplication()
	{
		ApplicationCreateCommand applicationForCreate = new(ApplicationType.FormAMat, _fixture.Create<ContributorRequestModel>());// with { ApplicationType = ApplicationType.FormAMat } ;

		var createResult = await new ApplicationCreateCommandHandler(_applicationFactory, _repo, _mapper.Object).Handle(applicationForCreate, default);
		var createSuccessResult = Assert.IsType<CreateSuccessResult<ApplicationServiceModel>>(createResult);
		int id = createSuccessResult.Payload.ApplicationId;
		var schools = new List<ApplicationSchoolServiceModel>();
		schools.Add(_fixture.Create<ApplicationSchoolServiceModel>());
		schools.Add(_fixture.Create<ApplicationSchoolServiceModel>());
		schools.Add(_fixture.Create<ApplicationSchoolServiceModel>());

		var applicationToUpdate = _fixture.Create<ApplicationUpdateCommand>() with
		{
			ApplicationId = createSuccessResult.Payload.ApplicationId,
			ApplicationType = createSuccessResult.Payload.ApplicationType,
			ApplicationStatus = createSuccessResult.Payload.ApplicationStatus,
			Schools = schools
		};

		await _applicationController.Update(createSuccessResult.Payload.ApplicationId, applicationToUpdate, default);

		return await _applicationQueryService.GetById(id);
	}
}
