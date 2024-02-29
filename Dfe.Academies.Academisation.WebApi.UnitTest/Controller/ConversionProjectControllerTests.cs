using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.Service.Commands.FormAMat;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using Moq;
using Xunit;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Data.ProjectAggregate;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ConversionProjectControllerTests
	{
		private readonly Mock<IConversionProjectQueryService> _mockConversionProjectQueryService;
		private readonly Mock<IMediator> _mockMediator;
		private readonly ConversionProjectController _controller;

		public ConversionProjectControllerTests()
		{
			_mockConversionProjectQueryService = new Mock<IConversionProjectQueryService>();
			_mockMediator = new Mock<IMediator>();
			_controller = new ConversionProjectController(_mockConversionProjectQueryService.Object, _mockMediator.Object);
		}

		private SetSchoolOverviewCommand CreateValidSetSchoolOverviewCommand()
		{
			return new SetSchoolOverviewCommand(
				id: 1,
				publishedAdmissionNumber: "100",
				viabilityIssues: "No issues",
				partOfPfiScheme: "No",
				financialDeficit: "None",
				numberOfPlacesFundedFor: 150m,
				numberOfResidentialPlaces: 10m,
				numberOfFundedResidentialPlaces: 5m,
				pfiSchemeDetails: "N/A",
				distanceFromSchoolToTrustHeadquarters: 20m,
				distanceFromSchoolToTrustHeadquartersAdditionalInformation: "Within city limits",
				memberOfParliamentNameAndParty: "Jane Doe - PartyName",
				pupilsAttendingGroupPermanentlyExcluded: true,
				pupilsAttendingGroupMedicalAndHealthNeeds: true,
				pupilsAttendingGroupTeenageMums: true
			);
		}

		[Fact]
		public async Task SetSchoolOverview_ReturnsOk_WhenUpdateIsSuccessful()
		{
			var request = CreateValidSetSchoolOverviewCommand();
			_mockMediator.Setup(m => m.Send(It.IsAny<SetSchoolOverviewCommand>(), default))
						 .ReturnsAsync(new CommandSuccessResult());

			var result = await _controller.SetSchoolOverview(request.Id, request);

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetSchoolOverview_ReturnsNotFound_WhenProjectNotFound()
		{
			var request = CreateValidSetSchoolOverviewCommand();
			_mockMediator.Setup(m => m.Send(It.IsAny<SetSchoolOverviewCommand>(), default))
						 .ReturnsAsync(new NotFoundCommandResult());

			var result = await _controller.SetSchoolOverview(request.Id, request);

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetSchoolOverview_ReturnsBadRequestFound_WhenCommandValidationFails()
		{
			var request = CreateValidSetSchoolOverviewCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);

			_mockMediator.Setup(m => m.Send(It.IsAny<SetSchoolOverviewCommand>(), default))
						 .ReturnsAsync(validationErrorResult);

			var result = await _controller.SetSchoolOverview(request.Id, request);

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task GetFormAMatProjectById_ReturnsOk_WhenProjectIsFound()
		{
			_mockConversionProjectQueryService
				.Setup(m => m.GetFormAMatProjectById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new FormAMatProjectServiceModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()));

			var result = await _controller.GetFormAMatProjectById(1, CancellationToken.None);

			Assert.IsType<OkObjectResult>(result.Result);
		}

		[Fact]
		public async Task GetFormAMatProjectById_ReturnsNotFound_WhenProjectIsNotFound()
		{
			_mockConversionProjectQueryService
				.Setup(m => m.GetFormAMatProjectById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((FormAMatProjectServiceModel?)null);

			var result = await _controller.GetFormAMatProjectById(1, CancellationToken.None);

			Assert.IsType<NotFoundObjectResult>(result.Result);
		}

		[Fact]
		public async Task SetExternalApplicationForm_ReturnsOk_WhenUpdateIsSuccessful()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetExternalApplicationFormCommand>(), default))
					 .ReturnsAsync(new CommandSuccessResult());

			var result = await _controller.SetExternalApplicationForm(1, new SetExternalApplicationFormCommand(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()));

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetExternalApplicationForm_ReturnsNotFound_WhenProjectNotFound()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetExternalApplicationFormCommand>(), default))
						 .ReturnsAsync(new NotFoundCommandResult());

			var result = await _controller.SetExternalApplicationForm(1, new SetExternalApplicationFormCommand(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()));

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetExternalApplicationForm_ReturnsBadRequestFound_WhenCommandValidationFails()
		{
			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);

			_mockMediator.Setup(m => m.Send(It.IsAny<SetExternalApplicationFormCommand>(), default))
						 .ReturnsAsync(validationErrorResult);

			var result = await _controller.SetExternalApplicationForm(1, new SetExternalApplicationFormCommand(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()));

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetFormAMatAssignedUser_ReturnsOk_WhenUpdateIsSuccessful()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetFormAMatAssignedUserCommand>(), default))
					 .ReturnsAsync(new CommandSuccessResult());

			var result = await _controller.SetFormAMatAssignedUser(1, new SetFormAMatAssignedUserCommand(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetFormAMatAssignedUser_ReturnsNotFound_WhenProjectNotFound()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetFormAMatAssignedUserCommand>(), default))
						 .ReturnsAsync(new NotFoundCommandResult());

			var result = await _controller.SetFormAMatAssignedUser(1, new SetFormAMatAssignedUserCommand(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetFormAMatAssignedUser_ReturnsBadRequestFound_WhenCommandValidationFails()
		{
			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);

			_mockMediator.Setup(m => m.Send(It.IsAny<SetFormAMatAssignedUserCommand>(), default))
						 .ReturnsAsync(validationErrorResult);

			var result = await _controller.SetFormAMatAssignedUser(1, new SetFormAMatAssignedUserCommand(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetAssignedUser_ReturnsOk_WhenUpdateIsSuccessful()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetAssignedUserCommand>(), default))
					 .ReturnsAsync(new CommandSuccessResult());

			var result = await _controller.SetAssignedUser(1, new SetAssignedUserCommand(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetAssignedUser_ReturnsNotFound_WhenProjectNotFound()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetAssignedUserCommand>(), default))
						 .ReturnsAsync(new NotFoundCommandResult());

			var result = await _controller.SetAssignedUser(1, new SetAssignedUserCommand(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetAssignedUser_ReturnsBadRequestFound_WhenCommandValidationFails()
		{
			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);

			_mockMediator.Setup(m => m.Send(It.IsAny<SetAssignedUserCommand>(), default))
						 .ReturnsAsync(validationErrorResult);

			var result = await _controller.SetAssignedUser(1, new SetAssignedUserCommand(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetPerformanceData_ReturnsOk_WhenUpdateIsSuccessful()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetPerformanceDataCommand>(), default))
					 .ReturnsAsync(new CommandSuccessResult());

			var result = await _controller.SetPerformanceData(1, new SetPerformanceDataCommand(It.IsAny<int>(), null, null, null, null));

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetPerformanceData_ReturnsNotFound_WhenProjectNotFound()
		{
			_mockMediator.Setup(m => m.Send(It.IsAny<SetPerformanceDataCommand>(), default))
						 .ReturnsAsync(new NotFoundCommandResult());

			var result = await _controller.SetPerformanceData(1, new SetPerformanceDataCommand(It.IsAny<int>(), null, null, null, null));

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetPerformanceData_ReturnsBadRequestFound_WhenCommandValidationFails()
		{
			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);

			_mockMediator.Setup(m => m.Send(It.IsAny<SetPerformanceDataCommand>(), default))
						 .ReturnsAsync(validationErrorResult);

			var result = await _controller.SetPerformanceData(1, new SetPerformanceDataCommand(It.IsAny<int>(), null, null, null, null));

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task GetFormAMatProjects_ReturnsOk_WhenProjectsFound()
		{
			var formAMatProjects = new List<FormAMatProjectServiceModel>() {
				new (It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>())
			};
			var dummyResponse = new PagedDataResponse<FormAMatProjectServiceModel>(formAMatProjects, new PagingResponse());
			var searchModel = new ConversionProjectSearchModel(1, 1, null, null, null, null, null, null);

			_mockConversionProjectQueryService.Setup(m => m.GetFormAMatProjects(null, null, null, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), null, null, null))
					 .ReturnsAsync(dummyResponse);

			var result = await _controller.GetFormAMatProjects(searchModel, CancellationToken.None);

			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultResponse = Assert.IsAssignableFrom<PagedDataResponse<FormAMatProjectServiceModel>>(okObjectResult.Value);
			Assert.Equal(resultResponse, dummyResponse);
		}

		[Fact]
		public async Task GetFormAMatProjects_ReturnsNotFound_WhenProjectsNotFound()
		{
			var searchModel = new ConversionProjectSearchModel(1, 1, null, null, null, null, null, null);
			_mockConversionProjectQueryService.Setup(m => m.GetFormAMatProjects(null, null, null, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), null, null, null))
					 .ReturnsAsync((PagedDataResponse<FormAMatProjectServiceModel>?)null);

			var result = await _controller.GetFormAMatProjects(searchModel, CancellationToken.None);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetProjects_ReturnsOk_WhenProjectsFound()
		{
			var projects = new List<ConversionProjectServiceModel>() {
				new (It.IsAny<int>(), It.IsAny<int>())
			};
			var dummyResponse = new PagedDataResponse<ConversionProjectServiceModel>(projects, new PagingResponse());
			var searchModel = new ConversionProjectSearchModel(1, 1, null, null, null, null, null, null);

			_mockConversionProjectQueryService.Setup(m => m.GetProjectsV2(null, null, null, It.IsAny<int>(), It.IsAny<int>(), null, null, null))
					 .ReturnsAsync(dummyResponse);

			var result = await _controller.GetProjects(searchModel);

			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultResponse = Assert.IsAssignableFrom<PagedDataResponse<ConversionProjectServiceModel>>(okObjectResult.Value);
			Assert.Equal(resultResponse, dummyResponse);
		}

		[Fact]
		public async Task GetProjects_ReturnsNotFound_WhenProjectsNotFound()
		{
			var searchModel = new ConversionProjectSearchModel(1, 1, null, null, null, null, null, null);
			_mockConversionProjectQueryService.Setup(m => m.GetProjectsV2(null, null, null, It.IsAny<int>(), It.IsAny<int>(), null, null, null))
					 .ReturnsAsync((PagedDataResponse<ConversionProjectServiceModel>?)null);

			var result = await _controller.GetProjects(searchModel);

			Assert.IsType<NotFoundResult>(result.Result);
		}
	}
}
