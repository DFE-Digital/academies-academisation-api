using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangePlanningPermissionCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangePlanningPermissionCommandHandler>> _loggerMock;
	private readonly SetSignificantChangePlanningPermissionCommandHandler _handler;

	public SetSignificantChangePlanningPermissionCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangePlanningPermissionCommandHandler>>();
		_handler = new SetSignificantChangePlanningPermissionCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangePlanningPermissionCommand(
			Id: 100,
			PlanningPermissionAnswer: PlanningPermissionAnswer.Yes,
			AdditionalInformation: "Further detail",
			SupportingEvidence: "Supporting evidence");

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repositoryMock.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task Handle_ProjectFound_UpdatesPlanningPermissionAndPersistsChanges()
	{
		var command = new SetSignificantChangePlanningPermissionCommand(
			Id: 200,
			PlanningPermissionAnswer: PlanningPermissionAnswer.No,
			AdditionalInformation: "Planning permission not granted yet",
			SupportingEvidence: "Awaiting local authority determination");

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
		project.Details.PlanningPermission.Should().Be(PlanningPermissionAnswer.No);
		project.Details.PlanningPermissionAdditionalInformation.Should().Be("Planning permission not granted yet");
		project.Details.PlanningPermissionSupportingEvidence.Should().Be("Awaiting local authority determination");
		project.Details.GetPlanningPermissionTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	private static SignificantChangeProject BuildProject()
	{
		return SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(
				urn: 123456,
				tier: 1,
				trustName: "Test Trust",
				trustUkprn: "12345678",
				typeOfSignificantChange: "Change of age range",
				schoolName: "Test School"),
			createdOn: DateTime.UtcNow);
	}
}
