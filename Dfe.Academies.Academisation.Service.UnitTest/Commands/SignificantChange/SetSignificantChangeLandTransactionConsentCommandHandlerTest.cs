using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeLandTransactionConsentCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeLandTransactionConsentCommandHandler>> _loggerMock;
	private readonly SetSignificantChangeLandTransactionConsentCommandHandler _handler;

	public SetSignificantChangeLandTransactionConsentCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeLandTransactionConsentCommandHandler>>();
		_handler = new SetSignificantChangeLandTransactionConsentCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeLandTransactionCommand(
			id: 100,
			landTransactionApplication: SignificantChangeGenericYesNoNa.Yes,
			landTransactionApplicationAdditionalInfo: null,
			landTransactionConsent: SignificantChangeGenericYesNoNa.Yes,
			landTransactionConsentAdditionalInfo: null,
			landTransactionSupportingEvidence: "evidence link"
        );

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
		var command = new SetSignificantChangeLandTransactionCommand(
			id: 200,
			landTransactionApplication: SignificantChangeGenericYesNoNa.Yes,
			landTransactionApplicationAdditionalInfo: null,
			landTransactionConsent: SignificantChangeGenericYesNoNa.No,
			landTransactionConsentAdditionalInfo: "some additional info",
			landTransactionSupportingEvidence: "evidence link"
        );

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
		project.Details.LandTransactionApplication.Should().Be(command.LandTransactionApplication);
		project.Details.LandTransactionApplicationAdditionalInfo.Should().Be(command.LandTransactionApplicationAdditionalInfo);
		project.Details.LandTransactionConsent.Should().Be(command.LandTransactionConsent);
		project.Details.LandTransactionConsentAdditionalInfo.Should().Be(command.LandTransactionConsentAdditionalInfo);
		project.Details.LandTransactionSupportingEvidence.Should().Be(command.LandTransactionSupportingEvidence);
		project.Tier.Should().Be(2);
		project.Details.GetLandTransactionTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}