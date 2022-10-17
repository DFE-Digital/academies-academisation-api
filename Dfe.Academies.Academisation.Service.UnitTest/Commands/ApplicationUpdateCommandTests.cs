using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands
{
	public class ApplicationUpdateCommandTests
	{
		private readonly Fixture _fixture = new();

		private readonly Mock<IApplicationGetDataQuery> _getDataQueryMock = new();
		private readonly Mock<IApplicationUpdateDataCommand> _updateApplicationCommandMock = new();
		private readonly ApplicationUpdateCommand _subject;

		public ApplicationUpdateCommandTests()
		{
			_subject = new ApplicationUpdateCommand(_getDataQueryMock.Object, _updateApplicationCommandMock.Object);
		}

		[Fact]
		public async Task IdsDoNotMatch___ValidationErrorResultReturned()
		{
			// Arrange
			ApplicationUpdateRequestModel applicationServiceModel = _fixture.Create<ApplicationUpdateRequestModel>();

			// Act
			var result = await _subject.Execute(applicationServiceModel.ApplicationId + 1, applicationServiceModel);

			// Assert
			Assert.IsType<CommandValidationErrorResult>(result);
		}

		[Fact]
		public async Task NotFound___NotFoundResultReturned()
		{
			// Arrange
			ApplicationUpdateRequestModel applicationServiceModel = _fixture.Create<ApplicationUpdateRequestModel>();
			_getDataQueryMock.Setup(x => x.Execute(applicationServiceModel.ApplicationId)).ReturnsAsync((IApplication?)null);

			// Act
			var result = await _subject.Execute(applicationServiceModel.ApplicationId, applicationServiceModel);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public async Task UpdateValid___SuccessResultReturned()
		{
			// Arrange
			Mock<IApplication> applicationMock = new();
			ApplicationUpdateRequestModel applicationServiceModel = _fixture.Create<ApplicationUpdateRequestModel>();
			applicationMock.Setup(x => x.Update(
				It.IsAny<ApplicationType>(),
				It.IsAny<ApplicationStatus>(),
				It.IsAny<IEnumerable<KeyValuePair<int, ContributorDetails>>>(),
				It.IsAny<IEnumerable<UpdateSchoolParameter>>()
				)).Returns(new CommandSuccessResult());
			_getDataQueryMock.Setup(x => x.Execute(applicationServiceModel.ApplicationId))
				.ReturnsAsync(applicationMock.Object);

			// Act
			var result = await _subject.Execute(applicationServiceModel.ApplicationId, applicationServiceModel);

			// Assert
			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
