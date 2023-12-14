using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ApplicationControllerTests
	{
		private readonly Fixture _fixture = new();
		private readonly Mock<IApplicationQueryService> _getQueryMock = new();
		private readonly Mock<ILogger<ApplicationController>> _applicationLogger = new ();
		private readonly Mock<IMediator> _mockMediator = new();
		private readonly ApplicationController _subject;
		private readonly Mock<ITrustQueryService>_trustQueryService = new();

		public ApplicationControllerTests()
		{
			_subject = new ApplicationController(_getQueryMock.Object, _trustQueryService.Object, _mockMediator.Object, _applicationLogger.Object);
		}

		[Fact]
		public async Task Post___ServiceReturnsSuccess___SuccessResponseReturned()
		{
			// arrange
			var requestModel = _fixture.Create<ApplicationCreateCommand>();
			var applicationServiceModel = _fixture.Create<ApplicationServiceModel>();

			var cancelationToken = default(CancellationToken);
			_mockMediator.Setup(x => x.Send(requestModel, cancelationToken))
				.ReturnsAsync(new CreateSuccessResult<ApplicationServiceModel>(applicationServiceModel));

			// act
			var result = await _subject.Post(requestModel, cancelationToken);

			// assert
			var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
			Assert.Equal(applicationServiceModel, createdResult.Value);
			Assert.Equal("GetApplication", createdResult.RouteName);
			Assert.Equal(applicationServiceModel.ApplicationId, createdResult.RouteValues!["id"]);
		}

		[Fact]
		public async Task Post___ServiceReturnsValidationError___BadRequestResponseReturned()
		{
			// arrange
			var requestModel = _fixture.Create<ApplicationCreateCommand>();
			var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
			
			var cancelationToken = default(CancellationToken);
			_mockMediator.Setup(x => x.Send(requestModel, cancelationToken))
				.ReturnsAsync(new CreateValidationErrorResult(expectedValidationError));

			// act
			var result = await _subject.Post(requestModel, cancelationToken);

			// assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}

		[Fact]
		public async Task Submit___ServiceReturnsNotFound___NotFoundReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();

			_mockMediator.Setup(x => x.Send(It.Is<ApplicationSubmitCommand>(cmd => cmd.applicationId == applicationId), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new NotFoundCommandResult());

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			var badRequestResult = Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task Submit___ServiceReturnsCommandValidationError___BadRequestReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();

			List<ValidationError> expectedValidationError = new();

			_mockMediator.Setup(x => x.Send(It.Is<ApplicationSubmitCommand>(cmd => cmd.applicationId == applicationId), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandValidationErrorResult(expectedValidationError));

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}

		[Fact]
		public async Task Submit___ServiceReturnsCommandSuccess___SuccessResponseReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();

			_mockMediator.Setup(x => x.Send(It.Is<ApplicationSubmitCommand>(cmd => cmd.applicationId == applicationId), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandSuccessResult());

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			Assert.IsType<OkResult>(result);
		}
		
		[Fact]
		public async Task Submit___ServiceReturnsCreateValidationError___BadRequestReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();

			List<ValidationError> expectedValidationError = new();
			_mockMediator.Setup(x => x.Send(It.Is<ApplicationSubmitCommand>(cmd => cmd.applicationId == applicationId), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CreateValidationErrorResult(expectedValidationError));

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}

		[Fact]
		public async Task Submit___ServiceReturnsCreateSuccess___BadRequestReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();

			ConversionProjectServiceModel projectServiceModel = new ConversionProjectServiceModel(1, 1);
			_mockMediator.Setup(x => x.Send(It.Is<ApplicationSubmitCommand>(cmd => cmd.applicationId == applicationId), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CreateSuccessResult<ConversionProjectServiceModel>(projectServiceModel));

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
			Assert.Equal(projectServiceModel, createdAtRouteResult.Value);
		}

		[Fact]
		public async Task Get___ServiceReturnsNull___NotFoundReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();

			// act
			var result = await _subject.Get(applicationId);

			// assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task Get___ServiceReturnsApplication___SuccessResponseReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();
			var applicationServiceModel = _fixture.Create<ApplicationServiceModel>();

			_getQueryMock.Setup(x => x.GetById(applicationId)).ReturnsAsync(applicationServiceModel);

			// act
			var result = await _subject.Get(applicationId);

			// assert
			var getResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.Equal(applicationServiceModel, getResult.Value);
		}

		[Fact]
		public async Task Update___ServiceReturnsSuccess___SuccessResponseReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();
			var applicationServiceModel = _fixture.Create<ApplicationUpdateCommand>() with { ApplicationId = applicationId };

			_mockMediator.Setup(x => x.Send(It.Is<ApplicationUpdateCommand>(cmd => cmd == applicationServiceModel), It.IsAny<CancellationToken>()))
	.ReturnsAsync(new CommandSuccessResult());

			// act
			var result = await _subject.Update(applicationId, applicationServiceModel, default);

			// assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task IdsDoNotMatch___ValidationErrorResultReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();
			var applicationServiceModel = _fixture.Create<ApplicationUpdateCommand>() with { ApplicationId = applicationId + 1 };

			_mockMediator.Setup(x => x.Send(It.Is<ApplicationUpdateCommand>(cmd => cmd == applicationServiceModel), It.IsAny<CancellationToken>()))
	.ReturnsAsync(new CommandSuccessResult());

			// act
			var result = await _subject.Update(applicationId, applicationServiceModel, default);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			var errors = ((BadRequestObjectResult)result).Value as ReadOnlyCollection<ValidationError>;

			Assert.True(errors?[0].ErrorMessage == "Ids must be the same");
			Assert.True(errors?[0].PropertyName == "Id");
		}

		[Fact]
		public async Task Update___ServiceReturnsNotFound___NotFoundReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();
			var applicationServiceModel = _fixture.Create<ApplicationUpdateCommand>() with { ApplicationId = applicationId };

			_mockMediator.Setup(x => x.Send(It.Is<ApplicationUpdateCommand>(cmd => cmd == applicationServiceModel), It.IsAny<CancellationToken>()))
	.ReturnsAsync(new NotFoundCommandResult());

			// act
			var result = await _subject.Update(applicationId, applicationServiceModel, default);

			// assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task Update___ServiceReturnsValidationError___ValidationErrorReturned()
		{
			// arrange
			int applicationId = _fixture.Create<int>();
			var applicationServiceModel = _fixture.Create<ApplicationUpdateCommand>() with { ApplicationId = applicationId };

			var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
			_mockMediator.Setup(x => x.Send(It.Is<ApplicationUpdateCommand>(cmd => cmd == applicationServiceModel), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new CommandValidationErrorResult(expectedValidationError));

			// act
			var result = await _subject.Update(applicationId, applicationServiceModel, default);

			// assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestObjectResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}

		[Fact]
		public async Task ListByUser___ServiceReturnsEmptyList___SuccessResponseReturned()
		{
			// arrange
			string userEmail = _fixture.Create<string>();

			_getQueryMock.Setup(x => x.GetByUserEmail(userEmail))
				.ReturnsAsync(new List<ApplicationServiceModel>());

			// act
			var result = await _subject.ListByUser(userEmail);

			// assert
			var listResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.Equal(new List<ApplicationServiceModel>(), listResult.Value);
		}

		[Fact]
		public async Task ListByUser___ServiceReturnsPopulatedList___SuccessResponseReturned()
		{
			// arrange
			string userEmail = _fixture.Create<string>();
			var applications = new List<ApplicationServiceModel>
			{
				_fixture.Create<ApplicationServiceModel>(),
				_fixture.Create<ApplicationServiceModel>()
			};

			_getQueryMock.Setup(x => x.GetByUserEmail(userEmail))
				.ReturnsAsync(applications);

			// act
			var result = await _subject.ListByUser(userEmail);

			// assert
			var listResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.Equal(applications, listResult.Value);
		}

		[Fact]
		public async Task GetByApplicationReference_ReferenceExists_ReturnsOkWithApplication()
		{
			//Arrange
			var applicationReference = "A2B_001";
			_getQueryMock.Setup(x => x.GetByApplicationReference(applicationReference))
				.ReturnsAsync(_fixture.Build<ApplicationServiceModel>().With(x => x.ApplicationReference, applicationReference).Create());
			
			//Act
			var result = await _subject.Get(applicationReference);
			
			//Assert
			result.Should().NotBeNull();
			((result.Result as OkObjectResult).Value as ApplicationServiceModel).ApplicationReference.Should()
				.Be(applicationReference);
		}

		[Fact]
		public async Task GetByApplicationReference_ReferenceNotValid_Returns404()
		{
			//Arrange
			var applicationReference = "A2B_000";
			_getQueryMock.Setup(x => x.GetByApplicationReference(applicationReference))
				.ReturnsAsync((ApplicationServiceModel)null!);
			
			//Act
			var result = await _subject.Get(applicationReference);

			//Assert
			result.Result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task GetAllApplications__ReturnsList()
		{
			_getQueryMock.Setup(x => x.GetAllApplications())
				.ReturnsAsync(new List<ApplicationSchoolSharepointServiceModel>());

			var result = await _subject.GetAll();

			result.Result.Should().BeOfType<OkObjectResult>();
			(result.Result as OkObjectResult).Value.Should().NotBeNull();
		}
	}
}
