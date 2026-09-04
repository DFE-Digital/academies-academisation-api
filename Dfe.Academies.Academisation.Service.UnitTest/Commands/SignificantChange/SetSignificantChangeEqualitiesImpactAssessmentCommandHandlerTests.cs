using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeEqualitiesImpactAssessmentCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeEqualitiesImpactAssessmentCommandHandler>> _loggerMock;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly SetSignificantChangeEqualitiesImpactAssessmentCommandHandler _handler;

	public SetSignificantChangeEqualitiesImpactAssessmentCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeEqualitiesImpactAssessmentCommandHandler>>();
		_unitOfWorkMock = new Mock<IUnitOfWork>();

		_unitOfWorkMock
			.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);
		_repositoryMock.Setup(x => x.UnitOfWork).Returns(_unitOfWorkMock.Object);

		_handler = new SetSignificantChangeEqualitiesImpactAssessmentCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeEqualitiesImpactAssessmentCommand(
			Id: 100,
			EqualitiesImpactAssessmentCompleted: true,
			EqualitiesImpactIdentified: EqualitiesImpact.None,
			EqualitiesImpactIdentifiedMitigation: null);

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
		var command = new SetSignificantChangeEqualitiesImpactAssessmentCommand(
			Id: 200,
			EqualitiesImpactAssessmentCompleted: true,
			EqualitiesImpactIdentified: EqualitiesImpact.ImpactsIdentified,
			EqualitiesImpactIdentifiedMitigation: "Mitigation plan in place");

		var project = CreateProject();

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.EqualitiesImpactAssessmentCompleted.Should().BeTrue();
		project.Details.EqualitiesImpactIdentified.Should().Be(EqualitiesImpact.ImpactsIdentified);
		project.Details.EqualitiesImpactIdentifiedMitigation.Should().Be("Mitigation plan in place");
		project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task Handle_PartiallyCompletedSection_LeavesTaskInProgress()
	{
		var command = new SetSignificantChangeEqualitiesImpactAssessmentCommand(
			Id: 300,
			EqualitiesImpactAssessmentCompleted: false,
			EqualitiesImpactIdentified: null,
			EqualitiesImpactIdentifiedMitigation: null);

		var project = CreateProject();

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.EqualitiesImpactAssessmentCompleted.Should().BeFalse();
		project.Details.EqualitiesImpactIdentified.Should().BeNull();
		project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
	}

	[Fact]
	public async Task Handle_NullValues_ClearsPreviouslyAnsweredSection()
	{
		var project = CreateProject();
		project.SetEqualitiesImpactAssessment(true, EqualitiesImpact.ImpactsIdentified, "Previous mitigation");

		var command = new SetSignificantChangeEqualitiesImpactAssessmentCommand(
			Id: 400,
			EqualitiesImpactAssessmentCompleted: null,
			EqualitiesImpactIdentified: null,
			EqualitiesImpactIdentifiedMitigation: null);

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.EqualitiesImpactAssessmentCompleted.Should().BeNull();
		project.Details.EqualitiesImpactIdentified.Should().BeNull();
		project.Details.EqualitiesImpactIdentifiedMitigation.Should().BeNull();
		project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
	}

	[Fact]
	public async Task Handle_ProjectFound_DoesNotChangeTier()
	{
		var command = new SetSignificantChangeEqualitiesImpactAssessmentCommand(
			Id: 500,
			EqualitiesImpactAssessmentCompleted: true,
			EqualitiesImpactIdentified: EqualitiesImpact.PotentialImpacts,
			EqualitiesImpactIdentifiedMitigation: null);

		var project = CreateProject();

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);

		await _handler.Handle(command, CancellationToken.None);

		project.Tier.Should().Be(1);
	}

	[Fact]
	public async Task Handle_PassesCancellationTokenToRepository()
	{
		using var cancellationTokenSource = new CancellationTokenSource();
		var cancellationToken = cancellationTokenSource.Token;

		var command = new SetSignificantChangeEqualitiesImpactAssessmentCommand(
			Id: 600,
			EqualitiesImpactAssessmentCompleted: true,
			EqualitiesImpactIdentified: EqualitiesImpact.None,
			EqualitiesImpactIdentifiedMitigation: null);

		var project = CreateProject();

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, cancellationToken))
			.ReturnsAsync(project);

		await _handler.Handle(command, cancellationToken);

		_repositoryMock.Verify(x => x.GetSignificantChangeProjectById(command.Id, cancellationToken), Times.Once);
		_unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
	}

	private static SignificantChangeProject CreateProject()
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
