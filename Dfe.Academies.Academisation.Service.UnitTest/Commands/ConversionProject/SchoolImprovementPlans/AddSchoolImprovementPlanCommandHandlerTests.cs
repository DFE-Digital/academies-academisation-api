using AutoFixture.AutoMoq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
	public class AddSchoolImprovementPlanCommandHandlerTests
    {
		private MockRepository mockRepository;

		private Mock<IConversionProjectRepository> mockConversionProjectRepository;
		private readonly Mock<ILogger<ConversionProjectAddSchoolImprovementPlanCommandHandler>> mockLogger;

		public AddSchoolImprovementPlanCommandHandlerTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Default);

			this.mockConversionProjectRepository = this.mockRepository.Create<IConversionProjectRepository>();
			this.mockLogger = this.mockRepository.Create<ILogger<ConversionProjectAddSchoolImprovementPlanCommandHandler>>();
		}

		private ConversionProjectAddSchoolImprovementPlanCommandHandler CreateConversionProjectAddSchoolImprovementPlanCommandHandler()
		{
			return new ConversionProjectAddSchoolImprovementPlanCommandHandler(
				this.mockConversionProjectRepository.Object, this.mockLogger.Object);
		}

		[Fact]
		public async Task Handle_ConversionProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var Id = 100101;
			var arrangers = new List<SchoolImprovementPlanArranger>() { SchoolImprovementPlanArranger.LocalAuthority };
			var providedBy = "Trust CEO";
			var startDate = DateTime.UtcNow;
			var expectedEndate = SchoolImprovementPlanExpectedEndDate.ToConversion;
			var confidenceLevel = SchoolImprovementPlanConfidenceLevel.Low;
			var planComments = "test comments";

			var command = new ConversionProjectAddSchoolImprovementPlanCommand(Id, arrangers, null, providedBy, startDate, expectedEndate, null, confidenceLevel, planComments);

			mockConversionProjectRepository.Setup(x => x.GetById(It.IsAny<int>()))!
				.ReturnsAsync((Project)null);

			// Act
			var result = await CreateConversionProjectAddSchoolImprovementPlanCommandHandler().Handle(command, default);

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

			// Create a conversion project

			var conversionProject = fixture.Create<Project>();
			var arrangers = new List<SchoolImprovementPlanArranger>() { SchoolImprovementPlanArranger.LocalAuthority };
			var providedBy = "Trust CEO";
			var startDate = DateTime.UtcNow;
			var expectedEndate = SchoolImprovementPlanExpectedEndDate.ToConversion;
			var confidenceLevel = SchoolImprovementPlanConfidenceLevel.Low;
			var planComments = "test comments";

			var command = new ConversionProjectAddSchoolImprovementPlanCommand(conversionProject.Id, arrangers, null, providedBy, startDate, expectedEndate, null, confidenceLevel, planComments);

			
			// Mock Unit of work and Repository 
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(1));  // Return Task<int> with a dummy value
			mockConversionProjectRepository.Setup(repo => repo.UnitOfWork)
				.Returns(unitOfWorkMock.Object);

			// Mock GetById to use our Project from above
			mockConversionProjectRepository.Setup(x => x.GetConversionProject(It.Is<int>(x => x == command.ProjectId), It.IsAny<CancellationToken>())).ReturnsAsync(conversionProject);

			// Act
			var result = await CreateConversionProjectAddSchoolImprovementPlanCommandHandler().Handle(command, default);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();

			var improvementPlan = conversionProject.SchoolImprovementPlans.First();

			improvementPlan.ProjectId.Should().Be(conversionProject.Id);
			improvementPlan.ArrangedBy.Should().BeEquivalentTo(arrangers);
			improvementPlan.ArrangedByOther.Should().BeNull();
			improvementPlan.ProvidedBy.Should().Be(providedBy);
			improvementPlan.StartDate.Should().Be(startDate);
			improvementPlan.ExpectedEndDate.Should().Be(expectedEndate);
			improvementPlan.ExpectedEndDateOther.Should().BeNull();
			improvementPlan.ConfidenceLevel.Should().Be(confidenceLevel);
			improvementPlan.PlanComments.Should().Be(planComments);

			mockConversionProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Once);
		}
	}
}
