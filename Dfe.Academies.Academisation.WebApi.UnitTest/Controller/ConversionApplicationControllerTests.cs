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
	public class ConversionApplicationControllerTests
	{
		private readonly Fixture fixture = new();
		private readonly Mock<IApplicationCreateCommand> _createCommandMock = new();
		private readonly Mock<IApplicationGetQuery> _getQueryMock = new();
		private readonly Mock<IApplicationSubmitCommand> _submitCommandMock = new();
		private readonly ConversionApplicationController _subject;

		public ConversionApplicationControllerTests()
		{
			_subject = new ConversionApplicationController(_createCommandMock.Object, _getQueryMock.Object, _submitCommandMock.Object);
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
			var validationErrorResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			var validationErrors = (IReadOnlyCollection<ValidationError>)validationErrorResult.Value!;
			Assert.Equal(expectedValidationError, validationErrors);
		}
	}
}
