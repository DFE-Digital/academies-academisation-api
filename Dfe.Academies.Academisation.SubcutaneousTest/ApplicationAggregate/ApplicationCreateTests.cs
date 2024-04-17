﻿using AutoFixture;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ApplicationAggregate;

public class ApplicationCreateTests
{
	private readonly Fixture _fixture = new();
	private readonly Faker _faker = new();

	private readonly IApplicationQueryService _applicationQueryService;
	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();
	private readonly ILogger<ApplicationController> _applicationLogger;
	private readonly Mock<IMediator> _mediator = new Mock<IMediator>();

	private readonly AcademisationContext _context;
	private readonly IApplicationRepository _repo;
	private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
	private readonly ITrustQueryService _trustQueryService;

	public ApplicationCreateTests()
	{
		_context = new TestApplicationContext().CreateContext();
		_repo = new ApplicationRepository(_context, _mapper.Object);

		_applicationQueryService = new ApplicationQueryService(_repo, _mapper.Object);
		_trustQueryService = new TrustQueryService(_context, _mapper.Object);
		_applicationLogger = new Mock<ILogger<ApplicationController>>().Object;
		

		var applicationCreateCommandHandler = new ApplicationCreateCommandHandler(_applicationFactory, _repo, _mapper.Object);

		_mediator.Setup(x => x.Send(It.IsAny<ApplicationCreateCommand>(), It.IsAny<CancellationToken>()))
			.Returns<IRequest<CreateResult>, CancellationToken>(async (cmd, ct) =>
			{

				return await applicationCreateCommandHandler.Handle((ApplicationCreateCommand)cmd, ct);
			});

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Internet.Email()));
	}

	[Fact]
	public async Task ParametersValid___ApplicationCreated()
	{
		// arrange
		var applicationController = new ApplicationController(
			_applicationQueryService,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		ApplicationCreateCommand applicationCreateRequestModel = _fixture
			.Create<ApplicationCreateCommand>();

		// act
		var result = await applicationController.Post(applicationCreateRequestModel, default);

		// assert
		(CreatedAtRouteResult createdAtRouteResult, _) = DfeAssert.CreatedAtRoute(result, "GetApplication");

		object? idValue = createdAtRouteResult.RouteValues!["id"];
		int id = Assert.IsType<int>(idValue);
		var applicationReference = $"A2B_{id}";
		var getResult = await applicationController.Get(id);

		var getOkayResult = Assert.IsAssignableFrom<OkObjectResult>(getResult.Result);
		var actualApplication = Assert.IsAssignableFrom<ApplicationServiceModel>(getOkayResult.Value);
		var actualContributor = Assert.Single(actualApplication.Contributors);

		ApplicationServiceModel expectedApplication = new(
			id,
			applicationCreateRequestModel.ApplicationType,
			ApplicationStatus.InProgress,
			new List<ApplicationContributorServiceModel> { new ApplicationContributorServiceModel(
				actualContributor.ContributorId,
				applicationCreateRequestModel.Contributor.FirstName,
				applicationCreateRequestModel.Contributor.LastName,
				applicationCreateRequestModel.Contributor.EmailAddress,
				applicationCreateRequestModel.Contributor.Role,
				applicationCreateRequestModel.Contributor.OtherRoleName) },
			new List<ApplicationSchoolServiceModel>(),
			null, null, null, id.ToString(), actualApplication.EntityId,null);

		Assert.Equivalent(expectedApplication, actualApplication);
	}

	[Fact]
	public async Task ParametersInvalid___ApplicationNotCreated()
	{
		// arrange
		var applicationController = new ApplicationController(
			_applicationQueryService,
			_trustQueryService,
			_mediator.Object,
			_applicationLogger);

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Name.FullName()));

		ApplicationCreateCommand applicationCreateRequestModel = _fixture
			.Create<ApplicationCreateCommand>();

		// act
		var result = await applicationController.Post(applicationCreateRequestModel, default);

		// assert
		Assert.IsType<BadRequestObjectResult>(result.Result);
		Assert.Equal(3, _context.Applications.Count());
	}
}
