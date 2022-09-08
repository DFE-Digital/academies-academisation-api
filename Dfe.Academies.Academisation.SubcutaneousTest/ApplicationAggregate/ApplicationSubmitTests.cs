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
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Moq;


namespace Dfe.Academies.Academisation.SubcutaneousTest.ApplicationAggregate;

public class ApplicationSubmitTests
{
	private readonly Fixture _fixture = new();
	private readonly Faker _faker = new();
	private readonly AcademisationContext _context;

	private readonly IApplicationCreateCommand _applicationCreateCommand;
	private readonly IApplicationGetQuery _applicationGetQuery;
	private readonly IApplicationUpdateCommand _applicationUpdateCommand;
	private readonly IApplicationSubmitCommand _applicationSubmitCommand;
	private readonly IApplicationListByUserQuery _applicationsListByUserQuery;

	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();

	private readonly IApplicationCreateDataCommand _applicationCreateDataCommand;
	private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;
	private readonly IApplicationGetDataQuery _applicationGetDataQuery;
	private readonly IProjectCreateDataCommand _projectCreateDataCommand;

	public ApplicationSubmitTests()
	{
		_context = new TestApplicationContext().CreateContext();

		_applicationCreateDataCommand = new ApplicationCreateDataCommand(_context);
		_applicationGetDataQuery = new ApplicationGetDataQuery(_context);

		_applicationCreateCommand = new ApplicationCreateCommand(_applicationFactory, _applicationCreateDataCommand);
		_applicationUpdateDataCommand = new ApplicationUpdateDataCommand(_context);
		_applicationGetQuery = new ApplicationGetQuery(_applicationGetDataQuery);

		_projectCreateDataCommand = new ProjectCreateDataCommand(_context);
		_applicationUpdateCommand = new Mock<IApplicationUpdateCommand>().Object;
		_applicationSubmitCommand = new ApplicationSubmitCommand(_applicationGetDataQuery, _applicationUpdateDataCommand, 
			new ProjectFactory(), _projectCreateDataCommand);
		_applicationsListByUserQuery = new Mock<IApplicationListByUserQuery>().Object;

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Internet.Email()));
	}


	[Fact]
	public async Task JoinAMatApplicationExists___ApplicationIsSubmitted_And_ProjectIsCreated()
	{
		// Arrange
		var applicationController = new ApplicationController(
			_applicationCreateCommand,
			_applicationGetQuery,
			_applicationUpdateCommand,
			_applicationSubmitCommand,
			_applicationsListByUserQuery);

		ApplicationCreateRequestModel applicationCreateRequestModel = _fixture
			.Create<ApplicationCreateRequestModel>();
		
		var createResult = await applicationController.Post(applicationCreateRequestModel);

		(_, var createdPayload) = DfeAssert.CreatedAtRoute(createResult, "GetApplication");

		// Act		
		var submissionResult = await applicationController.Submit(createdPayload.ApplicationId);

		// Assert
		DfeAssert.OkResult(submissionResult);
		var applicationGetResult = await applicationController.Get(1);

		(_, var getPayload) = DfeAssert.OkObjectResult<ApplicationServiceModel>(applicationGetResult!.Result);

		Assert.Equal(ApplicationStatus.Submitted, getPayload.ApplicationStatus);
	}
}
