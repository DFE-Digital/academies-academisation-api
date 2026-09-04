using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeConsultationDurationCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeConsultationDurationCommandHandler>> _loggerMock;
	private readonly SetSignificantChangeConsultationDurationCommandHandler _handler;

	public SetSignificantChangeConsultationDurationCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeConsultationDurationCommandHandler>>();
		_handler = new SetSignificantChangeConsultationDurationCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeConsultationDurationCommand(
			id: 100,
			consultationLastedMinimumThreeWeeks: ConsultationDurationAnswer.Yes,
			consultationDurationNotMetReason: null);

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repositoryMock.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task Handle_ProjectFound_UpdatesSectionAndPersistsChanges()
	{
		var command = new SetSignificantChangeConsultationDurationCommand(
			id: 200,
			consultationLastedMinimumThreeWeeks: ConsultationDurationAnswer.No,
			consultationDurationNotMetReason: "Consultation ran for two weeks only");

		var project = BuildProject();
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock
			.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		_repositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.ConsultationLastedMinimumThreeWeeks.Should().Be(ConsultationDurationAnswer.No);
		project.Details.ConsultationDurationNotMetReason.Should().Be("Consultation ran for two weeks only");
		project.Tier.Should().Be(2);
		project.Details.GetConsultationDurationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task Handle_WhenSatisfactoryConsultationCarriedOut_DoesNotEscalateTier()
	{
		var command = new SetSignificantChangeConsultationDurationCommand(
			id: 300,
			consultationLastedMinimumThreeWeeks: ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut,
			consultationDurationNotMetReason: null);

		var project = BuildProject();
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock
			.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		_repositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Tier.Should().Be(1);
		project.Details.GetConsultationDurationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
	}

	private static SignificantChangeProject BuildProject()
	{
		return SignificantChangeProject.Create(
			urn: 123456,
			tier: 1,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			route: "Change of age range",
			schoolName: "Test School",
			createdOn: DateTime.UtcNow);
	}
}