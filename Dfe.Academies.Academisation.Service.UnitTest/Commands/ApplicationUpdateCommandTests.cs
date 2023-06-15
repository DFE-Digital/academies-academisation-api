using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands
{
	public class ApplicationUpdateCommandTests
	{
		private readonly Fixture _fixture = new();

		private readonly Mock<IApplicationRepository> _repo = new();
		private readonly Service.Commands.Application.ApplicationUpdateCommandHandler _subject;

		public ApplicationUpdateCommandTests()
		{
			_subject = new Service.Commands.Application.ApplicationUpdateCommandHandler(_repo.Object);
			_repo.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
		}

		[Fact]
		public async Task NotFound___NotFoundResultReturned()
		{
			// Arrange
			ApplicationUpdateCommand applicationServiceModel = _fixture.Create<ApplicationUpdateCommand>();
			_repo.Setup(x => x.GetByIdAsync(applicationServiceModel.ApplicationId)).ReturnsAsync((Application?)null);

			// Act
			var result = await _subject.Handle(applicationServiceModel, default);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public async Task UpdateValid___SuccessResultReturned()
		{
			// Arrange
			Mock<IApplication> applicationMock = new();
			ApplicationUpdateCommand applicationServiceModel = _fixture.Create<ApplicationUpdateCommand>();

			applicationMock.Setup(x => x.Update(
				It.IsAny<ApplicationType>(),
				It.IsAny<ApplicationStatus>(),
				It.IsAny<IEnumerable<KeyValuePair<int, ContributorDetails>>>(),
				It.IsAny<IEnumerable<UpdateSchoolParameter>>()
				)).Returns(new CommandSuccessResult());

			_repo.Setup(x => x.GetByIdAsync(applicationServiceModel.ApplicationId))
				.ReturnsAsync(applicationMock.Object);

			// Act
			var result = await _subject.Handle(applicationServiceModel, default);

			// Assert
			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
