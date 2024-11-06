using AutoFixture.AutoMoq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data.Http;
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
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Polly;
using System.Net;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.CompleteProject
{
    public class CreateCompleteFormAMatConversionProjectsCommandHandlerTests
    {
		private MockRepository mockRepository;
		private Fixture _fixture = new Fixture();

		private Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository;
		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private Mock<ICompleteTransmissionLogRepository> _mockCompleteTransmissionLogRepository;
		private Mock<ICompleteApiClientFactory> _mockCompleteApiClientFactory;
		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private Mock<IPollyPolicyFactory> _mockPollyPolicyFactory;
		private CorrelationContext _correlationContext;
		private Mock<ILogger<CreateCompleteFormAMatConversionProjectsCommandHandler>> _mockLogger;
		private CreateCompleteFormAMatConversionProjectsCommandHandler _handler;

		public CreateCompleteFormAMatConversionProjectsCommandHandlerTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Default);
			_fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });

			this._mockConversionProjectRepository = this.mockRepository.Create<IConversionProjectRepository>();
			this._mockAdvisoryBoardDecisionRepository = this.mockRepository.Create<IAdvisoryBoardDecisionRepository>();
			this._mockProjectGroupRepository = this.mockRepository.Create<IProjectGroupRepository>();
			this._mockCompleteTransmissionLogRepository = this.mockRepository.Create<ICompleteTransmissionLogRepository>();
			this._mockCompleteApiClientFactory = this.mockRepository.Create<ICompleteApiClientFactory>();
			this._mockDateTimeProvider = this.mockRepository.Create<IDateTimeProvider>();
			this._mockPollyPolicyFactory = this.mockRepository.Create<IPollyPolicyFactory>();
			this._correlationContext = new CorrelationContext();
			_correlationContext.SetContext(Guid.NewGuid());
			this._mockLogger = this.mockRepository.Create<ILogger<CreateCompleteFormAMatConversionProjectsCommandHandler>>();
		}

		private CreateCompleteFormAMatConversionProjectsCommandHandler CreateCreateCompleteFormAMatConversionProjectsCommandHandler()
		{
			return new CreateCompleteFormAMatConversionProjectsCommandHandler(
				this._mockConversionProjectRepository.Object,
				this._mockAdvisoryBoardDecisionRepository.Object,
				this._mockProjectGroupRepository.Object,
				this._mockCompleteTransmissionLogRepository.Object,
				this._mockCompleteApiClientFactory.Object,
				this._mockDateTimeProvider.Object,
				this._correlationContext,
				this._mockPollyPolicyFactory.Object,
				this._mockLogger.Object);
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
				.ReturnsAsync((IEnumerable<IProject>)null);

			var mockClient = new Mock<HttpClient>();
			_mockCompleteApiClientFactory.Setup(factory => factory.Create(It.Is<CorrelationContext>(x => x == _correlationContext)))
				.Returns(mockClient.Object);

			_handler = CreateCreateCompleteFormAMatConversionProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
			// Verifying that the logger was called for the "No projects found" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No conversion projects found.")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

			// Verifying that the GetProjectsToSendToCompleteAsync was called once
			_mockConversionProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
		}
		[Fact]
		public async Task Handle_ConversionProjectsExist_SuccessfulResponse_ReturnsCommandSuccessResult()
		{
			// Arrange
			_fixture.Customize<ProjectDetails>(x => x.With(x => x.AssignedUser, new User(Guid.NewGuid(), "TestFirst TestLast", "test@test.com")));

			var command = _fixture.Create<CreateCompleteFormAMatConversionProjectsCommand>();
			var conversionProjects = _fixture.CreateMany<IProject>().ToList();
			var advisoryDecision = _fixture.Create<ConversionAdvisoryBoardDecision>();
			var successResponse = _fixture.Create<CreateCompleteConversionProjectSuccessResponse>();
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

			var handlerMock = CreateHttpMessageHandlerMock(HttpStatusCode.Created, successResponse);
			var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://localhost") };

			_mockCompleteApiClientFactory.Setup(factory => factory.Create(It.IsAny<CorrelationContext>()))
				.Returns(httpClient);

			_handler = CreateCreateCompleteFormAMatConversionProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Success sending conversion project to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(conversionProjects.Count));
			_mockConversionProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
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

			var handlerMock = CreateHttpMessageHandlerMock(HttpStatusCode.BadRequest, errorResponse);
			var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://localhost") };

			_mockCompleteApiClientFactory.Setup(factory => factory.Create(It.IsAny<CorrelationContext>()))
				.Returns(httpClient);

			_handler = CreateCreateCompleteFormAMatConversionProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Error,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error sending conversion project to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(conversionProjects.Count));
			_mockConversionProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}

