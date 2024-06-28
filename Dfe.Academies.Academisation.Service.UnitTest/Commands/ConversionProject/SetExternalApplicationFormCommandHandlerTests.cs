using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
	public class SetFormAMatProjectReferenceCommandHandlerTests
	{
		private readonly MockRepository mockRepository;

		private readonly Mock<IConversionProjectRepository> mockConversionProjectRepository;
		private readonly Mock<ILogger<SetFormAMatProjectReferenceCommandHandler>> mockLogger;

		public SetFormAMatProjectReferenceCommandHandlerTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);

			this.mockConversionProjectRepository = this.mockRepository.Create<IConversionProjectRepository>();
			this.mockLogger = this.mockRepository.Create<ILogger<SetFormAMatProjectReferenceCommandHandler>>();
		}

		private SetFormAMatProjectReferenceCommandHandler CreateHandler()
		{
			return new SetFormAMatProjectReferenceCommandHandler(
				this.mockConversionProjectRepository.Object,
				this.mockLogger.Object);
		}

		[Fact]
		public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var projectId = 1;
			var formAMatProjectId = 2;
			var command = new SetFormAMatProjectReferenceCommand(projectId, formAMatProjectId);

			this.mockConversionProjectRepository
				.Setup(x => x.GetConversionProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Project)null);
			this.mockLogger.Setup(logger =>
			logger.Log(
				LogLevel.Error, // The LogLevel parameter
				It.IsAny<EventId>(), // The EventId parameter, set to any value
				It.Is<It.IsAnyType>((o, t) =>
					o.ToString().Contains("Conversion project not found with id:")), // The state parameter, checking the message contains the expected text
				null, // The exception parameter, set to null as no exception is passed in the LogError method
				It.IsAny<Func<It.IsAnyType, Exception, string>>())) // The formatter parameter
			.Verifiable("Logging failure when project not found.");

			var handler = CreateHandler();

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().BeOfType<NotFoundCommandResult>();

			// At the end of your test
			this.mockLogger.Verify();

		}

		[Fact]
		public async Task Handle_ProjectFound_UpdatesFormAMatProjectReference()
		{
			// Arrange
			var fixture = new Fixture().Customize(new AutoMoqCustomization());
			var projectId = 1;
			var formAMatProjectId = 2;
			var command = new SetFormAMatProjectReferenceCommand(projectId, formAMatProjectId);
			var project = fixture.Create<Project>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
						   .Returns(Task.FromResult(1));

			this.mockConversionProjectRepository
				.Setup(x => x.GetConversionProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(project);

			this.mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Project>()))
				.Verifiable(); // Setup for Update method

			this.mockConversionProjectRepository.SetupGet(x => x.UnitOfWork)
				.Returns(mockUnitOfWork.Object);

			var handler = CreateHandler();

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();
			this.mockConversionProjectRepository.Verify(x => x.Update(It.IsAny<Project>()), Times.Once);
			mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

	}
}
