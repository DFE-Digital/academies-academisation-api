using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Commands.LegalRequirement;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class LegalRequirementControllerTests
	{
		private readonly Fixture _fixture = new();
		private readonly Mock<ILegalRequirementCreateCommand> _createCommandMock = new();
		private readonly Mock<ILegalRequirementGetQuery> _getQueryMock = new();		
		private readonly Mock<ILegalRequirementUpdateCommand> _updateCommandMock = new();		
		private readonly LegalRequirementController _subject;


		public LegalRequirementControllerTests()
		{
			_subject = new LegalRequirementController(_getQueryMock.Object, _createCommandMock.Object, _updateCommandMock.Object);
		}		

		[Fact]
		public async Task Post___ServiceReturnsSuccess___SuccessResponseReturned()
		{
			// arrange
			var requestModel = _fixture.Create<LegalRequirementServiceModel>();

			_createCommandMock.Setup(x => x.Execute(requestModel))
				.ReturnsAsync(new CreateSuccessResult<LegalRequirementServiceModel>(requestModel));

			// act
			var result = await _subject.Create(requestModel);

			// assert
			var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
			Assert.Equal(requestModel, createdResult.Value);
			Assert.Equal("GET", createdResult.RouteName);
			Assert.Equal(requestModel.Id, createdResult.RouteValues!["id"]);
		}


		[Fact]
		public async Task Post___ServiceReturnsValidationError___BadRequestResponseReturned()
		{
			// arrange
			var requestModel = _fixture.Create<LegalRequirementServiceModel>();
			var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
			_createCommandMock.Setup(x => x.Execute(requestModel))
				.ReturnsAsync(new CreateValidationErrorResult<LegalRequirementServiceModel>(expectedValidationError));

			// act
			var result = await _subject.Create(requestModel);

			// assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}


		[Fact]
		public async Task Get___ServiceReturnsNull___NotFoundReturned()
		{
			// arrange
			int id = _fixture.Create<int>();

			// act
			var result = await _subject.Get(id);

			// assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task Get___ServiceReturnsApplication___SuccessResponseReturned()
		{
			// arrange
			int id = _fixture.Create<int>();
			var applicationServiceModel = _fixture.Create<LegalRequirementServiceModel>();

			_getQueryMock.Setup(x => x.Execute(id)).ReturnsAsync(applicationServiceModel);

			// act
			var result = await _subject.Get(id);

			// assert
			var getResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.Equal(applicationServiceModel, getResult.Value);
		}

		[Fact]
		public async Task Update___ServiceReturnsSuccess___SuccessResponseReturned()
		{
			// arrange
			int id = _fixture.Create<int>();
			var requestModel = _fixture.Create<LegalRequirementServiceModel>();

			_updateCommandMock.Setup(x => x.Execute(requestModel))
				.ReturnsAsync(new CommandSuccessResult());

			// act
			var result = await _subject.Update(requestModel);

			// assert
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task Update___ServiceReturnsNotFound___NotFoundReturned()
		{
			// arrange
			int id = _fixture.Create<int>();
			var requestModel = _fixture.Create<LegalRequirementServiceModel>();

			_updateCommandMock.Setup(x => x.Execute(requestModel))
				.ReturnsAsync(new NotFoundCommandResult());

			// act
			var result = await _subject.Update(requestModel);

			// assert
			Assert.IsType<NotFoundResult>(result);
		}


		[Fact]
		public async Task Update___ServiceReturnsBadRequest___BadRequestCommandResult()
		{
			// arrange
			int id = _fixture.Create<int>();
			var requestModel = _fixture.Create<LegalRequirementServiceModel>();

			_updateCommandMock.Setup(x => x.Execute(requestModel))
				.ReturnsAsync(new BadRequestCommandResult());

			// act
			var result = await _subject.Update(requestModel);

			// assert
			Assert.IsType<BadRequestResult>(result);
		}


		[Fact]
		public async Task Update___ServiceReturnsValidationError___ValidationErrorReturned()
		{
			// arrange
			int id = _fixture.Create<int>();
			var applicationServiceModel = _fixture.Create<LegalRequirementServiceModel>();

			var expectedValidationError = new List<ValidationError>() { new ValidationError("PropertyName", "Error message") };
			_updateCommandMock.Setup(x => x.Execute(applicationServiceModel))
				.ReturnsAsync(new CommandValidationErrorResult(expectedValidationError));

			// act
			var result = await _subject.Update(applicationServiceModel);

			// assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestObjectResult.Value);
			Assert.Equal(expectedValidationError, validationErrors);
		}		
	}
}
