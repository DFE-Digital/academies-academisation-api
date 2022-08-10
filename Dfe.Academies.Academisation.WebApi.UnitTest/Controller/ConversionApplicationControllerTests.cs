using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ApplicationControllerTests
	{
		private readonly Fixture fixture = new();
		private readonly Mock<IApplicationCreateCommand> _createCommandMock = new();
		private readonly Mock<IApplicationGetQuery> _getQueryMock = new();
		private readonly Mock<IApplicationSubmitCommand> _submitCommandMock = new();
		private readonly Mock<IApplicationUpdateCommand> _updateCommandMock = new();
		private readonly ApplicationController _subject;

		public ApplicationControllerTests()
		{
			_subject = new ApplicationController(_createCommandMock.Object, _getQueryMock.Object, _updateCommandMock.Object, _submitCommandMock.Object);
		}

		[Fact]
		public async Task Post___ServiceReturnsSuccess___SuccessResponseReturned()
		{
			// arrange
			var requestModel = fixture.Create<ApplicationCreateRequestModel>();

			var applicationServiceModel = fixture.Create<ApplicationServiceModel>();
			_createCommandMock.Setup(x => x.Execute(requestModel))
				.ReturnsAsync(new CreateSuccessResult<ApplicationServiceModel>(applicationServiceModel));

			// act
			var result = await _subject.Post(requestModel);

			// assert
			var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
			Assert.Equal(applicationServiceModel, createdResult.Value);
			Assert.Equal("Get", createdResult.RouteName);
			Assert.Equal(applicationServiceModel.ApplicationId, createdResult.RouteValues!["id"]);
		}

		[Fact]
		public async Task Post___ServiceReturnsValidationErrorResult___BadRequestResponseReturned()
		{
			// arrange
			var requestModel = fixture.Create<ApplicationCreateRequestModel>();
			var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
			_createCommandMock.Setup(x => x.Execute(requestModel))
				.ReturnsAsync(new CreateValidationErrorResult<ApplicationServiceModel>(expectedValidationError));

			// act
			var result = await _subject.Post(requestModel);

			// assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}

		[Fact]
		public async Task Submit___ServiceReturnsSuccess___SuccessResponseReturned()
		{
			// arrange
			int applicationId = fixture.Create<int>();

			_submitCommandMock.Setup(x => x.Execute(applicationId)).ReturnsAsync(new CommandSuccessResult());

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			var createdResult = Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task Submit___ServiceReturnsNotFound___NotFoundReturned()
		{
			// arrange
			int applicationId = fixture.Create<int>();

			_submitCommandMock.Setup(x => x.Execute(applicationId)).ReturnsAsync(new NotFoundCommandResult());

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			var badRequestResult = Assert.IsType<NotFoundResult>(result);
		}


		[Fact]
		public async Task Submit___ServiceReturnsValidationError___BadRequestReturned()
		{
			// arrange
			int applicationId = fixture.Create<int>();

			var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
			_submitCommandMock.Setup(x => x.Execute(applicationId)).ReturnsAsync(new CommandValidationErrorResult(expectedValidationError));

			// act
			var result = await _subject.Submit(applicationId);

			// assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}

		[Fact]
		public async Task Get___QueryReturnsNull___NotFoundReturned()
		{
			// arrange
			int applicationId = fixture.Create<int>();

			// act
			var result = await _subject.Get(applicationId);

			// assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task Get____ServiceReturnsApplication___SuccessResponseReturned()
		{
			// arrange
			int applicationId = fixture.Create<int>();
			var applicationServiceModel = fixture.Create<ApplicationServiceModel>();

			_getQueryMock.Setup(x => x.Execute(applicationId)).ReturnsAsync(applicationServiceModel);

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
			int applicationId = fixture.Create<int>();
			var applicationServiceModel = fixture.Create<ApplicationServiceModel>();

			_updateCommandMock.Setup(x => x.Execute(applicationId, applicationServiceModel))
				.ReturnsAsync(new CommandSuccessResult());

			// act
			var result = await _subject.Update(applicationId, applicationServiceModel);

			// assert
			Assert.IsType<OkResult>(result.Result);
		}

		[Fact]
		public async Task Update___ServiceReturnsNotFound___NotFoundReturned()
		{
			// arrange
			int applicationId = fixture.Create<int>();
			var applicationServiceModel = fixture.Create<ApplicationServiceModel>();

			_updateCommandMock.Setup(x => x.Execute(applicationId, applicationServiceModel))
				.ReturnsAsync(new NotFoundCommandResult());

			// act
			var result = await _subject.Update(applicationId, applicationServiceModel);

			// assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task Update___ServiceReturnsValidationError___ValidationErrorReturned()
		{
			// arrange
			int applicationId = fixture.Create<int>();
			var applicationServiceModel = fixture.Create<ApplicationServiceModel>();

			_updateCommandMock.Setup(x => x.Execute(applicationId, applicationServiceModel))
				.ReturnsAsync(new CommandValidationErrorResult(new List<ValidationError>()));

			// act
			var result = await _subject.Update(applicationId, applicationServiceModel);

			// assert
			Assert.IsType<BadRequestResult>(result.Result);
		}
	}
}
