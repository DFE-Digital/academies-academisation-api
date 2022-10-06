using AutoFixture;
using AutoMapper;
using Bogus;
using Castle.Core.Logging;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ApplicationAggregate;

public class ApplicationCreateTests
{
	private readonly Fixture _fixture = new();
	private readonly Faker _faker = new();

	private readonly IApplicationCreateCommand _applicationCreateCommand;
	private readonly IApplicationGetQuery _applicationGetQuery;
	private readonly IApplicationUpdateCommand _applicationUpdateCommand;
	private readonly IApplicationSubmitCommand _applicationSubmitCommand;
	private readonly IApplicationListByUserQuery _applicationsListByUserQuery;
	private readonly ILogger<ApplicationController> _applicationLogger;

	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();

	private readonly AcademisationContext _context;
	private readonly IApplicationCreateDataCommand _applicationCreateDataCommand;
	private readonly IApplicationGetDataQuery _applicationGetDataQuery;
	private readonly ISetJoinTrustDetailsCommandHandler _setTrustCommandHandler;
	private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
	public ApplicationCreateTests()
	{
		_context = new TestApplicationContext().CreateContext();
		_applicationCreateDataCommand = new ApplicationCreateDataCommand(_context, _mapper.Object);
		_applicationGetDataQuery = new ApplicationGetDataQuery(_context, _mapper.Object);

		_applicationCreateCommand = new ApplicationCreateCommand(_applicationFactory, _applicationCreateDataCommand, _mapper.Object);
		_applicationGetQuery = new ApplicationGetQuery(_applicationGetDataQuery, _mapper.Object);

		_applicationUpdateCommand = new Mock<IApplicationUpdateCommand>().Object;
		_applicationSubmitCommand = new Mock<IApplicationSubmitCommand>().Object;
		_applicationsListByUserQuery = new Mock<IApplicationListByUserQuery>().Object;
		_applicationLogger = new Mock<ILogger<ApplicationController>>().Object;
		_setTrustCommandHandler = new Mock<ISetJoinTrustDetailsCommandHandler>().Object;

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Internet.Email()));
	}

	[Fact]
	public async Task ParametersValid___ApplicationCreated()
	{
		// arrange
		var applicationController = new ApplicationController(
			_applicationCreateCommand,
			_applicationGetQuery,
			_applicationUpdateCommand,
			_applicationSubmitCommand, 
			_setTrustCommandHandler,
			_applicationsListByUserQuery,
			_applicationLogger);

		ApplicationCreateRequestModel applicationCreateRequestModel = _fixture
			.Create<ApplicationCreateRequestModel>();

		// act
		var result = await applicationController.Post(applicationCreateRequestModel);

		// assert
		(CreatedAtRouteResult createdAtRouteResult, _) = DfeAssert.CreatedAtRoute(result, "GetApplication");

		object? idValue = createdAtRouteResult.RouteValues!["id"];
		int id = Assert.IsType<int>(idValue);

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
			null, null);

		Assert.Equivalent(expectedApplication, actualApplication);
	}

	[Fact]
	public async Task ParametersInvalid___ApplicationNotCreated()
	{
		// arrange
		var applicationController = new ApplicationController(
			_applicationCreateCommand,
			_applicationGetQuery,
			_applicationUpdateCommand,
			_applicationSubmitCommand,
			_setTrustCommandHandler,
			_applicationsListByUserQuery,
			_applicationLogger);

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Name.FullName()));

		ApplicationCreateRequestModel applicationCreateRequestModel = _fixture
			.Create<ApplicationCreateRequestModel>();

		// act
		var result = await applicationController.Post(applicationCreateRequestModel);

		// assert
		Assert.IsType<BadRequestObjectResult>(result.Result);
		Assert.Equal(3, _context.Applications.Count());
	}
}
