using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeLocalAuthorityObjectionsCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeLocalAuthorityObjectionsCommandHandler>> _loggerMock;
	private readonly SetSignificantChangeLocalAuthorityObjectionsCommandHandler _handler;

	public SetSignificantChangeLocalAuthorityObjectionsCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeLocalAuthorityObjectionsCommandHandler>>();
		_handler = new SetSignificantChangeLocalAuthorityObjectionsCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeLocalAuthorityObjectionsCommand(
			id: 100,
			localAuthorityRaisedObjections: true,
			localAuthorityObjectionsFurtherInformation: "Local authority objection details",
			supportingEvidenceLink: "https://example.org/evidence");

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
		var command = new SetSignificantChangeLocalAuthorityObjectionsCommand(
			id: 200,
			localAuthorityRaisedObjections: true,
			localAuthorityObjectionsFurtherInformation: "Local authority has raised concerns",
			supportingEvidenceLink: "https://example.org/evidence");

		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(
				123456,
				1,
				"Test Trust",
				"12345678",
				"Change of age range",
				"Test School"),
			DateTime.UtcNow);

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
		project.Details.LocalAuthorityRaisedObjections.Should().BeTrue();
		project.Details.LocalAuthorityObjectionsFurtherInformation.Should().Be("Local authority has raised concerns");
		project.Details.SupportingEvidenceLink.Should().Be("https://example.org/evidence");
		project.Tier.Should().Be(2);
		project.Details.GetLocalAuthorityObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
