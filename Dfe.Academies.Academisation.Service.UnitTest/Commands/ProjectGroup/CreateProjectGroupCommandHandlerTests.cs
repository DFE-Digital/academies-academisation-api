using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
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

		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private Mock<ILogger<CreateProjectGroupCommandHandler>> _mocklogger;
		private readonly Fixture _fixture = new();
		private CancellationToken _cancellationToken;

		public CreateProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);

			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_mockConversionProjectRepository = _mockRepository.Create<IConversionProjectRepository>();
			_mocklogger  = new Mock<ILogger<CreateProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
		}
		
		private CreateProjectGroupCommandHandler CreateProjectGroupCommandHandler()
		{
			return new CreateProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mockDateTimeProvider.Object,
				_mockConversionProjectRepository.Object,
				_mocklogger.Object);
		}

		[Fact]
		public async Task Handle_ValidCommandWithoutConversions_PersistsExpectedProjectGroup()
		{
			// Arrange
			var now = DateTime.Now;
			var expectedProjects = _fixture.Create<List<Project>>()[..0];
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateProjectProjectCommand(false);
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
		public async Task Handle_ValidCommandWithConversions_PersistsExpectedProjectGroup()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateProjectProjectCommand();
			var cancellationToken = CancellationToken.None;
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var expectedProjects = _fixture.Create<List<Project>>();
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectIds(request.ConversionProjectIds, It.Is<CancellationToken>(x => x == cancellationToken))).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Project>()));

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

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

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Exactly(3));
		}

		private static CreateProjectGroupCommand CreateValidCreateProjectProjectCommand(bool includeConversions = true)
		{
			string trustReference = "TR00001";
			string trustUkprn = "1111333";
			string trustName = "Test trust";

			return new CreateProjectGroupCommand(trustName, trustReference, trustUkprn, includeConversions ? [03823] : []);
		}
	}
}
