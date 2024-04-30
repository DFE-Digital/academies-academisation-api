using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class ConversionAdvisoryBoardDecisionControllerPostTests
{
	private class UnhandledCreateResult : CreateResult
	{
		public UnhandledCreateResult() : base(default) { }
	}

	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionQueryService> _mockGetQuery = new();
	private readonly Mock<IMediator> _mockMediator = new();

	[Fact]
	public async Task CommandReturnsCreateSuccessResult___ReturnsCreateAtRouteResult()
	{
		//Arrange
		var decisionServiceModel = _fixture.Create<ConversionAdvisoryBoardDecisionServiceModel>();

		_mockMediator
			.Setup(c => c.Send(It.IsAny<AdvisoryBoardDecisionCreateCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel>(decisionServiceModel));

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		var result = await subject.Post(It.IsAny<AdvisoryBoardDecisionCreateCommand>(), default);

		//Assert
		var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);

		Assert.Equal(decisionServiceModel, createdResult.Value);
		Assert.Equal("GetProject", createdResult.RouteName);
		Assert.Equal(decisionServiceModel.AdvisoryBoardDecisionId, createdResult.RouteValues!["projectId"]);
	}

	[Fact]
	public async Task CommandReturnsCreateValidationErrorResult___ReturnsBadRequestResult()
	{
		var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();

		//Arrange
		_mockMediator
			.Setup(c => c.Send(It.IsAny<AdvisoryBoardDecisionCreateCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(
				new CreateValidationErrorResult(expectedValidationErrors));

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		var result = await subject.Post(It.IsAny<AdvisoryBoardDecisionCreateCommand>(), It.IsAny<CancellationToken>());

		//Assert
		var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
		var validationErrors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
		Assert.Equal(expectedValidationErrors, validationErrors);
	}

	[Fact]
	public async Task CommandReturnsCreateUnhandledCreateResult___ThrowsException()
	{
		//Arrange
		_mockMediator
			.Setup(c => c.Send(It.IsAny<AdvisoryBoardDecisionCreateCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new UnhandledCreateResult());

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		await Assert.ThrowsAsync<NotImplementedException>(() => subject.Post(It.IsAny<AdvisoryBoardDecisionCreateCommand>(), It.IsAny<CancellationToken>()));
	}
}
