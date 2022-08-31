using System;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Commands;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.ApplicationAggregate;

public class ApplicationCreateTests
{
	private readonly Fixture _fixture = new();

	private readonly IApplicationCreateCommand _applicationCreateCommand;
	private readonly IApplicationGetQuery _applicationGetQuery;
	private readonly IApplicationUpdateCommand _applicationUpdateCommand;
	private readonly IApplicationSubmitCommand _applicationSubmitCommand;
	private readonly IApplicationListByUserQuery _applicationsListByUserQuery;

	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();

	private readonly AcademisationContext _context;
	private readonly IApplicationCreateDataCommand _applicationCreateDataCommand;
	private readonly IApplicationGetDataQuery _applicationGetDataQuery;

	public ApplicationCreateTests()
	{
		_context = new TestApplicationContext().CreateContext();
		_applicationCreateDataCommand = new ApplicationCreateDataCommand(_context);
		_applicationGetDataQuery = new ApplicationGetDataQuery(_context);

		_applicationCreateCommand = new ApplicationCreateCommand(_applicationFactory, _applicationCreateDataCommand);
		_applicationGetQuery = new ApplicationGetQuery(_applicationGetDataQuery);

		_applicationUpdateCommand = new Mock<IApplicationUpdateCommand>().Object;
		_applicationSubmitCommand = new Mock<IApplicationSubmitCommand>().Object;
		_applicationsListByUserQuery = new Mock<IApplicationListByUserQuery>().Object;
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
			_applicationsListByUserQuery);

		ApplicationCreateRequestModel applicationCreateRequestModel = _fixture.Create<ApplicationCreateRequestModel>();

		// act
		var result = await applicationController.Post(applicationCreateRequestModel);

		// assert
		var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
		Assert.IsType<ApplicationServiceModel>(createdResult.Value);
		//Assert.Equal("Get", createdResult.RouteName);
		//Assert.Equal(applicationServiceModel.ApplicationId, createdResult.RouteValues!["id"]);
	}
}
