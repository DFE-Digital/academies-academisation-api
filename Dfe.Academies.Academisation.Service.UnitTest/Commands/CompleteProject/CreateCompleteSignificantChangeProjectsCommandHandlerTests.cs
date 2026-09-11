using System.Net;
using System.Net.Http.Json;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Commands.CompleteProject;
using Dfe.Academies.Academisation.Service.Factories;
using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.CompleteProject;

public class CreateCompleteSignificantChangeProjectsCommandHandlerTests
{
	private readonly Mock<ICompleteApiClientRetryFactory> _completeApiClientRetryFactory = new();
	private readonly Mock<ISignificantChangeProjectRepository> _significantChangeProjectRepository = new();
	private readonly Mock<IAdvisoryBoardDecisionRepository> _advisoryBoardDecisionRepository = new();
	private readonly Mock<ICompleteTransmissionLogRepository> _completeTransmissionLogRepository = new();
	private readonly Mock<IDateTimeProvider> _dateTimeProvider = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();
	private readonly Mock<ILogger<CreateCompleteSignificantChangeProjectsCommandHandler>> _logger = new();

	private CreateCompleteSignificantChangeProjectsCommandHandler CreateHandler()
	{
		_completeApiClientRetryFactory
			.Setup(factory => factory.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
			.Returns(Policy.NoOpAsync<HttpResponseMessage>());
		_significantChangeProjectRepository.Setup(repository => repository.UnitOfWork).Returns(_unitOfWork.Object);
		_completeTransmissionLogRepository.Setup(repository => repository.UnitOfWork).Returns(_unitOfWork.Object);

		return new CreateCompleteSignificantChangeProjectsCommandHandler(
			_completeApiClientRetryFactory.Object,
			_significantChangeProjectRepository.Object,
			_advisoryBoardDecisionRepository.Object,
			_completeTransmissionLogRepository.Object,
			_dateTimeProvider.Object,
			_logger.Object);
	}

	[Fact]
	public async Task Handle_WhenNoProjectsAreReady_ReturnsNotFoundAndDoesNotCallComplete()
	{
		_significantChangeProjectRepository
			.Setup(repository => repository.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<SignificantChangeProject>());

		var result = await CreateHandler().Handle(new CreateCompleteSignificantChangeProjectsCommand(), CancellationToken.None);

		Assert.IsType<NotFoundCommandResult>(result);
		_completeApiClientRetryFactory.Verify(
			factory => factory.CreateSignificantChangeProjectAsync(It.IsAny<Dfe.Complete.Client.Contracts.CreateSignificantChangeProjectCommand>(), It.IsAny<IAsyncPolicy<HttpResponseMessage>>(), It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[Fact]
	public async Task Handle_WhenCompleteSucceeds_MarksProjectSentAndWritesSuccessfulTransmissionLog()
	{
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 2, "Trust", "10000001", "Change of age range", "School"),
			DateTime.UtcNow);
		project.AssignUser(Guid.NewGuid(), "assigned.user@test.local", "Assigned User");
		project.SetReadOnlyDate(DateTime.UtcNow);
		var completeProjectId = Guid.NewGuid();

		_significantChangeProjectRepository
			.Setup(repository => repository.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([project]);
		
		_advisoryBoardDecisionRepository
			.Setup(repository => repository.GetSignificantChangeDecision(project.Id))
			.ReturnsAsync((ConversionAdvisoryBoardDecision?)null);
		
		_completeApiClientRetryFactory
			.Setup(factory => factory.CreateSignificantChangeProjectAsync(
				It.IsAny<Dfe.Complete.Client.Contracts.CreateSignificantChangeProjectCommand>(),
				It.IsAny<IAsyncPolicy<HttpResponseMessage>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created)
			{
				Content = JsonContent.Create(new CreateCompleteSignificantChangeSuccessResponse(completeProjectId))
			});

		_dateTimeProvider.Setup(provider => provider.Now).Returns(DateTime.UtcNow);

		var result = await CreateHandler().Handle(new CreateCompleteSignificantChangeProjectsCommand(), CancellationToken.None);

		Assert.IsType<CommandSuccessResult>(result);
		Assert.True(project.ProjectSentToComplete);
		Assert.Equal(completeProjectId, project.CompleteProjectId);
		_completeTransmissionLogRepository.Verify(repository => repository.Insert(It.Is<CompleteTransmissionLog>(log =>
			log.SignificantChangeProjectId == project.Id &&
			log.CompleteProjectId == completeProjectId &&
			log.IsSuccess)), Times.Once);
		_unitOfWork.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
	}

	[Fact]
	public async Task Handle_WhenCompleteFails_MarksProjectSentAndWritesFailedTransmissionLog()
	{
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 2, "Trust", "10000001", "Change of age range", "School"),
			DateTime.UtcNow);
		var responseMessage = "Validation failed";

		_significantChangeProjectRepository
			.Setup(repository => repository.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync([project]);
		_advisoryBoardDecisionRepository
			.Setup(repository => repository.GetSignificantChangeDecision(project.Id))
			.ReturnsAsync((ConversionAdvisoryBoardDecision?)null);
		_completeApiClientRetryFactory
			.Setup(factory => factory.CreateSignificantChangeProjectAsync(
				It.IsAny<Dfe.Complete.Client.Contracts.CreateSignificantChangeProjectCommand>(),
				It.IsAny<IAsyncPolicy<HttpResponseMessage>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
			{
				Content = JsonContent.Create(new CreateCompleteProjectErrorResponse(responseMessage))
			});

		var result = await CreateHandler().Handle(new CreateCompleteSignificantChangeProjectsCommand(), CancellationToken.None);

		Assert.IsType<CommandSuccessResult>(result);
		Assert.True(project.ProjectSentToComplete);
		Assert.Null(project.CompleteProjectId);
		_completeTransmissionLogRepository.Verify(repository => repository.Insert(It.Is<CompleteTransmissionLog>(log =>
			log.SignificantChangeProjectId == project.Id &&
			!log.IsSuccess &&
			log.Response == responseMessage)), Times.Once);
	}
}
