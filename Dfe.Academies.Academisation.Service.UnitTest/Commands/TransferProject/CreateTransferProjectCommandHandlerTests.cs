using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class CreateTransferProjectCommandHandlerTests
    {
        private MockRepository mockRepository;

        private Mock<ITransferProjectRepository> mockTransferProjectRepository;
        private Mock<IMapper> mockMapper;
        private Mock<IDateTimeProvider> mockDateTimeProvider;

        public CreateTransferProjectCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockTransferProjectRepository = this.mockRepository.Create<ITransferProjectRepository>();
            this.mockDateTimeProvider = this.mockRepository.Create<IDateTimeProvider>();

			var mockContext = new Mock<IUnitOfWork>();
			this.mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
		}

        private CreateTransferProjectCommandHandler CreateCreateTransferProjectCommandHandler()
        {
            return new CreateTransferProjectCommandHandler(
                this.mockTransferProjectRepository.Object,
                this.mockDateTimeProvider.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_PersistsExpectedTransferProject()
        {
			var now = DateTime.Now;
			this.mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			this.mockTransferProjectRepository.Setup(x => x.Insert(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			// Arrange
			var createTransferProjectCommandHandler = this.CreateCreateTransferProjectCommandHandler();
            CreateTransferProjectCommand request = CreateValidTransferProjectCommand();
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await createTransferProjectCommandHandler.Handle(
                request,
                cancellationToken);

			// Assert
			this.mockTransferProjectRepository.Verify(x => x.Insert(It.Is<Domain.TransferProjectAggregate.TransferProject>(x => x.OutgoingTrustUkprn == request.OutgoingTrustUkprn 
			&& x.TransferringAcademies.Count(ta => ta.IncomingTrustUkprn == request.IncomingTrustUkprn && request.TransferringAcademyUkprns.Contains(ta.OutgoingAcademyUkprn)) == request.TransferringAcademyUkprns.Count
			&& x.CreatedOn == now)), Times.Once());

			// called twice to generate urn from database generated field
			this.mockTransferProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Exactly(2));
        }

		[Fact]
		public async Task Handle_ValidCommand_ReturnsCreateCommandSuccessResponse()
		{
			var now = DateTime.Now;
			this.mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			this.mockTransferProjectRepository.Setup(x => x.Insert(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			// Arrange
			var createTransferProjectCommandHandler = this.CreateCreateTransferProjectCommandHandler();
			CreateTransferProjectCommand request = CreateValidTransferProjectCommand();
			CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			result.GetType().Should().Be(typeof(CreateSuccessResult<AcademyTransferProjectResponse>));
			((CreateSuccessResult<AcademyTransferProjectResponse>)result).Payload.OutgoingTrustUkprn.Should().Be(request.OutgoingTrustUkprn);
			((CreateSuccessResult<AcademyTransferProjectResponse>)result).Payload.TransferringAcademies.All(x => x.IncomingTrustUkprn == request.IncomingTrustUkprn && request.TransferringAcademyUkprns.Contains(x.OutgoingAcademyUkprn)).Should().BeTrue();
		}

		private static CreateTransferProjectCommand CreateValidTransferProjectCommand()
		{
			string outgoingTrustUkprn = "11112222";
			string outgoingTrustName = "outgoingTrustName";
			string incomingTrustUkprn = "11110000";
			string incomingTrusName = "incomingTrusName";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };

			return new CreateTransferProjectCommand(outgoingTrustUkprn, outgoingTrustName, incomingTrustUkprn, incomingTrusName, academyUkprns);
		}
	}
}
