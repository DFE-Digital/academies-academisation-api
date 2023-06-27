using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectAggregate
{
	public class TestProjectMaintainTests
	{

		private readonly Mock<IMaintainTestProjectCommand> _maintainTestProjectCommand;


		public TestProjectMaintainTests()
		{
			_maintainTestProjectCommand = new Mock<IMaintainTestProjectCommand>();
		}

		private TestController System_under_test()
		{
			return new TestController(_maintainTestProjectCommand.Object);
		}
		[Fact]
		public async Task Should_return_200_when_exists_or_is_created_and_returned()
		{
			// Setup
			_maintainTestProjectCommand
				.Setup(x => x.Execute())
				.Returns(Task.FromResult(new CreateSuccessResult<TestProjectModel>(new TestProjectModel(10, 10))));

			TestController controller = System_under_test();

			// Act
			var actionResult = await controller.GetTestProject();

			// Assert
			actionResult.Result.Should().BeOfType<OkObjectResult>();

			var okResult = actionResult.Result as OkObjectResult;
			okResult.StatusCode.Should().Be(200);  // If you want to check HTTP status

			var resultValue = okResult.Value as CreateSuccessResult<TestProjectModel>;
			var model = resultValue.Payload;

			model.Id.Should().Be(10);
			model.Urn.Should().Be(10);
		}

	}
}
