using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeAdmissionsVariationRecommendationCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
	private readonly Mock<ILogger<SetSignificantChangeAdmissionsVariationRecommendationCommandHandler>> _loggerMock;
	private readonly SetSignificantChangeAdmissionsVariationRecommendationCommandHandler _handler;

	public SetSignificantChangeAdmissionsVariationRecommendationCommandHandlerTests()
	{
		_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
		_loggerMock = new Mock<ILogger<SetSignificantChangeAdmissionsVariationRecommendationCommandHandler>>();
		_handler = new SetSignificantChangeAdmissionsVariationRecommendationCommandHandler(_repositoryMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ProjectNotFound_ReturnsNotFoundCommandResult()
	{
		var command = new SetSignificantChangeAdmissionsVariationRecommendationCommand(
			Id: 100,
			RecommendationAnswer: AdmissionsVariationRecommendationAnswer.Approve,
			FurtherInformation: "Approved with conditions");

		_repositoryMock
			.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repositoryMock.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task Handle_ProjectFound_UpdatesRecommendationAndPersistsChanges()
	{
		var command = new SetSignificantChangeAdmissionsVariationRecommendationCommand(
			Id: 200,
			RecommendationAnswer: AdmissionsVariationRecommendationAnswer.Defer,
			FurtherInformation: "Deferred pending review");

		var project = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
				123456,
				1,
				"Test Trust",
				"12345678",
				"Change of age range",
				"Test School"), DateTime.UtcNow
		);

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
		project.Details.AdmissionsVariationRecommendation.Should().Be(AdmissionsVariationRecommendationAnswer.Defer);
		project.Details.AdmissionsVariationRecommendationFurtherInformation.Should().Be("Deferred pending review");
		project.Details.GetAdmissionsVariationRecommendationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);

		_repositoryMock.Verify(x => x.Update(project), Times.Once);
		_repositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
