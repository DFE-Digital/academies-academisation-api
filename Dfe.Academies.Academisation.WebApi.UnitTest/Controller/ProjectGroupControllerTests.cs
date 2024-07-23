using Dfe.Academies.Academisation.Core;
using System.Threading;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoFixture;
using System.Linq;
using Dfe.Academies.Academisation.IService.Query.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ProjectGroupControllerTests
	{
		private readonly Mock<ILogger<ProjectGroupController>> _loggerMock;
		private readonly Mock<IMediator> _mediatrMock;
		private CancellationToken _cancellationToken;
		private ProjectGroupController _controller;
		private readonly Mock<IProjectGroupQueryService> _projectGroupQueryServiceMock;
		private readonly Fixture _fixture = new();
		private string _trustUrn;
		public ProjectGroupControllerTests()
		{
			_mediatrMock = new Mock<IMediator>();
			_loggerMock = new Mock<ILogger<ProjectGroupController>>();
			_cancellationToken = CancellationToken.None;
			_projectGroupQueryServiceMock = new Mock<IProjectGroupQueryService>();
			_controller = new ProjectGroupController(_mediatrMock.Object, _loggerMock.Object, _projectGroupQueryServiceMock.Object);
			_trustUrn = _fixture.Create<string>()[..8];
		}

		[Fact]
		public async Task CreateProjectGroup_ReturnsOk()
		{
			// Arrange
			var command = new CreateProjectGroupCommand(_trustUrn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CommandSuccessResult());

			// Action
			var result = await _controller.CreateProjectGroup(command, _cancellationToken) as OkResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.OK.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task CreateProjectGroup_ReturnsBadRequest()
		{
			// Arrange
			var command = new CreateProjectGroupCommand(_trustUrn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new BadRequestCommandResult());

			// Action
			var result = await _controller.CreateProjectGroup(command, _cancellationToken) as BadRequestObjectResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.BadRequest.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task SetProjectGroup_ReturnsOk()
		{
			// Arrange
			var urn = "34234233";
			var command = new SetProjectGroupCommand(_trustUrn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CommandSuccessResult());

			// Action
			var result = await _controller.SetProjectGroup(urn, command, _cancellationToken) as OkResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.OK.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}


		[Fact]
		public async Task SetProjectGroup_ReturnsNotFound()
		{
			// Arrange
			var urn = "34234233";
			var command = new SetProjectGroupCommand(_trustUrn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new NotFoundCommandResult());

			// Action
			var result = await _controller.SetProjectGroup(urn, command, _cancellationToken) as NotFoundResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.NotFound.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task SetProjectGroup_ReturnsBadRequest()
		{
			// Arrange
			var urn = "34234233";
			var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();
			var command = new SetProjectGroupCommand(_trustUrn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CommandValidationErrorResult(expectedValidationErrors));

			// Action
			var result = await _controller.SetProjectGroup(urn, command, _cancellationToken);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationErrors, validationErrors);
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task GetProjectGroupByUrn_ReturnsOk()
		{
			// Arrange
			var projectGroupUrn = "34234233";
			var projectGroupResponse = new ProjectGroupServiceModel(_trustUrn);
			_projectGroupQueryServiceMock.Setup(x => x.GetProjectGroupByUrn(projectGroupUrn, _cancellationToken))
				.ReturnsAsync(projectGroupResponse);

			// Action
			var result = await _controller.GetProjectGroupByUrn(projectGroupUrn, _cancellationToken);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result); 
			var response = Assert.IsType<ProjectGroupServiceModel>(okObjectResult.Value);
			Assert.Equal(response.ConversionsUrns.Count, projectGroupResponse.ConversionsUrns.Count);
			Assert.Equal(response.TrustUrn, projectGroupResponse.TrustUrn);
			_projectGroupQueryServiceMock.Verify(x => x.GetProjectGroupByUrn(projectGroupUrn, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task GetProjectGroupByUrn_ReturnsNotFound()
		{
			// Arrange
			var projectGroupUrn = "34234233";
			_projectGroupQueryServiceMock.Setup(x => x.GetProjectGroupByUrn(projectGroupUrn, _cancellationToken))
				.ReturnsAsync((ProjectGroupServiceModel?)null);

			// Action
			var result = await _controller.GetProjectGroupByUrn(projectGroupUrn, _cancellationToken);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
			_projectGroupQueryServiceMock.Verify(x => x.GetProjectGroupByUrn(projectGroupUrn, _cancellationToken), Times.Once());
		}
	}
}
