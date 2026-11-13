using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange
{
	public class SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommandHandlerTests
	{
		private readonly Mock<ISignificantChangeProjectRepository> _repository = new();
		private readonly SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommandHandler _handler;

		public SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommandHandlerTests()
		{
			_handler = new SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommandHandler(
				_repository.Object,
				Mock.Of<ILogger<SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommandHandler>>());
		}

		[Fact]
		public async Task Handle_WhenProjectDoesNotExist_ReturnsNotFound()
		{
			var command = new SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand(66, YesNoNotApplicableAnswer.Yes, "Business case", null);
			
			_repository.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
				.ReturnsAsync((SignificantChangeProject?)null);

			var result = await _handler.Handle(command, CancellationToken.None);

			result.Should().BeOfType<NotFoundCommandResult>();
			_repository.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
		}

		[Fact]
		public async Task Handle_WhenProjectExists_UpdatesSatelliteSchoolConsiderationsAndPersistsChanges()
		{
			var project = CreateProject();
			var unitOfWork = new Mock<IUnitOfWork>();
			
			unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
			_repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
			_repository.Setup(x => x.GetSignificantChangeProjectById(180, It.IsAny<CancellationToken>())).ReturnsAsync(project);
			
			var result = await _handler.Handle(
				new SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand(180, YesNoNotApplicableAnswer.No, "Not a good idea", "Evidence"),
				CancellationToken.None);
			result.Should().BeOfType<CommandSuccessResult>();
			project.Details.SpecialSchoolSatelliteConsiderationsMeetRequirements.Should().Be(YesNoNotApplicableAnswer.No);
			project.Details.SpecialSchoolSatelliteConsiderationsAdditionalInformation.Should().Be("Not a good idea");
			project.Details.SpecialSchoolSatelliteConsiderationsSupportingEvidence.Should().Be("Evidence");
			
			_repository.Verify(x => x.Update(project), Times.Once);
			unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Theory]
		[InlineData(YesNoNotApplicableAnswer.Yes, "additional info", null)]
		[InlineData(YesNoNotApplicableAnswer.NotApplicable, "additional info", null)]
		[InlineData(YesNoNotApplicableAnswer.No, "additional info", "additional info")]
		public async Task Handle_WhenProjectExists_UpdatesShouldSetAdditionalInfo(YesNoNotApplicableAnswer answer, string? additionalInformation, string? expectedAdditioanInformation)
		{
			var project = CreateProject();
			var unitOfWork = new Mock<IUnitOfWork>();
			
			unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
			_repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
			_repository.Setup(x => x.GetSignificantChangeProjectById(180, It.IsAny<CancellationToken>())).ReturnsAsync(project);
			
			var result = await _handler.Handle(
				new SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand(180, answer, additionalInformation, "Evidence"),
				CancellationToken.None);
			result.Should().BeOfType<CommandSuccessResult>();
			project.Details.SpecialSchoolSatelliteConsiderationsMeetRequirements.Should().Be(answer);
			project.Details.SpecialSchoolSatelliteConsiderationsAdditionalInformation.Should().Be(expectedAdditioanInformation);
			project.Details.SpecialSchoolSatelliteConsiderationsSupportingEvidence.Should().Be("Evidence");
			
			_repository.Verify(x => x.Update(project), Times.Once);
			unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		private static SignificantChangeProject CreateProject() => SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
	}
}
