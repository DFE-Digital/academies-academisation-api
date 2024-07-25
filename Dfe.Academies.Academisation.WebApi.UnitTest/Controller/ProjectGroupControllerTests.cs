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
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Azure;

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
			var response = new ProjectGroupResponseModel("12312", _trustUrn, []);
			var command = new CreateProjectGroupCommand(_trustUrn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CreateSuccessResult<ProjectGroupResponseModel>(response));

			// Action
			var result = await _controller.CreateProjectGroup(command, _cancellationToken);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var responseModel = Assert.IsType<ProjectGroupResponseModel>(result.Value);
			Assert.Equal(responseModel.Urn, response.Urn);
			Assert.Equal(responseModel.TrustUrn, response.TrustUrn);
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task CreateProjectGroup_ReturnsBadRequest()
		{
			// Arrange
			var command = new CreateProjectGroupCommand(_trustUrn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CreateValidationErrorResult([new ValidationError("ConversionsUrns", "Validation Error")]));

			// Action
			var result = await _controller.CreateProjectGroup(command, _cancellationToken);

			// Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
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
		public async Task GetProjectGroups_ReturnsOk()
		{
			// Arrange 
			var searchModel = new ProjectGroupSearchModel(1, 10, "34234233", null, null, null, null, null);
			var projectGroupResponse = new List<ProjectGroupResponseModel> { new(searchModel.Urn!, _trustUrn, []) };
			var pagingResponse = new PagedDataResponse<ProjectGroupResponseModel>(projectGroupResponse, new PagingResponse()
			{
				Page = 1,
				RecordCount = 1
			});
			_projectGroupQueryServiceMock.Setup(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.Urn == searchModel.Urn), _cancellationToken))
				.ReturnsAsync(pagingResponse);

			// Action
			var result = await _controller.GetProjectGroups(searchModel, _cancellationToken);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result); 
			var response = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(okObjectResult.Value);
			Assert.Equal(response.Data.Count(), projectGroupResponse.Count);
			Assert.Equal(response.Paging.RecordCount, pagingResponse.Paging.RecordCount);
			Assert.Equal(response.Paging.Page, pagingResponse.Paging.Page);
			_projectGroupQueryServiceMock.Verify(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.Urn == searchModel.Urn), _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task GetProjectGroups_ReturnsNotFound()
		{
			// Arrange
			var searchModel = new ProjectGroupSearchModel(1, 10, "34234233", null, null, null, null, null);
			var pagingResponse = new PagedDataResponse<ProjectGroupResponseModel>([], new PagingResponse()
			{
				Page = 1,
				RecordCount = 1
			});
			_projectGroupQueryServiceMock.Setup(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.Urn == searchModel.Urn), _cancellationToken))
				.ReturnsAsync(pagingResponse);

			// Action
			var result = await _controller.GetProjectGroups(searchModel, _cancellationToken);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
			_projectGroupQueryServiceMock.Verify(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.Urn == searchModel.Urn), _cancellationToken), Times.Once());
		}
	}
}
