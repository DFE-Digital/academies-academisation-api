using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange
{
	public class CreateSignificantProjectCommandHandlerTests
	{
		private readonly MockRepository _mockRepository;
		private readonly Mock<ISignificantChangeProjectRepository> _mockSignificantChangeProjectRepository;
		private readonly Mock<IAcademiesQueryService> _mockAcademiesQueryService;
		private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
		private readonly Mock<ILogger<CreateSignificantProjectCommandHandler>> _mockLogger;

		private static readonly string trustUkprn = "12345678";
		private static readonly string trustName = "a trust name";

		public CreateSignificantProjectCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Default);

			_mockSignificantChangeProjectRepository = _mockRepository.Create<ISignificantChangeProjectRepository>();
			_mockAcademiesQueryService = _mockRepository.Create<IAcademiesQueryService>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_mockLogger = _mockRepository.Create<ILogger<CreateSignificantProjectCommandHandler>>();

			_mockAcademiesQueryService.Setup(x => x.GetTrust(trustUkprn))
				.ReturnsAsync(new TrustDto() { Name = trustName });

			var mockContext = new Mock<IUnitOfWork>();
			_mockSignificantChangeProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
		}

		private CreateSignificantProjectCommandHandler CreateHandler()
		{
			return new CreateSignificantProjectCommandHandler(
				_mockSignificantChangeProjectRepository.Object,
				_mockAcademiesQueryService.Object,
				_mockDateTimeProvider.Object,
				_mockLogger.Object);
		}

		[Fact]
		public async Task Handle_ValidCommand_PersistsExpectedSignificantProject()
		{
			_mockSignificantChangeProjectRepository
				.Setup(x => x.Insert(It.IsAny<SignificantChangeProject>()));

			var handler = CreateHandler();
			var request = CreateValidCommand();
			CancellationToken cancellationToken = default;

			var result = await handler.Handle(request, cancellationToken);

			result.Should().BeOfType<CreateSuccessResult<SignificantChangeProjectResponse>>();

			_mockSignificantChangeProjectRepository.Verify(x => x.Insert(It.Is<SignificantChangeProject>(p =>
				p.Urn == request.Urn &&
				p.Tier == request.Tier &&
				p.TrustName == trustName &&
				p.TrustUkprn == request.TrustUkprn &&
				p.TypeOfSignificantChange == request.Route &&
				p.Status == SignificantChangeStatus.PreDecision
			)), Times.Once);

			_mockAcademiesQueryService.Verify(x => x.GetTrust(request.TrustUkprn), Times.Once);

			_mockSignificantChangeProjectRepository.Verify(x =>
				x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(c => c == cancellationToken)), Times.Once);
		}

		[Fact]
		public async Task Handle_ValidCommand_ReturnsCreateSuccessResponse()
		{
			_mockSignificantChangeProjectRepository
				.Setup(x => x.Insert(It.IsAny<SignificantChangeProject>()));

			var handler = CreateHandler();
			var request = CreateValidCommand();
			CancellationToken cancellationToken = default;

			var result = await handler.Handle(request, cancellationToken);

			result.Should().BeOfType<CreateSuccessResult<SignificantChangeProjectResponse>>();

			var payload = ((CreateSuccessResult<SignificantChangeProjectResponse>)result).Payload;
			payload.Urn.Should().Be(request.Urn);
			payload.Tier.Should().Be(request.Tier);
			payload.TrustName.Should().Be(trustName);
			payload.TrustUkprn.Should().Be(request.TrustUkprn);
			payload.TypeOfSignificantChange.Should().Be(request.Route);
			payload.Status.Should().Be(nameof(SignificantChangeStatus.PreDecision));
		}

		[Fact]
		public async Task Handle_TrustNotFound_LogsWarning()
		{
			_mockAcademiesQueryService.Setup(x => x.GetTrust(trustUkprn))
				.ReturnsAsync((TrustDto?)null);

			var handler = CreateHandler();
			var request = CreateValidCommand();
			CancellationToken cancellationToken = default;
			var result = await handler.Handle(request, cancellationToken);

			_mockLogger.Verify(
				x => x.Log(
					LogLevel.Warning,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Trust with UKPRN {trustUkprn} not found")),
					null,
					It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
				Times.Once);

			result.Should().BeOfType<CreateSuccessResult<SignificantChangeProjectResponse>>();

			_mockSignificantChangeProjectRepository.Verify(x => x.Insert(It.Is<SignificantChangeProject>(p =>
				p.Urn == request.Urn &&
				p.Tier == request.Tier &&
				p.TrustName == string.Empty &&
				p.TrustUkprn == request.TrustUkprn &&
				p.TypeOfSignificantChange == request.Route &&
				p.Status == SignificantChangeStatus.PreDecision
			)), Times.Once);


			_mockSignificantChangeProjectRepository.Verify(x =>
				x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(c => c == cancellationToken)), Times.Once);
		}

		private static CreateSignificantProjectCommand CreateValidCommand()
		{
			return new CreateSignificantProjectCommand(
				Urn: 123456,
				Tier: 2,
				Route: "Change of age range",
				TrustUkprn: trustUkprn);
		}
	}
}
