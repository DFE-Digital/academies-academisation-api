using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService;
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
	public class ConversionApplicationControllerTests
	{
		private readonly Fixture fixture = new();

		[Fact]
		public async Task Post___ServiceReturnsSuccess___SuccessResponseReturned()
		{
			// arrange
			var mockCreateCommand = new Mock<IApplicationCreateCommand>();
			var mockGetQuery = new Mock<IApplicationGetQuery>();

			var applicationServiceModel = fixture.Create<ApplicationServiceModel>();

			mockCreateCommand.Setup(x => x.Execute(It.IsAny<ApplicationCreateRequestModel>()))
				.ReturnsAsync(new CreateSuccessResult<ApplicationServiceModel>(applicationServiceModel));

			var subject = new ConversionApplicationController(mockCreateCommand.Object, mockGetQuery.Object);

			var requestModel = fixture.Create<ApplicationCreateRequestModel>();

			// act
			var result = await subject.Post(requestModel);

			// assert
			Assert.IsType<CreatedAtRouteResult>(result.Result);

			var thing = subject.RouteData;

			var createdResult = (CreatedAtRouteResult)result.Result!;
			Assert.Equal(applicationServiceModel, createdResult.Value);
			Assert.Equal("Get", createdResult.RouteName);
			Assert.Equal(applicationServiceModel.ApplicationId, createdResult.RouteValues!["id"]);
		}

		[Fact]
		public async Task Post_ServiceReturnsValidationErrorResult_BadRequestResponseReturned()
		{
			// arrange
			var mockCreateCommand = new Mock<IApplicationCreateCommand>();
			var mockGetQuery = new Mock<IApplicationGetQuery>();

			var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
			var applicationServiceModel = fixture.Create<ApplicationServiceModel>();
			mockCreateCommand.Setup(x => x.Execute(It.IsAny<ApplicationCreateRequestModel>()))
				.ReturnsAsync(new CreateValidationErrorResult<ApplicationServiceModel>(expectedValidationError));

			var subject = new ConversionApplicationController(mockCreateCommand.Object, mockGetQuery.Object);

			var requestModel = fixture.Create<ApplicationCreateRequestModel>();

			// act
			var result = await subject.Post(requestModel);

			// assert
			Assert.IsType<BadRequestObjectResult>(result.Result);
			var validationErrorResult = (BadRequestObjectResult)result.Result!;
			var validationerrors = (IReadOnlyCollection<ValidationError>)validationErrorResult.Value!;
			Assert.Equal(expectedValidationError, validationerrors);
		}

	}
}
