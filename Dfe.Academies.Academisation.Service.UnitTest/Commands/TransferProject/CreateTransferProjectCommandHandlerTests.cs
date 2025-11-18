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
        private readonly MockRepository mockRepository;
        private readonly Mock<ITransferProjectRepository> mockTransferProjectRepository;
		private readonly Mock<IDateTimeProvider> mockDateTimeProvider;

		public CreateTransferProjectCommandHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockTransferProjectRepository = mockRepository.Create<ITransferProjectRepository>();
            mockDateTimeProvider = mockRepository.Create<IDateTimeProvider>();

			var mockContext = new Mock<IUnitOfWork>();
			mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
		}

        private CreateTransferProjectCommandHandler CreateCreateTransferProjectCommandHandler()
        {
            return new CreateTransferProjectCommandHandler(
                mockTransferProjectRepository.Object,
                mockDateTimeProvider.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_PersistsExpectedTransferProject()
        {
			var now = DateTime.Now;
			mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			mockTransferProjectRepository.Setup(x => x.Insert(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			// Arrange
			var createTransferProjectCommandHandler = CreateCreateTransferProjectCommandHandler();
            CreateTransferProjectCommand request = CreateValidTransferProjectCommand();
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await createTransferProjectCommandHandler.Handle(
                request,
                cancellationToken);

			// Assert
			mockTransferProjectRepository.Verify(x => x.Insert(It.Is<Domain.TransferProjectAggregate.TransferProject>(x => x.OutgoingTrustUkprn == request.OutgoingTrustUkprn 
			&& x.TransferringAcademies.Count == request.TransferringAcademies.Count
			&& x.CreatedOn == now)), Times.Once());

			// called twice to generate urn from database generated field
			mockTransferProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Exactly(2));
        }

		[Fact]
		public async Task Handle_ValidCommand_ReturnsCreateCommandSuccessResponse()
		{
			var now = DateTime.Now;
			mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			mockTransferProjectRepository.Setup(x => x.Insert(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			// Arrange
			var createTransferProjectCommandHandler = CreateCreateTransferProjectCommandHandler();
			CreateTransferProjectCommand request = CreateValidTransferProjectCommand();
			CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			result.GetType().Should().Be(typeof(CreateSuccessResult<AcademyTransferProjectResponse>));
			((CreateSuccessResult<AcademyTransferProjectResponse>)result).Payload.OutgoingTrustUkprn.Should().Be(request.OutgoingTrustUkprn);
			((CreateSuccessResult<AcademyTransferProjectResponse>)result).Payload.TransferringAcademies.All(x => {
				var transferringAcademy = request.TransferringAcademies.Single(ta => ta.OutgoingAcademyUkprn == x.OutgoingAcademyUkprn);
				return transferringAcademy.IncomingTrustUkprn!.Equals(x.IncomingTrustUkprn) && transferringAcademy.IncomingTrustName!.Equals(x.IncomingTrustName);
			}).Should().BeTrue();
		}

		[Fact]
		public async Task Handle_ValidCommandCustomRef_ReturnsCreateCommandSuccessResponse()
		{
			var now = DateTime.Now;
			mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			mockTransferProjectRepository.Setup(x => x.Insert(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			// Arrange
			var createTransferProjectCommandHandler = CreateCreateTransferProjectCommandHandler();
			CreateTransferProjectCommand request = CreateValidTransferProjectCommand();
			request.Reference = "CustomRef123";
			CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			result.GetType().Should().Be(typeof(CreateSuccessResult<AcademyTransferProjectResponse>));
			((CreateSuccessResult<AcademyTransferProjectResponse>)result).Payload.OutgoingTrustUkprn.Should().Be(request.OutgoingTrustUkprn);
			((CreateSuccessResult<AcademyTransferProjectResponse>)result).Payload.TransferringAcademies.All(x => {
				var transferringAcademy = request.TransferringAcademies.Single(ta => ta.OutgoingAcademyUkprn == x.OutgoingAcademyUkprn);
				return transferringAcademy.IncomingTrustUkprn!.Equals(x.IncomingTrustUkprn) && transferringAcademy.IncomingTrustName!.Equals(x.IncomingTrustName);
			}).Should().BeTrue();
			((CreateSuccessResult<AcademyTransferProjectResponse>)result).Payload.ProjectReference.Should().Be(request.Reference);
		}

		private static CreateTransferProjectCommand CreateValidTransferProjectCommand()
		{
			string outgoingTrustUkprn = "11112222";
			string outgoingTrustName = "outgoingTrustName";
			string incomingTrustUkprn = "11110000";
			string incomingTrusName = "incomingTrusName";

			bool isFormAMat = true;
			var transferringAcademies = new List<TransferringAcademyDto>() { 
				new TransferringAcademyDto(){ IncomingTrustUkprn = incomingTrustUkprn, IncomingTrustName = incomingTrusName, OutgoingAcademyUkprn = "22221111" },
				new TransferringAcademyDto(){ IncomingTrustUkprn = incomingTrustUkprn, IncomingTrustName = incomingTrusName, OutgoingAcademyUkprn = "33331111" },
			};

			return new CreateTransferProjectCommand(outgoingTrustUkprn, outgoingTrustName, transferringAcademies, isFormAMat);
		}
	}
}
