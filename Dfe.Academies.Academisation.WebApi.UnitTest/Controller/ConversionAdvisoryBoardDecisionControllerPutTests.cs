using System;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.Query;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class ConversionAdvisoryBoardDecisionControllerPutTests
{
	private class UnhandledUpdateResult : CommandResult {}

	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionCreateCommand> _mockCreateCommand = new();
	private readonly Mock<IConversionAdvisoryBoardDecisionGetQuery> _mockGetQuery = new();
	private readonly Mock<IAdvisoryBoardDecisionUpdateCommand> _mockUpdateCommand = new();


	[Fact]
	public async Task CommandReturnsCommandSuccessResult___ReturnsOkResult()
	{
		//Arrange
		_mockUpdateCommand
			.Setup(c => c.Execute(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>()))
			.ReturnsAsync(new CommandSuccessResult());

		var subject = new ConversionAdvisoryBoardDecisionController(
			_mockCreateCommand.Object, 
			_mockGetQuery.Object,
			_mockUpdateCommand.Object);
			
		//Act
		var result = await subject.Put(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>());

		//Assert
		Assert.IsType<OkResult>(result);
	}
		
	[Fact]
	public async Task CommandReturnsCommandValidationErrorResult___ReturnsBadRequestResult()
	{
		var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();
			
		//Arrange
		_mockUpdateCommand
			.Setup(c => c.Execute(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>()))
			.ReturnsAsync(new CommandValidationErrorResult(expectedValidationErrors));

		var subject = new ConversionAdvisoryBoardDecisionController(
			_mockCreateCommand.Object, 
			_mockGetQuery.Object,
			_mockUpdateCommand.Object);
			
		//Act
		var result = await subject.Put(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>());

		//Assert
		var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
		var validationErrors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);			
		Assert.Equal(expectedValidationErrors, validationErrors);
	}
		
	[Fact]
	public async Task CommandReturnsCreateUnhandledUpdateResult___ThrowsException()
	{
		_mockUpdateCommand
			.Setup(c => c.Execute(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>()))
			.ReturnsAsync(new UnhandledUpdateResult());
			
		//Arrange
		var subject = new ConversionAdvisoryBoardDecisionController(
			_mockCreateCommand.Object, 
			_mockGetQuery.Object,
			_mockUpdateCommand.Object);
			
		//Act && Assert
		await Assert.ThrowsAsync<NotImplementedException>(
			() => subject.Put(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>()));
	}
}
