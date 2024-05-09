using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class ConversionAdvisoryBoardDecisionControllerGetTests
{
	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionQueryService> _mockGetQuery = new();
	private readonly Mock<IMediator> _mockMediator = new();

	[Fact]
	public async Task QueryReturnIsNotNull___ReturnsOk()
	{
		//Arrange
		var data = _fixture.Create<ConversionAdvisoryBoardDecisionServiceModel>();

		_mockGetQuery.Setup(q => q.GetByProjectId(It.IsAny<int>(), false))
			.ReturnsAsync(data);

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		var result = await subject.GetByProjectId(It.IsAny<int>());

		//Assert
		var foundResult = Assert.IsType<OkObjectResult>(result.Result);
		var foundValue = Assert.IsType<ConversionAdvisoryBoardDecisionServiceModel>(foundResult.Value);
		Assert.Equal(data, foundValue);
	}

	[Fact]
	public async Task QueryReturnIsNull___ReturnsNotFound()
	{
		//Arrange
		_mockGetQuery.Setup(q => q.GetByProjectId(It.IsAny<int>(), false))
			.ReturnsAsync(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>());

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		var result = await subject.GetByProjectId(It.IsAny<int>());

		//Assert
		Assert.IsType<NotFoundResult>(result.Result);
	}
}
