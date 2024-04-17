using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using FluentAssertions;
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

		[Fact]
		public async Task GetByApplicationReference_ValidApplicationReference_ReturnsApplication()
		{
			//Arrange
			var applicationReference = "A2B_001";
			_mockDataQuery.Setup(x => x.GetByApplicationReference(applicationReference))
				.ReturnsAsync(new Fixture().Build<Application>().With(x => x.ApplicationReference, applicationReference).Create());
			ApplicationQueryService queryService = new(_mockDataQuery.Object, _mockMapper.Object);

			//Act
			var result = await queryService.GetByApplicationReference(applicationReference);
			
			//Assert
			result.Should().NotBeNull();
			result?.ApplicationReference.Should().Be(applicationReference);
		}

		[Fact]
		public async Task GetByApplicationReference_InvalidApplicationReference_ReturnsNull()
		{
			//Arrange
			var applicationReference = "A2B_000";
			ApplicationQueryService queryService = new(_mockDataQuery.Object, _mockMapper.Object);

			//Act
			var result = await queryService.GetByApplicationReference(applicationReference);
			
			//Assert
			result.Should().BeNull();
		}
	}
}
