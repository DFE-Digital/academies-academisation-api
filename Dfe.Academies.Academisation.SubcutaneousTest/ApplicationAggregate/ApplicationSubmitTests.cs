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
using Dfe.Academies.Academisation.IService.Commands.Project;
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
		_applicationUpdateCommand = new ApplicationUpdateCommand(_applicationGetDataQuery, _applicationUpdateDataCommand);
		_applicationSubmitCommand = new ApplicationSubmitCommand(_applicationGetDataQuery, _applicationUpdateDataCommand,
			new ProjectFactory(), _projectCreateDataCommand);
		_applicationsListByUserQuery = new Mock<IApplicationListByUserQuery>().Object;

		_fixture.Customize<ContributorRequestModel>(composer =>
			composer.With(c => c.EmailAddress, _faker.Internet.Email()));

		_fixture.Customize<ApplicationSchoolServiceModel>(sd =>
			sd.With(s => s.SchoolConversionApproverContactEmail, _faker.Internet.Email())
			.With(s => s.Id, 0)
			.With(s => s.SchoolConversionContactChairEmail, _faker.Internet.Email())
			.With(s => s.SchoolConversionContactHeadEmail, _faker.Internet.Email())
			.With(s => s.SchoolConversionMainContactOtherEmail, _faker.Internet.Email()));
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

		var school = _fixture.Create<ApplicationSchoolServiceModel>();
		var updateRequest = createdPayload with
		{
			Schools = new List<ApplicationSchoolServiceModel> { school }
		};

		var updateResult = await applicationController.Update(updateRequest.ApplicationId, updateRequest);
		DfeAssert.OkResult(updateResult);

		// Act		
		var submissionResult = await applicationController.Submit(createdPayload.ApplicationId);

		// Assert
		DfeAssert.OkResult(submissionResult);

		var applicationGetResult = await applicationController.Get(createdPayload.ApplicationId);
		(_, var getPayload) = DfeAssert.OkObjectResult(applicationGetResult);

		Assert.Equal(ApplicationStatus.Submitted, getPayload.ApplicationStatus);

		var projectController = new LegacyProjectController(new LegacyProjectGetQuery(new ProjectGetDataQuery(_context)), Mock.Of<ILegacyProjectUpdateCommand>());
		var projectResult = await projectController.Get(1);

		(_, var project) = DfeAssert.OkObjectResult(projectResult);

		Assert.Multiple(
			() => Assert.Equal("Converter Pre-AO (C)", project.ProjectStatus),
			() => Assert.Equal(DateTime.Today.AddMonths(6), project.OpeningDate),
			() => Assert.Equal("Converter", project.AcademyTypeAndRoute),
			() => Assert.Equal(school.SchoolConversionTargetDate, project.ProposedAcademyOpeningDate),
			() => Assert.Equal(25000.0m, project.ConversionSupportGrantAmount),
			() => Assert.Equal(school.SchoolCapacityPublishedAdmissionsNumber.ToString(), project.PublishedAdmissionNumber),
			() => Assert.Equal(ToYesNoString(school.LandAndBuildings.PartOfPfiScheme), project.PartOfPfiScheme),
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
