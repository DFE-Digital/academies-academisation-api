using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands
{
	public class ApplicationUpdateCommandTests
	{
		private readonly Fixture _fixture = new();

		private readonly Mock<IApplicationGetDataQuery> _getDataQueryMock = new();
		private readonly Mock<IApplicationUpdateDataCommand> _updateApplicationCommandMock = new();
		private readonly Mock<IApplication> _applicationMock = new();

		[Fact]
		public async Task IdsDoNotMatch___ValidationErrorResultReturned()
		{
			// Arrange
			ApplicationServiceModel applicationServiceModel = _fixture.Create<ApplicationServiceModel>();
			var subject = new ApplicationUpdateCommand(_getDataQueryMock.Object, _updateApplicationCommandMock.Object);

			// Act
			var result = await subject.Execute(applicationServiceModel.ApplicationId+1, applicationServiceModel);

			// Assert
			Assert.IsType<CommandValidationErrorResult>(result);
		}

		[Fact]
		public async Task NotFound___NotFoundResultReturned()
		{
			// Arrange
			ApplicationServiceModel applicationServiceModel = _fixture.Create<ApplicationServiceModel>();
			_getDataQueryMock.Setup(x => x.Execute(applicationServiceModel.ApplicationId)).ReturnsAsync((IApplication?) null);

			var subject = new ApplicationUpdateCommand(_getDataQueryMock.Object, _updateApplicationCommandMock.Object);

			// Act
			var result = await subject.Execute(applicationServiceModel.ApplicationId, applicationServiceModel);

			// Assert
			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public async Task UpdateValid___SuccessResultReturned()
		{
			// Arrange
			Mock<IApplication> applicationMock = new ();
			ApplicationServiceModel applicationServiceModel = _fixture.Create<ApplicationServiceModel>();
			applicationMock.Setup(x => x.Update(
				It.IsAny<ApplicationType>(),
				It.IsAny<ApplicationStatus>(),
				It.IsAny<Dictionary<int, ContributorDetails>>(),
				It.IsAny<Dictionary<int, SchoolDetails>>()
				)).Returns(new CommandSuccessResult());
			_getDataQueryMock.Setup(x => x.Execute(applicationServiceModel.ApplicationId))
				.ReturnsAsync(applicationMock.Object);

			var subject = new ApplicationUpdateCommand(_getDataQueryMock.Object, _updateApplicationCommandMock.Object);

			// Act
			var result = await subject.Execute(applicationServiceModel.ApplicationId, applicationServiceModel);

			// Assert
			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
