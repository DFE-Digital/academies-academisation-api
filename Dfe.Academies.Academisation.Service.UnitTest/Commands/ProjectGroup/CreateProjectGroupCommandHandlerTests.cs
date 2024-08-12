using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandlerTests
	{
		private MockRepository _mockRepository;

		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private Mock<ITransferProjectRepository> _mockTransferProjectRepository;
		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private Mock<ILogger<CreateProjectGroupCommandHandler>> _mocklogger;
		private readonly Fixture _fixture = new();
		private CancellationToken _cancellationToken;

		public CreateProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);
			_mockTransferProjectRepository = _mockRepository.Create<ITransferProjectRepository>();
			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_mockConversionProjectRepository = _mockRepository.Create<IConversionProjectRepository>();
			_mocklogger  = new Mock<ILogger<CreateProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
		}
		
		private CreateProjectGroupCommandHandler CreateProjectGroupCommandHandler()
		{
			return new CreateProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mockDateTimeProvider.Object,
				_mockConversionProjectRepository.Object,
				_mocklogger.Object,
				_mockTransferProjectRepository.Object);
		}

		[Fact]
		public async Task Handle_ValidCommandWithoutConversionsAndTransfers_PersistsExpectedProjectGroup()
		{
			// Arrange
			var now = DateTime.Now;
			var expectedProjects = _fixture.Create<List<Project>>()[..0];
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateProjectProjectCommand(false, false);
			var expectedProjectGroupReference = "GRP_00000000";

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var responseModel = Assert.IsType<CreateSuccessResult<ProjectGroupResponseModel>>(result).Payload;
			Assert.Equal(responseModel.TrustReferenceNumber, request.TrustReferenceNumber);
			Assert.Equal(responseModel.ReferenceNumber, expectedProjectGroupReference);
			Assert.Equal(responseModel.Projects.Count(), expectedProjects.Count);
			foreach (var conversion in responseModel.Projects.Select((Value, Index) => (Value, Index)))
			{
				Assert.Equal(conversion.Value.Urn, expectedProjects[conversion.Index].Details.Urn);
				Assert.Equal(conversion.Value.SchoolName, expectedProjects[conversion.Index].Details.SchoolName);
			};
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReferenceNumber
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Once());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Exactly(2));
		}

		[Fact]
		public async Task Handle_ValidCommandWithConversionsAndTransfer_PersistsExpectedProjectGroup()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateProjectProjectCommand();
			var expectedTransferProject = GetTransferProject(request.TrustUkprn);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var expectedProjects = _fixture.Create<List<Project>>();
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectIds(request.ConversionProjectIds, It.Is<CancellationToken>(x => x == _cancellationToken))).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Project>()));
			_mockTransferProjectRepository.Setup(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken))
				.ReturnsAsync([expectedTransferProject]);
			_mockTransferProjectRepository.Setup(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var responseModel = Assert.IsType<CreateSuccessResult<ProjectGroupResponseModel>>(result).Payload;
			Assert.Equal(responseModel.TrustReferenceNumber, request.TrustReferenceNumber);
			Assert.Equal(responseModel.Projects.Count(), expectedProjects.Count);
			Assert.NotEmpty(responseModel.ReferenceNumber!);
			Assert.StartsWith(responseModel.ReferenceNumber, "GRP_00000000");
			foreach (var conversion in responseModel.Projects.Select((Value, Index) => (Value, Index)))
			{
				Assert.Equal(conversion.Value.Urn, expectedProjects[conversion.Index].Details.Urn);
				Assert.Equal(conversion.Value.SchoolName, expectedProjects[conversion.Index].Details.SchoolName);
			};
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReferenceNumber
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Once());
			_mockTransferProjectRepository.Verify(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken), Times.Once);
			_mockTransferProjectRepository.Verify(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Exactly(4));
		}

		private static CreateProjectGroupCommand CreateValidCreateProjectProjectCommand(bool includeConversions = true, bool includeTransfers = true)
		{
			string trustReference = "TR00001";
			string trustUkprn = "1111333";
			string trustName = "Test trust";

			return new CreateProjectGroupCommand(trustName, trustReference, trustUkprn,
				includeConversions ? [03823] : [],
				includeTransfers ? [1234] : []);
		}

		private ITransferProject GetTransferProject(string incomingTrustUkprn)
		{
			var transferAcademy = new TransferringAcademy(incomingTrustUkprn, "in trust", _fixture.Create<string>()[..8], "region", "local authority");
			var transferringAcademies = new List<TransferringAcademy>() { transferAcademy };

			return Domain.TransferProjectAggregate.TransferProject.Create("12345678", "out trust", transferringAcademies, false, DateTime.Now);
		}
	}
}
