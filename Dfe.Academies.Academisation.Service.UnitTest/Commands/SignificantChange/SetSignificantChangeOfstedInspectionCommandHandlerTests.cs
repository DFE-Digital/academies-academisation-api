using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeOfstedInspectionCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repository = new();
	private readonly SetSignificantChangeOfstedInspectionCommandHandler _handler;

	public SetSignificantChangeOfstedInspectionCommandHandlerTests()
	{
		_handler = new SetSignificantChangeOfstedInspectionCommandHandler(
			_repository.Object,
			Mock.Of<ILogger<SetSignificantChangeOfstedInspectionCommandHandler>>());
	}

	[Fact]
	public async Task Handle_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var command = new SetSignificantChangeOfstedInspectionCommand(100, null, null, null, null);
		_repository.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repository.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
	}

	[Fact]
	public async Task Handle_WhenProjectExists_UpdatesOfstedInspectionAndPersistsChanges()
	{
		var inspectionDate = new DateTime(2026, 10, 20);
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		var unitOfWork = new Mock<IUnitOfWork>();
		unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
		_repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
		_repository.Setup(x => x.GetSignificantChangeProjectById(200, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await _handler.Handle(
			new SetSignificantChangeOfstedInspectionCommand(
				200,
				inspectionDate,
				false,
				"Standards were not met",
				"Inspection evidence"),
			CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.OfstedInspectionDate.Should().Be(inspectionDate);
		project.Details.MeetsOfstedStandards.Should().BeFalse();
		project.Details.OfstedAdditionalInformation.Should().Be("Standards were not met");
		project.Details.OfstedSupportingEvidence.Should().Be("Inspection evidence");
		_repository.Verify(x => x.Update(project), Times.Once);
		unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
