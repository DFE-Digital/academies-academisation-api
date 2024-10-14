using AutoFixture.AutoMoq;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Moq;
using Xunit;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
    public class ConversionProjectQueryServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IConversionProjectRepository> mockConversionProjectRepository;
        private Mock<IFormAMatProjectRepository> mockFormAMatProjectRepository;
		private readonly Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository = new();
		private readonly Fixture _fixture = new();

			
		public ConversionProjectQueryServiceTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockConversionProjectRepository = mockRepository.Create<IConversionProjectRepository>();
            mockFormAMatProjectRepository = mockRepository.Create<IFormAMatProjectRepository>();
			_mockAdvisoryBoardDecisionRepository = mockRepository.Create<IAdvisoryBoardDecisionRepository>();
			_fixture.Customize(new AutoMoqCustomization());
        }

        private ConversionProjectQueryService CreateService()
        {
            return new ConversionProjectQueryService(
                mockConversionProjectRepository.Object,
                mockFormAMatProjectRepository.Object, _mockAdvisoryBoardDecisionRepository.Object);
        }

        [Fact]
        public async Task GetProjectsForGroup_StateUnderTest_ExpectedBehavior()
        {
			// Arrange
			var expectedProjects = _fixture.Create<List<Project>>();
			mockConversionProjectRepository.Setup(m => m.GetConversionProjectsForNewGroup(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedProjects);
			var service = CreateService();

			// Act
			var result = await service.GetProjectsForGroup("trustReferenceNumber", default);

			// Assert
			Assert.Multiple(
				() => Assert.Equivalent(expectedProjects[0].MapToServiceModel(), result![0]),
				() => Assert.Equivalent(expectedProjects[1].MapToServiceModel(), result![1]),
				() => Assert.Equivalent(expectedProjects[2].MapToServiceModel(), result![2]));
          
            mockRepository.VerifyAll();
        }
    }
}
