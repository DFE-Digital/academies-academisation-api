using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
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

	private readonly ApplicationController _applicationController;

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

		_applicationController = new(
			_applicationCreateCommand,
			_applicationGetQuery,
			_applicationUpdateCommand,
			_applicationSubmitCommand,
			_applicationsListByUserQuery);

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
	}

	[Fact]
	public async Task ParametersValid___ApplicationUpdated()
	{
		// arrange
		var existingApplication = await CreateExistingApplication();
		Assert.NotNull(existingApplication);
		Assert.Equal(3, existingApplication.Contributors.Count);
		Assert.Equal(3, existingApplication.Schools.Count);

		var applicationContributorServiceModel = _fixture.Create<ApplicationContributorServiceModel>();
		var applicationSchoolServiceModel = _fixture.Create<ApplicationSchoolServiceModel>();

		var schoolsList = existingApplication.Schools.ToList();
		schoolsList.Add(applicationSchoolServiceModel);

		ApplicationServiceModel applicationToUpdate = new(
			existingApplication.ApplicationId,
			existingApplication.ApplicationType,
			existingApplication.ApplicationStatus,
			new List<ApplicationContributorServiceModel>()
			{
				existingApplication.Contributors.ToArray()[0],
				existingApplication.Contributors.ToArray()[1],
				applicationContributorServiceModel
			},
			new List<ApplicationSchoolServiceModel>()
			{
				existingApplication.Schools.ToArray()[0],
				applicationSchoolServiceModel,
				existingApplication.Schools.ToArray()[2]
			}
			);

		// act
		var updateResult = await _applicationController.Update(existingApplication.ApplicationId, applicationToUpdate);

		// assert
		DfeAssertions.OkResult(updateResult);
		var gotApplication = await _applicationGetQuery.Execute(existingApplication.ApplicationId);
		Assert.NotNull(gotApplication);

		var expectedApplication = applicationToUpdate with
		{
			Contributors = new List<ApplicationContributorServiceModel>()
			{
				existingApplication.Contributors.ToArray()[0],
				existingApplication.Contributors.ToArray()[1],
				applicationContributorServiceModel with
				{
					ContributorId = gotApplication.Contributors.Single(c => c.EmailAddress == applicationContributorServiceModel.EmailAddress).ContributorId
				}
			},
			Schools = new List<ApplicationSchoolServiceModel>()
			{
				existingApplication.Schools.ToArray()[0],
				applicationSchoolServiceModel with
				{
					Id = gotApplication.Schools.Single(s => s.Urn == applicationSchoolServiceModel.Urn).Id
				},
				existingApplication.Schools.ToArray()[2],
			}
		};

		Assert.Equivalent(expectedApplication, gotApplication);

	}

	private async Task<ApplicationServiceModel> CreateExistingApplication()
	{
		ApplicationCreateRequestModel applicationForCreate = _fixture.Create<ApplicationCreateRequestModel>();

		var createResult = await _applicationCreateCommand.Execute(applicationForCreate);
		var createSuccessResult = Assert.IsType<CreateSuccessResult<ApplicationServiceModel>>(createResult);
		int id = createSuccessResult.Payload.ApplicationId;

		var applicationToUpdate = _fixture.Create<ApplicationServiceModel>() with
		{
			ApplicationId = createSuccessResult.Payload.ApplicationId,
			ApplicationType = createSuccessResult.Payload.ApplicationType,
			ApplicationStatus = createSuccessResult.Payload.ApplicationStatus
		};

		await _applicationController.Update(createSuccessResult.Payload.ApplicationId, applicationToUpdate);

		return await _applicationGetQuery.Execute(id);
	}
}
