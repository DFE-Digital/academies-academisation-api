using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandlerTests
	{
		private MockRepository _mockRepository;

		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;

		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private CreateProjectGroupCommandValidator _validator;
		private Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private Mock<ILogger<CreateProjectGroupCommandHandler>> _mocklogger;
		private readonly Fixture _fixture = new();
		private CancellationToken _cancellationToken;

		public CreateProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);

			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_validator = new CreateProjectGroupCommandValidator();
			_mockConversionProjectRepository = _mockRepository.Create<IConversionProjectRepository>();
			_mocklogger  = new Mock<ILogger<CreateProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object); 
			mockContext.Setup(x => x.BeginTransactionAsync()).ReturnsAsync((new Mock<IDbContextTransaction>()).Object);
			mockContext.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);
			var mockExecuteStrategy = new Mock<IExecutionStrategy>();
			mockContext.Setup(x => x.CreateExecutionStrategy()).Returns(mockExecuteStrategy.Object);
			_cancellationToken = CancellationToken.None;
		}
		
		private CreateProjectGroupCommandHandler CreateProjectGroupCommandHandler()
		{
			return new CreateProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mockDateTimeProvider.Object,
				_validator,
				_mockConversionProjectRepository.Object,
				_mocklogger.Object);
		}

		[Fact]
		public async Task Handle_ValidCommandWithoutConversions_PersistsExpectedProjectGroup()
		{
			// Arrange
			var now = DateTime.Now;
			var expectedProjects = _fixture.Create<List<Project>>();
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateTProjectProjectCommand(false);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupAsync(It.IsAny<List<int>>(), It.Is<CancellationToken>(x => x == _cancellationToken))).ReturnsAsync(expectedProjects);
			var expectedProjectGroupReference = "GRP_00000000";

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var responseModel = Assert.IsType<CreateSuccessResult<ProjectGroupResponseModel>>(result).Payload;
			Assert.Equal(responseModel.TrustUrn, request.TrustUrn);
			Assert.Equal(responseModel.Urn, expectedProjectGroupReference);
			Assert.Equal(responseModel.Conversions.Count(), expectedProjects.Count);
			foreach (var conversion in responseModel.Conversions.Select((Value, Index) => (Value, Index)))
			{
				Assert.Equal(conversion.Value.Urn, expectedProjects[conversion.Index].Details.Urn);
				Assert.Equal(conversion.Value.SchoolName, expectedProjects[conversion.Index].Details.SchoolName);
			};
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustUrn
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Once());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Exactly(2));
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.CommitTransactionAsync(), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsForGroup(request.TrustUrn, It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Never());
			_mockConversionProjectRepository.Verify(x => x.UpdateProjectsWithProjectGroupIdAsync(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Never());
		}

		[Fact]
		public async Task Handle_ValidCommandWithConversions_PersistsExpectedProjectGroup()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateTProjectProjectCommand();
			var cancellationToken = CancellationToken.None;
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var expectedProjects = _fixture.Create<List<Project>>();
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupAsync(It.IsAny<List<int>>(), It.Is<CancellationToken>(x => x == cancellationToken))).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsForGroup(request.TrustUrn, It.Is<CancellationToken>(x => x == cancellationToken))).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.UpdateProjectsWithProjectGroupIdAsync(It.Is<List<int>>(x => x == request.ConversionsUrns), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
			
			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustUrn
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Once());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Exactly(2));
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.CommitTransactionAsync(), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsForGroup(request.TrustUrn, It.Is<CancellationToken>(x => x == cancellationToken)), Times.Exactly(2));
			_mockConversionProjectRepository.Verify(x => x.UpdateProjectsWithProjectGroupIdAsync(It.Is<List<int>>(x => x == request.ConversionsUrns), It.IsAny<int>(), It.IsAny<DateTime>(), It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once());
		}

		[Fact]
		public async Task Handle_InValidCommand_ReturnsBadRequest()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var createProjectGroupCommandHandler = CreateProjectGroupCommandHandler();
			var request = new CreateProjectGroupCommand(string.Empty, [3424]);
			var cancellationToken = CancellationToken.None;

			// Act
			var result = await createProjectGroupCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			var validationError = Assert.IsType<CreateValidationErrorResult>(result);
			validationError.ValidationErrors.Should().HaveCount(2);
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustUrn
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Never());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Never());
		}

		private static CreateProjectGroupCommand CreateValidCreateTProjectProjectCommand(bool includeConversions = true)
		{
			string trustReference = "11112222";

			return new CreateProjectGroupCommand(trustReference, includeConversions ? [03823] : []);
		}
	}
}
