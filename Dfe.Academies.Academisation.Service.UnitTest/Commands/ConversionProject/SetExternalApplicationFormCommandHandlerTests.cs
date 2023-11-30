using AutoFixture.AutoMoq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
    public class SetExternalApplicationFormCommandHandlerTests
    {
        private MockRepository mockRepository;

        private Mock<IConversionProjectRepository> mockConversionProjectRepository;
		private readonly Mock<ILogger<SetExternalApplicationFormCommandHandler>> mockLogger;

		public SetExternalApplicationFormCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockConversionProjectRepository = this.mockRepository.Create<IConversionProjectRepository>();
            this.mockLogger = this.mockRepository.Create<ILogger<SetExternalApplicationFormCommandHandler>>();
		}

        private SetExternalApplicationFormCommandHandler CreateSetExternalApplicationFormCommandHandler()
        {
            return new SetExternalApplicationFormCommandHandler(
                this.mockConversionProjectRepository.Object, this.mockLogger.Object);
        }

		[Fact]
		public async Task Handle_ConversionProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var Id = 100101;
			var ExternalApplicationFormUrl = "http://test";
			var ExternalApplicationFormSaved = true;

			var command = new SetExternalApplicationFormCommand(Id, ExternalApplicationFormSaved, ExternalApplicationFormUrl);

			mockConversionProjectRepository.Setup(x => x.GetById(It.IsAny<int>()))!
				.ReturnsAsync((Project)null);

			// Act
			var result = await CreateSetExternalApplicationFormCommandHandler().Handle(command, default);

			// Assert
			result.Should().BeOfType<NotFoundCommandResult>();
			mockLogger.Verify(logger =>
					logger.Log(
						LogLevel.Error,
						It.IsAny<EventId>(),
						It.Is<It.IsAnyType>((v, t) => true),
						It.IsAny<Exception>(),
						It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
				Times.Once);
		}

		[Fact]
		public async Task Handle_ConversionProjectFound_ReturnsCommandSuccessResult()
		{
			// Arrange
			var fixture = new Fixture();
			fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });

			var Id = 100101;
			var ExternalApplicationFormUrl = "http://test";
			var ExternalApplicationFormSaved = true;

			var command = new SetExternalApplicationFormCommand(Id, ExternalApplicationFormSaved, ExternalApplicationFormUrl);
			// Create a conversion project to 'SetExternalApplicationForm' to
			var conversionProject = fixture.Create<Project>();
			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			mockConversionProjectRepository.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Mock GetById to use our Project from above
			mockConversionProjectRepository.Setup(x => x.GetConversionProject(It.Is<int>( x => x == command.Id))).ReturnsAsync(conversionProject);

			// Act
			var result = await CreateSetExternalApplicationFormCommandHandler().Handle(command, default);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();

			conversionProject.Details.ExternalApplicationFormSaved.Should().Be(command.ExternalApplicationFormSaved);
			conversionProject.Details.ExternalApplicationFormUrl.Should().Be(command.ExternalApplicationFormUrl);

			mockConversionProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Once);
		}
	}
}
