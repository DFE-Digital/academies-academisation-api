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
			var request = CreateValidCreateTProjectProjectCommand(false);
			var expectedProjectGroupReference = "GRP_00000000";

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var responseModel = Assert.IsType<CreateSuccessResult<ProjectGroupResponseModel>>(result).Payload;
			Assert.Equal(responseModel.TrustUrn, request.TrustReferenceNumber);
			Assert.Equal(responseModel.Urn, expectedProjectGroupReference);
			Assert.Equal(responseModel.Conversions.Count(), expectedProjects.Count);
			foreach (var conversion in responseModel.Conversions.Select((Value, Index) => (Value, Index)))
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
			var request = CreateValidCreateTProjectProjectCommand();
			var cancellationToken = CancellationToken.None;
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var expectedProjects = _fixture.Create<List<Project>>();
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByUrns(request.ConversionsUrns, It.Is<CancellationToken>(x => x == cancellationToken))).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Project>()));

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			var responseModel = Assert.IsType<CreateSuccessResult<ProjectGroupResponseModel>>(result).Payload;
			Assert.Equal(responseModel.TrustUrn, request.TrustReferenceNumber);
			Assert.Equal(responseModel.Conversions.Count(), expectedProjects.Count);
			foreach (var conversion in responseModel.Conversions.Select((Value, Index) => (Value, Index)))
			{
				Assert.Equal(conversion.Value.Urn, expectedProjects[conversion.Index].Details.Urn);
				Assert.Equal(conversion.Value.SchoolName, expectedProjects[conversion.Index].Details.SchoolName);
			};
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReferenceNumber
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Once());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Exactly(3));
			}

		[Fact]
		public async Task Handle_InValidCommand_ReturnsBadRequest()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			var createProjectGroupCommandHandler = CreateProjectGroupCommandHandler();
			var request = new CreateProjectGroupCommand(string.Empty, string.Empty, [3424]);
			var cancellationToken = CancellationToken.None;

			// Act
			var result = await createProjectGroupCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			var validationError = Assert.IsType<CreateValidationErrorResult>(result);
			validationError.ValidationErrors.Should().HaveCount(2);
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReferenceNumber
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Never());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Never());
		}

		private static CreateProjectGroupCommand CreateValidCreateTProjectProjectCommand(bool includeConversions = true)
		{
			string trustReference = "11112222";
			string trustUkprn = "1111333";

			return new CreateProjectGroupCommand(trustReference, trustUkprn, includeConversions ? [03823] : []);
		}
	}
}
