using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class ApplicationGetQueryTests
	{
		private readonly Mock<IApplicationRepository> _mockDataQuery = new();
		private readonly Mock<IMapper> _mockMapper = new();

		[Fact]
		public async Task WhenDataQueryReturnIsNull___ReturnsNull()
		{
			//Arrange
			const int requestedId = 4;

			ApplicationQueryService queryService = new(_mockDataQuery.Object, _mockMapper.Object);

			//Act
			var result = await queryService.GetById(requestedId);

			//Assert
			Assert.Null(result);
		}
	}
}
