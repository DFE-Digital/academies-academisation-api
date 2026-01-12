using AutoFixture.AutoMoq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using System.Net;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Commands.CompleteProject;
using Dfe.Academies.Academisation.Service.Factories; 
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Polly;
using Xunit;
using Dfe.Complete.Client.Contracts;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.CompleteProject
{
    public class CreateCompleteTransferProjectsCommandHandlerTests
    { 
		private readonly Fixture _fixture = new();

		private readonly Mock<ITransferProjectRepository> _mockTransferProjectRepository;
		private readonly Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository;
		private readonly Mock<IAcademiesQueryService> _mockAcademiesQueryService;
		private readonly Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private readonly Mock<ICompleteTransmissionLogRepository> _mockCompleteTransmissionLogRepository; 
		private readonly Mock<IDateTimeProvider> _mockDateTimeProvider; 
		private readonly Mock<IPollyPolicyFactory> _mockPollyPolicyFactory;
		private readonly Mock<ILogger<CreateCompleteTransferProjectsCommandHandler>> _mockLogger;
		private CreateCompleteTransferProjectsCommandHandler? _handler;
		private readonly Mock<IProjectsClient> _mockProjectsClient;

		public CreateCompleteTransferProjectsCommandHandlerTests()
		{
			var mockRepository = new MockRepository(MockBehavior.Default);

			_mockTransferProjectRepository = mockRepository.Create<ITransferProjectRepository>();
			_mockAdvisoryBoardDecisionRepository = mockRepository.Create<IAdvisoryBoardDecisionRepository>();
			_mockAcademiesQueryService = mockRepository.Create<IAcademiesQueryService>();
			_mockProjectGroupRepository = mockRepository.Create<IProjectGroupRepository>();
			_mockCompleteTransmissionLogRepository = mockRepository.Create<ICompleteTransmissionLogRepository>(); 
			_mockDateTimeProvider = mockRepository.Create<IDateTimeProvider>();
			_mockPollyPolicyFactory = mockRepository.Create<IPollyPolicyFactory>();
			_mockLogger = mockRepository.Create<ILogger<CreateCompleteTransferProjectsCommandHandler>>();
			_mockProjectsClient = mockRepository.Create<IProjectsClient>();
			_fixture.Customize(new AutoMoqCustomization());
		}

		private CreateCompleteTransferProjectsCommandHandler CreateCreateCompleteTransferProjectsCommandHandler()
		{
			return new CreateCompleteTransferProjectsCommandHandler(
				_mockTransferProjectRepository.Object,
				_mockAdvisoryBoardDecisionRepository.Object,
				_mockAcademiesQueryService.Object,
				_mockCompleteTransmissionLogRepository.Object, 
				_mockDateTimeProvider.Object,
				new CompleteApiClientRetryFactory(_mockPollyPolicyFactory.Object, _mockProjectsClient.Object),
				_mockLogger.Object);
		}

		public static Mock<HttpMessageHandler> CreateHttpMessageHandlerMock(HttpStatusCode code, object content)
		{
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(() =>
				{
					return new HttpResponseMessage(code)
					{
						Content = new StringContent(JsonConvert.SerializeObject(content))
					};
				});
			return handlerMock;
		}

		[Fact]
		public async Task Handle_NoTransferProjectsFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var command = _fixture.Create<CreateCompleteTransferProjectsCommand>();
			_mockTransferProjectRepository.Setup(repo => repo.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync((IEnumerable<ITransferProject>)null!);
			 
			_mockProjectsClient.Setup(client => client.CreateTransferProjectAsync(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(_fixture.Create<ProjectId>());

			_handler = CreateCreateCompleteTransferProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
			// Verifying that the logger was called for the "No transfer projects found" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No transfer projects found.")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

			// Verifying that the GetFormAMatProjectsToSendToCompleteAsync was called once
			_mockTransferProjectRepository.Verify(repo => repo.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
			_mockProjectsClient.Verify(client => client.CreateTransferProjectAsync(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()), Times.Never());
		}
		[Fact]
		public async Task Handle_ConversionProjectsExist_SuccessfulResponse_ReturnsCommandSuccessResult()
		{
			// Arrange
			var command = _fixture.Create<CreateCompleteTransferProjectsCommand>();
			var transferProjects = _fixture.CreateMany<ITransferProject>().ToList();
			var establishments = new List<EstablishmentDto>();
			int transferringAcademiesCount = 0;
			foreach (var transferProject in transferProjects)
			{
				Mock.Get(transferProject).Setup(x => x.Id).Returns(_fixture.Create<int>());
				Mock.Get(transferProject).Setup(x => x.SpecificReasonsForTransfer).Returns(new List<string> { "Forced" });
				Mock.Get(transferProject).Setup(x => x.AssignedUserEmailAddress).Returns("test@test.com");
				Mock.Get(transferProject).Setup(x => x.AssignedUserFullName).Returns("TestFirst TestLast");
				Mock.Get(transferProject).Setup(x => x.AssignedUserId).Returns(Guid.NewGuid());
				Mock.Get(transferProject).Setup(x => x.OutgoingTrustUkprn).Returns("111111");
				Mock.Get(transferProject).Setup(x => x.HtbDate).Returns(DateTime.Now);
				Mock.Get(transferProject).Setup(x => x.TargetDateForTransfer).Returns(DateTime.Now);

				var transferringAcademies = _fixture.CreateMany<ITransferringAcademy>().ToList();
				transferringAcademiesCount = transferringAcademiesCount + transferringAcademies.Count;
				foreach (var academy in transferringAcademies)
				{
					Mock.Get(academy).Setup(x => x.TransferProjectId).Returns(transferProject.Id);
					Mock.Get(academy).Setup(x => x.OutgoingAcademyUkprn).Returns(_fixture.Create<string>());
					Mock.Get(academy).Setup(x => x.IncomingTrustUkprn).Returns("101010");
					establishments.Add(new EstablishmentDto { Urn = "101010", Ukprn = academy.OutgoingAcademyUkprn });
				}

				Mock.Get(transferProject).Setup(x => x.TransferringAcademies).Returns(transferringAcademies);
			}

			var advisoryDecision = _fixture.Create<ConversionAdvisoryBoardDecision>(); 
			var projectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			var mockContext = new Mock<IUnitOfWork>();

			_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockCompleteTransmissionLogRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_mockTransferProjectRepository.Setup(repo => repo.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transferProjects);
			_mockAdvisoryBoardDecisionRepository.Setup(repo => repo.GetTransferProjectDecsion(It.IsAny<int>()))
				.ReturnsAsync(advisoryDecision);
			_mockProjectGroupRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
				.ReturnsAsync(projectGroup);
			_mockAcademiesQueryService.Setup(x => x.GetBulkEstablishmentsByUkprn(It.IsAny<IEnumerable<string>>())).ReturnsAsync(establishments);
			_mockPollyPolicyFactory.Setup(pol => pol.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
			.Returns(Policy.NoOpAsync<HttpResponseMessage>());
			 
			_mockProjectsClient.Setup(client => client.CreateTransferProjectAsync(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(_fixture.Create<ProjectId>());

			_handler = CreateCreateCompleteTransferProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Success sending transfer project to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Exactly(transferProjects.SelectMany(x => x.TransferringAcademies).Count()));

			_mockTransferProjectRepository.Verify(repo => repo.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
			_mockProjectsClient.Verify(client => client.CreateTransferProjectAsync(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(transferringAcademiesCount));
		}
		[Fact]
		public async Task Handle_ConversionProjectsExist_ErrorResponse_ReturnsCommandSuccessResult()
		{
			// Arrange
			var transferProjects = _fixture.CreateMany<ITransferProject>().ToList();
			var establishments = new List<EstablishmentDto>();
			int transferringAcademiesCount = 0;
			foreach (var transferProject in transferProjects)
			{
				Mock.Get(transferProject).Setup(x => x.Id).Returns(_fixture.Create<int>());
				Mock.Get(transferProject).Setup(x => x.SpecificReasonsForTransfer).Returns(new List<string> { "Forced" });
				Mock.Get(transferProject).Setup(x => x.AssignedUserEmailAddress).Returns("test@test.com");
				Mock.Get(transferProject).Setup(x => x.AssignedUserFullName).Returns("TestFirst TestLast");
				Mock.Get(transferProject).Setup(x => x.AssignedUserId).Returns(Guid.NewGuid());
				Mock.Get(transferProject).Setup(x => x.OutgoingTrustUkprn).Returns("111111");
				Mock.Get(transferProject).Setup(x => x.HtbDate).Returns(DateTime.Now);
				Mock.Get(transferProject).Setup(x => x.TargetDateForTransfer).Returns(DateTime.Now);

				var transferringAcademies = _fixture.CreateMany<ITransferringAcademy>().ToList();
				transferringAcademiesCount = transferringAcademiesCount + transferringAcademies.Count;
				foreach (var academy in transferringAcademies)
				{
					Mock.Get(academy).Setup(x => x.TransferProjectId).Returns(transferProject.Id);
					Mock.Get(academy).Setup(x => x.OutgoingAcademyUkprn).Returns(_fixture.Create<string>());
					Mock.Get(academy).Setup(x => x.IncomingTrustUkprn).Returns("101010");
					establishments.Add(new EstablishmentDto { Urn = "101010", Ukprn = academy.OutgoingAcademyUkprn });
				}

				Mock.Get(transferProject).Setup(x => x.TransferringAcademies).Returns(transferringAcademies);
			}

			var command = _fixture.Create<CreateCompleteTransferProjectsCommand>();
			var advisoryDecision = _fixture.Create<ConversionAdvisoryBoardDecision>();
			var errorResponse = _fixture.Create<CreateCompleteProjectErrorResponse>();
			var projectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			var mockContext = new Mock<IUnitOfWork>();

			_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockCompleteTransmissionLogRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_mockTransferProjectRepository.Setup(repo => repo.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transferProjects);
			_mockAdvisoryBoardDecisionRepository.Setup(repo => repo.GetTransferProjectDecsion(It.IsAny<int>()))
				.ReturnsAsync(advisoryDecision);
			_mockProjectGroupRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
				.ReturnsAsync(projectGroup);
			_mockAcademiesQueryService.Setup(x => x.GetBulkEstablishmentsByUkprn(It.IsAny<IEnumerable<string>>())).ReturnsAsync(establishments);
			_mockPollyPolicyFactory.Setup(pol => pol.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
			.Returns(Policy.NoOpAsync<HttpResponseMessage>());

			_mockProjectsClient.Setup(client => client.CreateTransferProjectAsync(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()))
				.Throws(new CompleteApiException(errorResponse.Response!, 400, errorResponse.Response, new Dictionary<string, IEnumerable<string>>(), null));

			_handler = CreateCreateCompleteTransferProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Error,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error sending transfer project to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Exactly(transferProjects.SelectMany(x => x.TransferringAcademies).Count()));

			// Verifying that the GetFormAMatProjectsToSendToCompleteAsync was called once
			_mockTransferProjectRepository.Verify(repo => repo.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
			_mockProjectsClient.Verify(client => client.CreateTransferProjectAsync(It.IsAny<CreateTransferProjectCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(transferringAcademiesCount));
		}
	}
}
