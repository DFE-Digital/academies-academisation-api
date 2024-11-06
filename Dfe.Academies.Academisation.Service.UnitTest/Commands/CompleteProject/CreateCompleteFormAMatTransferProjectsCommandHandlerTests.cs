using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Commands.CompleteProject;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Polly;
using System.Net;
using System.Reflection;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.CompleteProject
{
	public class CreateCompleteFormAMatTransferProjectsCommandHandlerTests
	{
		private MockRepository mockRepository;
		private Fixture _fixture = new Fixture();

		private Mock<ITransferProjectRepository> _mockTransferProjectRepository;
		private Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository;
		private Mock<IAcademiesQueryService> _mockAcademiesQueryService;
		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private Mock<ICompleteTransmissionLogRepository> _mockCompleteTransmissionLogRepository;
		private Mock<ICompleteApiClientFactory> _mockCompleteApiClientFactory;
		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private ICorrelationContext _correlationContext;
		private Mock<IPollyPolicyFactory> _mockPollyPolicyFactory;
		private Mock<ILogger<CreateCompleteFormAMatTransferProjectsCommandHandler>> _mockLogger;
		private CreateCompleteFormAMatTransferProjectsCommandHandler _handler;

		public CreateCompleteFormAMatTransferProjectsCommandHandlerTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Default);

			this._mockTransferProjectRepository = this.mockRepository.Create<ITransferProjectRepository>();
			this._mockAdvisoryBoardDecisionRepository = this.mockRepository.Create<IAdvisoryBoardDecisionRepository>();
			this._mockAcademiesQueryService = this.mockRepository.Create<IAcademiesQueryService>();
			this._mockProjectGroupRepository = this.mockRepository.Create<IProjectGroupRepository>();
			this._mockCompleteTransmissionLogRepository = this.mockRepository.Create<ICompleteTransmissionLogRepository>();
			this._mockCompleteApiClientFactory = this.mockRepository.Create<ICompleteApiClientFactory>();
			this._mockDateTimeProvider = this.mockRepository.Create<IDateTimeProvider>();
			this._mockPollyPolicyFactory = this.mockRepository.Create<IPollyPolicyFactory>();
			this._mockLogger = this.mockRepository.Create<ILogger<CreateCompleteFormAMatTransferProjectsCommandHandler>>();

			this._correlationContext = new CorrelationContext();
			_correlationContext.SetContext(Guid.NewGuid());

			_fixture.Customize(new AutoMoqCustomization());
		}

		private CreateCompleteFormAMatTransferProjectsCommandHandler CreateCreateCompleteFormAMatTransferProjectsCommandHandler()
		{
			return new CreateCompleteFormAMatTransferProjectsCommandHandler(
				this._mockTransferProjectRepository.Object,
				this._mockAdvisoryBoardDecisionRepository.Object,
				this._mockAcademiesQueryService.Object,
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
		public async Task Handle_NoConversionProjectsFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var command = _fixture.Create<CreateCompleteFormAMatTransferProjectsCommand>();
			_mockTransferProjectRepository.Setup(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync((IEnumerable<ITransferProject>)null);

			var mockClient = new Mock<HttpClient>();
			_mockCompleteApiClientFactory.Setup(factory => factory.Create(It.Is<CorrelationContext>(x => x == _correlationContext)))
				.Returns(mockClient.Object);

			_handler = CreateCreateCompleteFormAMatTransferProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
			// Verifying that the logger was called for the "No transfer projects found" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No transfer projects found.")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

			// Verifying that the GetFormAMatProjectsToSendToCompleteAsync was called once
			_mockTransferProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
		}
		[Fact]
		public async Task Handle_ConversionProjectsExist_SuccessfulResponse_ReturnsCommandSuccessResult()
		{
			// Arrange
			var command = _fixture.Create<CreateCompleteFormAMatTransferProjectsCommand>();
			var transferProjects = _fixture.CreateMany<ITransferProject>().ToList();
			var establishments = new List<EstablishmentDto>();

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

				foreach (var academy in transferringAcademies) {
					Mock.Get(academy).Setup(x => x.TransferProjectId).Returns(transferProject.Id);
					Mock.Get(academy).Setup(x => x.OutgoingAcademyUkprn).Returns(_fixture.Create<string>());
					Mock.Get(academy).Setup(x => x.IncomingTrustName).Returns(_fixture.Create<string>());
					establishments.Add(new EstablishmentDto { Urn = "101010", Ukprn = academy.OutgoingAcademyUkprn });
				}

				Mock.Get(transferProject).Setup(x => x.TransferringAcademies).Returns(transferringAcademies);
			}

			var advisoryDecision = _fixture.Create<ConversionAdvisoryBoardDecision>();		
			var successResponse = _fixture.Create<CreateCompleteConversionProjectSuccessResponse>();
			var projectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			var mockContext = new Mock<IUnitOfWork>();

			_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockCompleteTransmissionLogRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_mockTransferProjectRepository.Setup(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transferProjects);
			_mockAdvisoryBoardDecisionRepository.Setup(repo => repo.GetTransferProjectDecsion(It.IsAny<int>()))
				.ReturnsAsync(advisoryDecision);
			_mockProjectGroupRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
				.ReturnsAsync(projectGroup);
			_mockAcademiesQueryService.Setup(x => x.GetBulkEstablishmentsByUkprn(It.IsAny<IEnumerable<string>>())).ReturnsAsync(establishments);
			_mockPollyPolicyFactory.Setup(pol => pol.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
			.Returns(Policy.NoOpAsync<HttpResponseMessage>());

			var handlerMock = CreateHttpMessageHandlerMock(HttpStatusCode.Created, successResponse);
			var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://localhost") };

			_mockCompleteApiClientFactory.Setup(factory => factory.Create(It.IsAny<CorrelationContext>()))
				.Returns(httpClient);

			_handler = CreateCreateCompleteFormAMatTransferProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Information,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Success sending transfer to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(transferProjects.SelectMany(x => x.TransferringAcademies).Count()));

			_mockTransferProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
		}
		[Fact]
		public async Task Handle_ConversionProjectsExist_ErrorResponse_ReturnsCommandSuccessResult()
		{
			// Arrange
			var transferProjects = _fixture.CreateMany<ITransferProject>().ToList();
			var establishments = new List<EstablishmentDto>();

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

				foreach (var academy in transferringAcademies)
				{
					Mock.Get(academy).Setup(x => x.TransferProjectId).Returns(transferProject.Id);
					Mock.Get(academy).Setup(x => x.OutgoingAcademyUkprn).Returns(_fixture.Create<string>());
					Mock.Get(academy).Setup(x => x.IncomingTrustName).Returns(_fixture.Create<string>());
					establishments.Add(new EstablishmentDto { Urn = "101010", Ukprn = academy.OutgoingAcademyUkprn });
				}

				Mock.Get(transferProject).Setup(x => x.TransferringAcademies).Returns(transferringAcademies);
			}

			var command = _fixture.Create<CreateCompleteFormAMatTransferProjectsCommand>();
			var advisoryDecision = _fixture.Create<ConversionAdvisoryBoardDecision>();
			var errorResponse = _fixture.Create<CreateCompleteProjectErrorResponse>();
			var projectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			var mockContext = new Mock<IUnitOfWork>();

			_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockCompleteTransmissionLogRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_mockTransferProjectRepository.Setup(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transferProjects);
			_mockAdvisoryBoardDecisionRepository.Setup(repo => repo.GetTransferProjectDecsion(It.IsAny<int>()))
				.ReturnsAsync(advisoryDecision);
			_mockProjectGroupRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
				.ReturnsAsync(projectGroup);
			_mockAcademiesQueryService.Setup(x => x.GetBulkEstablishmentsByUkprn(It.IsAny<IEnumerable<string>>())).ReturnsAsync(establishments);
			_mockPollyPolicyFactory.Setup(pol => pol.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
			.Returns(Policy.NoOpAsync<HttpResponseMessage>());

			var handlerMock = CreateHttpMessageHandlerMock(HttpStatusCode.BadRequest, errorResponse);
			var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://localhost") };

			_mockCompleteApiClientFactory.Setup(factory => factory.Create(It.IsAny<CorrelationContext>()))
				.Returns(httpClient);

			_handler = CreateCreateCompleteFormAMatTransferProjectsCommandHandler();

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);
			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			// Verifying that the logger was called for the "Success sending conversion" case
			_mockLogger.Verify(x => x.Log(LogLevel.Error,
				// We're checking for an Information log
				It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error sending transfer project to complete with project urn")),
				// Check if the message contains the expected string
				null,
				// No exception is expected here
				It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(transferProjects.SelectMany(x => x.TransferringAcademies).Count()));

			// Verifying that the GetFormAMatProjectsToSendToCompleteAsync was called once
			_mockTransferProjectRepository.Verify(repo => repo.GetFormAMatProjectsToSendToCompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}

