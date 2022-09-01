using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
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

namespace Dfe.Academies.Academisation.SubcutaneousTest.ApplicationAggregate;

public class ApplicationUpdateTests
{
	private readonly Fixture _fixture = new();
	private readonly Faker _faker = new();

	private readonly IApplicationCreateCommand _applicationCreateCommand;
	private readonly IApplicationGetQuery _applicationGetQuery;
	private readonly IApplicationUpdateCommand _applicationUpdateCommand;
	private readonly IApplicationSubmitCommand _applicationSubmitCommand;
	private readonly IApplicationListByUserQuery _applicationsListByUserQuery;

	private readonly IApplicationFactory _applicationFactory = new ApplicationFactory();

	private readonly AcademisationContext _context;
	private readonly IApplicationCreateDataCommand _applicationCreateDataCommand;
	private readonly IApplicationGetDataQuery _applicationGetDataQuery;
	private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;

	public ApplicationUpdateTests()
	{
		_context = new TestApplicationContext().CreateContext();
		_applicationCreateDataCommand = new ApplicationCreateDataCommand(_context);
		_applicationGetDataQuery = new ApplicationGetDataQuery(_context);
		_applicationUpdateDataCommand = new ApplicationUpdateDataCommand(_context);

		_applicationCreateCommand = new ApplicationCreateCommand(_applicationFactory, _applicationCreateDataCommand);
		_applicationGetQuery = new ApplicationGetQuery(_applicationGetDataQuery);
		_applicationUpdateCommand = new ApplicationUpdateCommand(_applicationGetDataQuery, _applicationUpdateDataCommand);
		_applicationSubmitCommand = new Mock<IApplicationSubmitCommand>().Object;
		_applicationsListByUserQuery = new Mock<IApplicationListByUserQuery>().Object;

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Internet.Email()));

		_fixture.Customize<ApplicationSchoolServiceModel>(composer =>
			composer
				.With(s => s.SchoolConversionContactHeadEmail, _faker.Internet.Email())
				.With(s => s.SchoolConversionContactChairEmail, _faker.Internet.Email())
				.With(s => s.SchoolConversionMainContactOtherEmail, _faker.Internet.Email())
				.With(s => s.SchoolConversionApproverContactEmail, _faker.Internet.Email())
				);
	}

	[Fact]
	public async Task ParametersValid___ApplicationUpdated()
	{
		// arrange
		ApplicationController applicationController = new(
			_applicationCreateCommand,
			_applicationGetQuery,
			_applicationUpdateCommand,
			_applicationSubmitCommand,
			_applicationsListByUserQuery);

		var existingApplication = await CreateApplication();
		Assert.NotNull(existingApplication);

		var applicationContributorServiceModel = _fixture.Create<ApplicationContributorServiceModel>();
		var applicationSchoolServiceModel = _fixture.Create<ApplicationSchoolServiceModel>();

		ApplicationServiceModel applicationServiceModel = new(
			existingApplication.ApplicationId,
			existingApplication.ApplicationType,
			existingApplication.ApplicationStatus,
			existingApplication.Contributors,
		////existingApplication.Schools.Take(2).ToList());
		////new List<ApplicationContributorServiceModel>() { applicationContributorServiceModel },
		new List<ApplicationSchoolServiceModel>() { applicationSchoolServiceModel });

		// act
		var updateResult = await applicationController.Update(existingApplication.ApplicationId, applicationServiceModel);

		// assert
		DfeAssertions.OkResult(updateResult);

	}

	private async Task<ApplicationServiceModel> CreateApplication()
	{
		ApplicationCreateRequestModel applicationForCreate = _fixture.Create<ApplicationCreateRequestModel>();

		var createResult = await _applicationCreateCommand.Execute(applicationForCreate);

		var createSuccessResult = Assert.IsType<CreateSuccessResult<ApplicationServiceModel>>(createResult);

		int id = createSuccessResult.Payload.ApplicationId;

		return await _applicationGetQuery.Execute(id);
	}
}
