using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeStakeholderConsultationCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeStakeholderConsultationCommandHandler>> _loggerMock;
	private readonly SetSignificantChangeStakeholderConsultationCommandHandler _handler;

	public SetSignificantChangeStakeholderConsultationCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeStakeholderConsultationCommandHandler>>();
		_handler = new SetSignificantChangeStakeholderConsultationCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeStakeholderConsultationCommand(
			id: 100,
			trustConsultedStakeholders: true,
			trustConsultedStakeholdersNotConsultedReason: null);

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
		var command = new SetSignificantChangeStakeholderConsultationCommand(
			id: 200,
			trustConsultedStakeholders: false,
			trustConsultedStakeholdersNotConsultedReason: "Trust did not consult stakeholders");

		var project = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
			urn: 123456,
			tier: 1,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			typeOfSignificantChange: "Change of age range",
			schoolName: "Test School"),
			createdOn: DateTime.UtcNow);

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
		project.Details.TrustConsultedStakeholders.Should().BeFalse();
		project.Details.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Trust did not consult stakeholders");
		project.Tier.Should().Be(2);
		project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}