using AutoFixture.AutoMoq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
	public class SetPerformanceDataCommandHandlerTests
    {
		private MockRepository mockRepository;

		private Mock<IConversionProjectRepository> mockConversionProjectRepository;
		private readonly Mock<ILogger<SetPerformanceDataCommandHandler>> mockLogger;

		public SetPerformanceDataCommandHandlerTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Default);

			this.mockConversionProjectRepository = this.mockRepository.Create<IConversionProjectRepository>();
			this.mockLogger = this.mockRepository.Create<ILogger<SetPerformanceDataCommandHandler>>();
		}

		private SetPerformanceDataCommandHandler CreateSetExternalApplicationFormCommandHandler()
		{
			return new SetPerformanceDataCommandHandler(
				this.mockConversionProjectRepository.Object, this.mockLogger.Object);
		}

		[Fact]
		public async Task Handle_ConversionProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var Id = 100101;
			string? keyStage2PerformanceAdditionalInformation = "Test 1";
			string? keyStage4PerformanceAdditionalInformation = "Test 2";
			string? keyStage5PerformanceAdditionalInformation = "Test 3";
			string? educationalAttendanceAdditionalInformation = "Test 4";

			var command = new SetPerformanceDataCommand(Id, keyStage2PerformanceAdditionalInformation, keyStage4PerformanceAdditionalInformation, keyStage5PerformanceAdditionalInformation, educationalAttendanceAdditionalInformation);


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
			string? keyStage2PerformanceAdditionalInformation = "Test 1";
			string? keyStage4PerformanceAdditionalInformation = "Test 2";
			string? keyStage5PerformanceAdditionalInformation = "Test 3";
			string? educationalAttendanceAdditionalInformation = "Test 4";

			var command = new SetPerformanceDataCommand(Id, keyStage2PerformanceAdditionalInformation, keyStage4PerformanceAdditionalInformation, keyStage5PerformanceAdditionalInformation, educationalAttendanceAdditionalInformation);


			// Create a conversion project to 'SetExternalApplicationForm' to
			var conversionProject = fixture.Create<Project>();
			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			mockConversionProjectRepository.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Mock GetById to use our Project from above
			mockConversionProjectRepository.Setup(x => x.GetConversionProject(It.Is<int>(x => x == command.Id))).ReturnsAsync(conversionProject);

			// Act
			var result = await CreateSetExternalApplicationFormCommandHandler().Handle(command, default);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();

			conversionProject.Details.KeyStage2PerformanceAdditionalInformation.Should().Be(command.KeyStage2PerformanceAdditionalInformation);
			conversionProject.Details.KeyStage4PerformanceAdditionalInformation.Should().Be(command.KeyStage4PerformanceAdditionalInformation);
			conversionProject.Details.KeyStage5PerformanceAdditionalInformation.Should().Be(command.KeyStage5PerformanceAdditionalInformation);
			conversionProject.Details.EducationalAttendanceAdditionalInformation.Should().Be(command.EducationalAttendanceAdditionalInformation);

			mockConversionProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Once);
		}
	}
}
