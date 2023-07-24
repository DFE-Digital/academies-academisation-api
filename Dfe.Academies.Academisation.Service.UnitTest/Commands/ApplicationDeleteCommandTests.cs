using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands
{
	public class ApplicationDeleteCommandTests
	{
		private readonly Fixture _fixture = new();

		private readonly Mock<IApplication> _applicationMock = new();


		private readonly int _applicationId;

		private readonly Mock<IApplicationRepository> _repo = new();

		private readonly Service.Commands.Application.ApplicationDeleteCommandHandler _subject;

		public ApplicationDeleteCommandTests()

		{
			_applicationId = _fixture.Create<int>();
			_subject = new Service.Commands.Application.ApplicationDeleteCommandHandler(_repo.Object);
			_repo.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
		}

		[Fact]
		public async Task NotFound___NotPassedToDataLayer_NotFoundReturned()
		{
			// arrange
			int notFoundApplicationId = _fixture.Create<int>();
			_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
			_repo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);
			_applicationMock.Setup(m => m.ValidateSoftDelete(notFoundApplicationId))
				.Returns(new NotFoundCommandResult());

			// act
			var result = await _subject.Handle(new ApplicationDeleteCommand(notFoundApplicationId), default(CancellationToken));

			// assert
			Assert.IsType<NotFoundCommandResult>(result);
			_repo.Verify(x => x.Update(It.IsAny<Application>()), Times.Never);
		}

		[Fact]
		public async Task ApplicationDeleted___PassedToDataLayer_CreateSuccessReturned()
		{
			// arrange
			_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
			_repo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);
			_applicationMock.Setup(m => m.ValidateSoftDelete(_applicationMock.Object.ApplicationId))
				.Returns(new CommandSuccessResult());

			// act
			var result = await _subject.Handle(new ApplicationDeleteCommand(_applicationId), default(CancellationToken));

			// assert
			Assert.IsType<CommandSuccessResult>(result);
			_repo.Verify(x => x.Update(It.IsAny<Application>()), Times.Never);
		}
	}

}

