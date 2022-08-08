using System;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.Query;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class ConversionAdvisoryBoardDecisionControllerPostTests
{
	private class UnhandledCreateResult : CreateResult<ConversionAdvisoryBoardDecisionServiceModel>
	{
		public UnhandledCreateResult() : base(default) { }
	}
		
	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionCreateCommand> _mockCreateCommand = new();
	private readonly Mock<IConversionAdvisoryBoardDecisionGetQuery> _mockGetQuery = new();
	private readonly Mock<IAdvisoryBoardDecisionUpdateCommand> _mockUpdateCommand = new();

	[Fact]
	public async Task CommandReturnsCreateSuccessResult___ReturnsCreateAtRouteResult()
	{
		//Arrange
		var decisionServiceModel = _fixture.Create<ConversionAdvisoryBoardDecisionServiceModel>();

		_mockCreateCommand
			.Setup(c => c.Execute(It.IsAny<AdvisoryBoardDecisionCreateRequestModel>()))
			.ReturnsAsync(new CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel>(decisionServiceModel));

		var subject = new ConversionAdvisoryBoardDecisionController(
			_mockCreateCommand.Object, 
			_mockGetQuery.Object,
			_mockUpdateCommand.Object);
			
		//Act
		var result = await subject.Post(It.IsAny<AdvisoryBoardDecisionCreateRequestModel>());

		//Assert
		var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);

		Assert.Equal(decisionServiceModel, createdResult.Value);
		Assert.Equal(HttpMethods.Get, createdResult.RouteName);
		Assert.Equal(decisionServiceModel.AdvisoryBoardDecisionId, createdResult.RouteValues!["id"]);
	}
		
	[Fact]
	public async Task CommandReturnsCreateValidationErrorResult___ReturnsBadRequestResult()
	{
		var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();
			
		//Arrange
		_mockCreateCommand
			.Setup(c => c.Execute(It.IsAny<AdvisoryBoardDecisionCreateRequestModel>()))
			.ReturnsAsync(
				new CreateValidationErrorResult<ConversionAdvisoryBoardDecisionServiceModel>(expectedValidationErrors));

		var subject = new ConversionAdvisoryBoardDecisionController(
			_mockCreateCommand.Object, 
			_mockGetQuery.Object,
			_mockUpdateCommand.Object);
			
		//Act
		var result = await subject.Post(It.IsAny<AdvisoryBoardDecisionCreateRequestModel>());

		//Assert
		var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
		var validationErrors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);			
		Assert.Equal(expectedValidationErrors, validationErrors);
	}
		
	[Fact]
	public async Task CommandReturnsCreateUnhandledCreateResult___ThrowsException()
	{
		//Arrange
		_mockCreateCommand
			.Setup(c => c.Execute(It.IsAny<AdvisoryBoardDecisionCreateRequestModel>()))
			.ReturnsAsync(new UnhandledCreateResult());

		var subject = new ConversionAdvisoryBoardDecisionController(
			_mockCreateCommand.Object, 
			_mockGetQuery.Object,
			_mockUpdateCommand.Object);
			
		//Act
		await Assert.ThrowsAsync<NotImplementedException>(() => subject.Post(It.IsAny<AdvisoryBoardDecisionCreateRequestModel>()));
	}
}
