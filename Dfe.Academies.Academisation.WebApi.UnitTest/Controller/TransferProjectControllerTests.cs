using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class TransferProjectControllerTests
	{
		private readonly Mock<ITransferProjectQueryService> _mockTransferProjectQueryService;
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<ILogger<TransferProjectController>> _logger;
		private readonly TransferProjectController _controller;

		public TransferProjectControllerTests()
		{
			_mockTransferProjectQueryService = new Mock<ITransferProjectQueryService>();
			_mockMediator = new Mock<IMediator>();
			_logger = new Mock<ILogger<TransferProjectController>>();
			_controller = new TransferProjectController(_mockMediator.Object, _mockTransferProjectQueryService.Object, _logger.Object);
		}

		[Fact]
		public async Task GetByUrn_ShouldReturnNotFoundResponse()
		{
			int urn = 12345;
			_mockTransferProjectQueryService.Setup(tpqs => tpqs.GetByUrn(urn)).ReturnsAsync(AcademyTransferProjectResponseFactory.Create(null));

			var result = await _controller.GetByUrn(urn);

			Assert.Null(result.Value);
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetByUrn_ShouldReturnOKResponse()
		{
			var dummyTransferProject = AcademyTransferProjectResponseFactory.Create(TransferProject.Create(
				"dummyOutgoingTrustUkprn",
				"dummyIncomingTrustUkprn",
				new List<string> { "dummyUkprn1", "dummyUkprn2" },
				DateTime.Now
			));
			int urn = 12346;
			_mockTransferProjectQueryService.Setup(tpqs => tpqs.GetByUrn(urn)).ReturnsAsync(dummyTransferProject);

			var result = await _controller.GetByUrn(urn);

			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultResponse = Assert.IsAssignableFrom<AcademyTransferProjectResponse>(okObjectResult.Value);
			Assert.Equal(resultResponse, dummyTransferProject);
		}

		[Fact]
		public async Task GetTransferProjects_ShouldReturnOKResponse()
		{
			_mockTransferProjectQueryService
				.Setup(tpqs => tpqs.GetTransferProjects(0, 0, null, ""))
				.ReturnsAsync(new PagedResultResponse<AcademyTransferProjectSummaryResponse>(
					new List<AcademyTransferProjectSummaryResponse>() { new() { } }, 1));

			var result = await _controller.GetTransferProjects("", 0, 0, null);

			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultResponse = Assert.IsAssignableFrom<PagedResultResponse<AcademyTransferProjectSummaryResponse>>(okObjectResult.Value);
			Assert.Equal(1, resultResponse.TotalCount);
		}

		[Fact]
		public async Task GetTransferProjects_ShouldReturnOKResponse_WhenNoProjectsFound()
		{
			_mockTransferProjectQueryService
				.Setup(tpqs => tpqs.GetTransferProjects(0, 0, null, ""))
				.ReturnsAsync(new PagedResultResponse<AcademyTransferProjectSummaryResponse>(
					It.IsAny<IEnumerable<AcademyTransferProjectSummaryResponse>>(), 0));

			var result = await _controller.GetTransferProjects("", 0, 0, null);

			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultResponse = Assert.IsAssignableFrom<PagedResultResponse<AcademyTransferProjectSummaryResponse>>(okObjectResult.Value);
			Assert.Equal(0, resultResponse.TotalCount);
		}

		[Fact]
		public async Task AssignUser_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new AssignTransferProjectUserCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<AssignTransferProjectUserCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.AssignUser(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task AssignUser_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new AssignTransferProjectUserCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<AssignTransferProjectUserCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.AssignUser(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task AssignUser_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new AssignTransferProjectUserCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<AssignTransferProjectUserCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.AssignUser(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}
		[Fact]
		public async Task SetTransferProjectGeneralInformation_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferProjectGeneralInformationCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectGeneralInformationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetTransferProjectGeneralInformation(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectGeneralInformation_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferProjectGeneralInformationCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectGeneralInformationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetTransferProjectGeneralInformation(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectGeneralInformation_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferProjectGeneralInformationCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectGeneralInformationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetTransferProjectGeneralInformation(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetSchoolAdditionalData_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferringAcademySchoolAdditionalDataCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferringAcademySchoolAdditionalDataCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetSchoolAdditionalData(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetSchoolAdditionalData_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferringAcademySchoolAdditionalDataCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferringAcademySchoolAdditionalDataCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetSchoolAdditionalData(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetSchoolAdditionalData_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferringAcademySchoolAdditionalDataCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferringAcademySchoolAdditionalDataCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetSchoolAdditionalData(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetTransferProjectBenefits_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferProjectBenefitsCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectBenefitsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetTransferProjectBenefits(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectBenefits_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferProjectBenefitsCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectBenefitsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetTransferProjectBenefits(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectBenefits_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferProjectBenefitsCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectBenefitsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetTransferProjectBenefits(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetTransferProjectFeatures_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferProjectFeaturesCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectFeaturesCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetTransferProjectFeatures(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectFeatures_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferProjectFeaturesCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectFeaturesCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetTransferProjectFeatures(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectFeatures_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferProjectFeaturesCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectFeaturesCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetTransferProjectFeatures(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetTransferProjectStatus_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferProjectStatusCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectStatusCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetTransferProjectStatus(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectStatus_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferProjectStatusCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectStatusCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetTransferProjectStatus(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectStatus_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferProjectStatusCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectStatusCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetTransferProjectStatus(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetTransferProjectLegalRequirements_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferProjectLegalRequirementsCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectLegalRequirementsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetTransferProjectLegalRequirements(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectLegalRequirements_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferProjectLegalRequirementsCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectLegalRequirementsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetTransferProjectLegalRequirements(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectLegalRequirements_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferProjectLegalRequirementsCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectLegalRequirementsCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetTransferProjectLegalRequirements(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetTransferProjectTransferDates_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferProjectTransferDatesCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectTransferDatesCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetTransferProjectTransferDates(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectTransferDates_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferProjectTransferDatesCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectTransferDatesCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetTransferProjectTransferDates(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectTransferDates_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferProjectTransferDatesCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectTransferDatesCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetTransferProjectTransferDates(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task SetTransferProjectRationale_WhenCommandSucceeds_ReturnsOkResult()
		{
			// Arrange
			var command = new SetTransferProjectRationaleCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectRationaleCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// Act
			var result = await _controller.SetTransferProjectRationale(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectRationale_WhenCommandReturnsNotFound_ReturnsNotFoundResult()
		{
			// Arrange
			var command = new SetTransferProjectRationaleCommand();
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectRationaleCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// Act
			var result = await _controller.SetTransferProjectRationale(123, command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task SetTransferProjectRationale_WhenCommandReturnsValidationError_ReturnsBadRequestResultWithErrors()
		{
			// Arrange
			var command = new SetTransferProjectRationaleCommand();

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<SetTransferProjectRationaleCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(validationErrorResult);

			// Act
			var result = await _controller.SetTransferProjectRationale(123, command, CancellationToken.None);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(validationErrors, errors);
		}

		[Fact]
		public async Task CreateTransferProject_WhenCommandReturnsCreated_ReturnsCreatedAtRoute()
		{
			// Arrange
			var expectedResult = new AcademyTransferProjectResponse { ProjectUrn = "myTestProjectUrn" };

			var command = new CreateTransferProjectCommand("outgoingTrustUkprn", "incomingTrustUkprn", new List<string>() { "ukprn1", "ukprn2" });
			_mockMediator.Setup(x => x.Send(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CreateSuccessResult<AcademyTransferProjectResponse>(expectedResult));

			// Act
			var result = await _controller.CreateTransferProject(command, CancellationToken.None);

			// Assert
			var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
			Assert.Equal("GetByUrn", createdAtRouteResult.RouteName);
			Assert.Equal(expectedResult, createdAtRouteResult.Value);
		}

		[Fact]
		public async Task CreateTransferProject_WhenCommandReturnsNull_ReturnsBadRequest()
		{
			// Arrange
			var command = new CreateTransferProjectCommand("outgoingTrustUkprn", "incomingTrustUkprn", new List<string>() { "ukprn1", "ukprn2" });

			var validationErrors = new List<ValidationError>()
				{
					new("PropertyName", "ErrorMessage")
				};

			var validationErrorResult = new CommandValidationErrorResult(validationErrors);
			_mockMediator.Setup(x => x.Send(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((CreateSuccessResult<AcademyTransferProjectResponse>)null);

			// Act
			var result = await _controller.CreateTransferProject(command, CancellationToken.None);

			// Assert
			Assert.IsType<BadRequestResult>(result.Result);
		}
	}
}
