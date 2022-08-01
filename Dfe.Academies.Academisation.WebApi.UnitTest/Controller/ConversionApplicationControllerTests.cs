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

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class ConversionApplicationControllerTests
{
	private readonly Fixture fixture = new();
	private readonly Mock<IApplicationGetQuery> _mockGetQuery = new();

	[Fact]
	public async Task Post___ServiceReturnsSuccess___SuccessResponseReturned()
	{
		// arrange
		var mockCreateCommand = new Mock<IApplicationCreateCommand>();
		
		var applicationServiceModel = fixture.Create<ApplicationServiceModel>();

		var requestModel = fixture.Create<ApplicationCreateRequestModel>();
		mockCreateCommand.Setup(x => x.Execute(requestModel))
			.ReturnsAsync(new CreateSuccessResult<ApplicationServiceModel>(applicationServiceModel));

		var subject = new ConversionApplicationController(mockCreateCommand.Object, _mockGetQuery.Object);

		// act
		var result = await subject.Post(requestModel);

		// assert
		Assert.IsType<CreatedAtRouteResult>(result.Result);
		var createdResult = (CreatedAtRouteResult)result.Result!;
		Assert.Equal(applicationServiceModel, createdResult.Value);
		Assert.Equal("Get", createdResult.RouteName);
		Assert.Equal(applicationServiceModel.ApplicationId, createdResult.RouteValues!["id"]);
	}

	[Fact]
	public async Task Post___ServiceReturnsValidationErrorResult___BadRequestResponseReturned()
	{
		// arrange
		var mockCreateCommand = new Mock<IApplicationCreateCommand>();

		var applicationServiceModel = fixture.Create<ApplicationServiceModel>();

		var requestModel = fixture.Create<ApplicationCreateRequestModel>();
		var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
		mockCreateCommand.Setup(x => x.Execute(requestModel))
			.ReturnsAsync(new CreateValidationErrorResult<ApplicationServiceModel>(expectedValidationError));

		var subject = new ConversionApplicationController(mockCreateCommand.Object, _mockGetQuery.Object);

		// act
		var result = await subject.Post(requestModel);

		// assert
		Assert.IsType<BadRequestObjectResult>(result.Result);
		var validationErrorResult = (BadRequestObjectResult)result.Result!;
		var validationErrors = (IReadOnlyCollection<ValidationError>)validationErrorResult.Value!;
		Assert.Equal(expectedValidationError, validationErrors);
	}
}

