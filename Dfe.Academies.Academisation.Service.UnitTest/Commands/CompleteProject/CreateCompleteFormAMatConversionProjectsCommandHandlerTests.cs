using AutoFixture.AutoMoq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Commands.CompleteProject;
using Dfe.Academies.Academisation.Service.Factories;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Polly;
using System.Net;
using Xunit;
using Dfe.Complete.Client.Contracts;
using User = Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.User;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.CompleteProject
{
    public class CreateCompleteFormAMatConversionProjectsCommandHandlerTests
    { 
		private readonly Fixture _fixture = new();

		private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private readonly Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository;
		private readonly Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private readonly Mock<ICompleteTransmissionLogRepository> _mockCompleteTransmissionLogRepository; 
		private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
		private readonly Mock<IPollyPolicyFactory> _mockPollyPolicyFactory; 
		private readonly Mock<ILogger<CreateCompleteFormAMatConversionProjectsCommandHandler>> _mockLogger;
		private CreateCompleteFormAMatConversionProjectsCommandHandler? _handler;
		private readonly Mock<IProjectsClient> _mockProjectsClient;

		public CreateCompleteFormAMatConversionProjectsCommandHandlerTests()
		{
			var mockRepository = new MockRepository(MockBehavior.Default);
			_fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });

			_mockConversionProjectRepository = mockRepository.Create<IConversionProjectRepository>();
			_mockAdvisoryBoardDecisionRepository = mockRepository.Create<IAdvisoryBoardDecisionRepository>();
			_mockProjectGroupRepository = mockRepository.Create<IProjectGroupRepository>();
			_mockCompleteTransmissionLogRepository = mockRepository.Create<ICompleteTransmissionLogRepository>(); 
			_mockDateTimeProvider = mockRepository.Create<IDateTimeProvider>();
			_mockPollyPolicyFactory = mockRepository.Create<IPollyPolicyFactory>(); 
			_mockLogger = mockRepository.Create<ILogger<CreateCompleteFormAMatConversionProjectsCommandHandler>>();
			_mockProjectsClient = mockRepository.Create<IProjectsClient>();
		}

		private CreateCompleteFormAMatConversionProjectsCommandHandler CreateCreateCompleteFormAMatConversionProjectsCommandHandler()
		{
			return new CreateCompleteFormAMatConversionProjectsCommandHandler(
				_mockConversionProjectRepository.Object,
				_mockAdvisoryBoardDecisionRepository.Object,
				_mockProjectGroupRepository.Object,
				_mockCompleteTransmissionLogRepository.Object,
				_mockDateTimeProvider.Object,
				_mockPollyPolicyFactory.Object,
				_mockLogger.Object,
				_mockProjectsClient.Object);
		}

		public static Mock<HttpMessageHandler> CreateHttpMessageHandlerMock(HttpStatusCode code, object content)
		{
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(() => {
					return new HttpResponseMessage(code)
					{
						Content = new StringContent(JsonConvert.SerializeObject(content))
					};
				});
			return handlerMock;
		}

		[Fact]
		public async Task Handle_NoConversionProjectsFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var command = _fixture.Create<CreateCompleteFormAMatConversionProjectsCommand>();
			_mockConversionProjectRepository.Setup(repo => repo.GetProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync((IEnumerable<IProject>)null!);
 

			_handler = CreateCreateCompleteFormAMatConversionProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
			// Verifying that the logger was called for the "No projects found" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No conversion projects found.")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

			// Verifying that the GetProjectsToSendToCompleteAsync was called once
			_mockConversionProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
			_mockProjectsClient.Verify(client => client.CreateConversionMatProjectAsync(It.IsAny<CreateConversionMatProjectCommand>(), It.IsAny<CancellationToken>()), Times.Never());
		}
		[Fact]
		public async Task Handle_ConversionProjectsExist_SuccessfulResponse_ReturnsCommandSuccessResult()
		{
			// Arrange
			_fixture.Customize<ProjectDetails>(x => x.With(x => x.AssignedUser, new User(Guid.NewGuid(), "TestFirst TestLast", "test@test.com")));

			var command = _fixture.Create<CreateCompleteFormAMatConversionProjectsCommand>();
			var conversionProjects = _fixture.CreateMany<IProject>().ToList();
			var advisoryDecision = _fixture.Create<ConversionAdvisoryBoardDecision>(); 
			var projectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			var mockContext = new Mock<IUnitOfWork>();

			_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockCompleteTransmissionLogRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_mockConversionProjectRepository.Setup(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(conversionProjects);
			_mockAdvisoryBoardDecisionRepository.Setup(repo => repo.GetConversionProjectDecsion(It.IsAny<int>()))
				.ReturnsAsync(advisoryDecision);
			_mockProjectGroupRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
				.ReturnsAsync(projectGroup);
			_mockPollyPolicyFactory.Setup(pol => pol.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
					.Returns(Policy.NoOpAsync<HttpResponseMessage>());
			 
			_mockProjectsClient.Setup(client => client.CreateConversionMatProjectAsync(It.IsAny<CreateConversionMatProjectCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(_fixture.Create<ProjectId>());

			_handler = CreateCreateCompleteFormAMatConversionProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Success sending conversion project to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Exactly(conversionProjects.Count));
			_mockConversionProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
			_mockProjectsClient.Verify(client => client.CreateConversionMatProjectAsync(It.IsAny<CreateConversionMatProjectCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(conversionProjects.Count));
		}
		[Fact]
		public async Task Handle_ConversionProjectsExist_ErrorResponse_ReturnsCommandSuccessResult()
		{
			// Arrange
			_fixture.Customize<ProjectDetails>(x => x.With(x => x.AssignedUser, new User(Guid.NewGuid(), "TestFirst TestLast", "test@test.com")));

			var command = _fixture.Create<CreateCompleteFormAMatConversionProjectsCommand>();
			var conversionProjects = _fixture.CreateMany<IProject>().ToList();
			var advisoryDecision = _fixture.Create<ConversionAdvisoryBoardDecision>();
			var errorResponse = _fixture.Create<CreateCompleteProjectErrorResponse>();
			var projectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			var mockContext = new Mock<IUnitOfWork>();

			_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockCompleteTransmissionLogRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_mockConversionProjectRepository.Setup(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(conversionProjects);
			_mockAdvisoryBoardDecisionRepository.Setup(repo => repo.GetConversionProjectDecsion(It.IsAny<int>()))
				.ReturnsAsync(advisoryDecision);
			_mockProjectGroupRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
				.ReturnsAsync(projectGroup);
			_mockPollyPolicyFactory.Setup(pol => pol.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
					.Returns(Policy.NoOpAsync<HttpResponseMessage>());

			_mockProjectsClient.Setup(client => client.CreateConversionMatProjectAsync(It.IsAny<CreateConversionMatProjectCommand>(), It.IsAny<CancellationToken>()))
				.Throws(new CompleteApiException(errorResponse.Response!, 400, errorResponse.Response, new Dictionary<string, IEnumerable<string>>(), null));

			_handler = CreateCreateCompleteFormAMatConversionProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Error,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error sending conversion project to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Exactly(conversionProjects.Count));
			_mockProjectsClient.Verify(client => client.CreateConversionMatProjectAsync(It.IsAny<CreateConversionMatProjectCommand>(), It.IsAny<CancellationToken>()),
				Times.Exactly(conversionProjects.Count));
			_mockConversionProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}

