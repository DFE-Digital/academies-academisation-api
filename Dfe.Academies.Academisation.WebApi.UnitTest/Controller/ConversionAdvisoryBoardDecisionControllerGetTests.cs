using AutoFixture;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.Query;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ConversionAdvisoryBoardDecisionControllerGetTests
	{
		private readonly Fixture _fixture = new();
		private readonly Mock<IAdvisoryBoardDecisionCreateCommand> _mockCreateCommand = new();
		private readonly Mock<IConversionAdvisoryBoardDecisionGetQuery> _mockGetQuery = new();

		[Fact]
		public async Task QueryReturnIsNotNull___ReturnsOk()
		{
			//Arrange
			var data = _fixture.Create<ConversionAdvisoryBoardDecisionServiceModel>();

			_mockGetQuery.Setup(q => q.Execute(It.IsAny<int>()))
				.ReturnsAsync(data);

			var subject =
				new ConversionAdvisoryBoardDecisionController(_mockCreateCommand.Object, _mockGetQuery.Object);

			//Act
			var result = await subject.Get(It.IsAny<int>());
			
			//Assert
			var foundResult = Assert.IsType<OkObjectResult>(result.Result);
			var foundValue = Assert.IsType<ConversionAdvisoryBoardDecisionServiceModel>(foundResult.Value);
			Assert.Equal(data, foundValue);
		}

		[Fact]
		public async Task QueryReturnIsNull___ReturnsNotFound()
		{
			//Arrange
			_mockGetQuery.Setup(q => q.Execute(It.IsAny<int>()))
				.ReturnsAsync(It.IsAny<ConversionAdvisoryBoardDecisionServiceModel>());

			var subject =
				new ConversionAdvisoryBoardDecisionController(_mockCreateCommand.Object, _mockGetQuery.Object);

			//Act
			var result = await subject.Get(It.IsAny<int>());
			
			//Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}
	}
}
	
