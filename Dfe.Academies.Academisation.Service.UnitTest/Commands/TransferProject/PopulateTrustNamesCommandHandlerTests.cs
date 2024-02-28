using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Contracts.V4.Trusts;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
    public class PopulateTrustNamesCommandHandlerTests
    {
        private MockRepository mockRepository;

        private Mock<ITransferProjectRepository> mockTransferProjectRepository;
        private Mock<IAcademiesQueryService> mockAcademiesQueryService;
        private Mock<ILogger<AssignTransferProjectUserCommandHandler>> mockLogger;

        public PopulateTrustNamesCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockTransferProjectRepository = this.mockRepository.Create<ITransferProjectRepository>();
			var mockContext = new Mock<IUnitOfWork>();
			mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			this.mockAcademiesQueryService = this.mockRepository.Create<IAcademiesQueryService>();
            this.mockLogger = this.mockRepository.Create<ILogger<AssignTransferProjectUserCommandHandler>>();
        }

        private PopulateTrustNamesCommandHandler CreatePopulateTrustNamesCommandHandler()
        {
            return new PopulateTrustNamesCommandHandler(
                this.mockTransferProjectRepository.Object,
                this.mockAcademiesQueryService.Object,
                this.mockLogger.Object);
        }

        [Fact]
        public async Task Handle_TransferProjectsFound_ReturnsCommandSuccessResult()
        {
			// Arrange
			var fixture = new Fixture();
			fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
			TrustDto? trust = fixture.Create<TrustDto>();
			var tp = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "out trust", "23456789", "in trust", new List<string> { "34567890" }, DateTime.Now);
			var transferProjects = new List<ITransferProject>() { tp };

			this.mockTransferProjectRepository.Setup(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));
			this.mockAcademiesQueryService.Setup(p => p.GetTrust(It.IsAny<string>())).Returns(Task.FromResult(trust));
			this.mockTransferProjectRepository.Setup(p => p.GetAllTransferProjectsWhereTrustNameIsNull()).Returns(Task.FromResult(transferProjects.AsEnumerable()));

			var populateTrustNamesCommandHandler = this.CreatePopulateTrustNamesCommandHandler();
            PopulateTrustNamesCommand request = new PopulateTrustNamesCommand();
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await populateTrustNamesCommandHandler.Handle(
                request,
                cancellationToken);

			// Assert
			result.Should().BeOfType<CommandSuccessResult>();

			this.mockTransferProjectRepository.Verify(x => x.Update(It.Is<Domain.TransferProjectAggregate.TransferProject>(x =>
						x.OutgoingTrustName == trust.Name && x.TransferringAcademies.All(ta => ta.IncomingTrustName == trust.Name))), Times.Exactly(transferProjects.Count()));

			// called twice to generate urn from database generated field
			this.mockTransferProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once());
		}
    }
}
