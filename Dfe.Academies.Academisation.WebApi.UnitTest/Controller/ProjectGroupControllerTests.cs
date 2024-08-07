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
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using System;

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
		private string _trustReferenceNumber;
		private string _trustUkprn;
		public ProjectGroupControllerTests()
		{
			_mediatrMock = new Mock<IMediator>();
			_loggerMock = new Mock<ILogger<ProjectGroupController>>();
			_cancellationToken = CancellationToken.None;
			_projectGroupQueryServiceMock = new Mock<IProjectGroupQueryService>();
			_controller = new ProjectGroupController(_mediatrMock.Object, _loggerMock.Object, _projectGroupQueryServiceMock.Object);
			_trustReferenceNumber = _fixture.Create<string>()[..8];
			_trustUkprn = _fixture.Create<string>()[..8];
		}

		[Fact]
		public async Task CreateProjectGroup_ReturnsOk()
		{
			// Arrange
			var response = new ProjectGroupResponseModel(1, "12312", _trustReferenceNumber, "trustName", null, null, null);
			var command = new CreateProjectGroupCommand("trustName", _trustReferenceNumber, _trustUkprn, []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CreateSuccessResult<ProjectGroupResponseModel>(response));

			// Action
			var result = await _controller.CreateProjectGroup(command, _cancellationToken);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
			var responseModel = Assert.IsType<ProjectGroupResponseModel>(okObjectResult.Value);
			Assert.Equal(responseModel.ReferenceNumber, response.ReferenceNumber);
			Assert.Equal(responseModel.TrustReferenceNumber, response.TrustReferenceNumber);
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task CreateProjectGroup_ReturnsBadRequest()
		{
			// Arrange
			var command = new CreateProjectGroupCommand(_trustReferenceNumber, _trustUkprn, "trustName", []);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CreateValidationErrorResult([new ValidationError("ConversionsUrns", "Validation Error")]));

			// Action
			var result = await _controller.CreateProjectGroup(command, _cancellationToken);

			// Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task SetProjectGroup_ReturnsOk()
		{
			// Arrange
			var referenceNumber = "34234233";
			var command = new SetProjectGroupCommand([]);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CommandSuccessResult());

			// Action
			var result = await _controller.SetProjectGroup(referenceNumber, command, _cancellationToken) as OkResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.OK.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task SetProjectGroup_ReturnsNotFound()
		{
			// Arrange
			var referenceNumber = "34234233";
			var command = new SetProjectGroupCommand([]);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new NotFoundCommandResult());

			// Action
			var result = await _controller.SetProjectGroup(referenceNumber, command, _cancellationToken) as NotFoundResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.NotFound.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task SetProjectGroup_ReturnsBadRequest()
		{
			// Arrange
			var refereneNumber = "34234233";
			var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();
			var command = new SetProjectGroupCommand([]);
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CommandValidationErrorResult(expectedValidationErrors));

			// Action
			var result = await _controller.SetProjectGroup(refereneNumber, command, _cancellationToken);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationErrors, validationErrors);
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task AssignProjectGroupUser_ReturnsNotFound()
		{
			// Arrange
			var referenceNumber = "34234233";
			var command = new SetProjectGroupAssignUserCommand(Guid.NewGuid(), _fixture.Create<string>(), "fullname@email.com")
			{
				GroupReferenceNumber = referenceNumber
			};
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new NotFoundCommandResult());

			// Action
			var result = await _controller.AssignProjectGroupUser(referenceNumber, command, _cancellationToken) as NotFoundResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.NotFound.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task AssignProjectGroupUser_ReturnsOk()
		{
			// Arrange
			var referenceNumber = "34234233";
			var command = new SetProjectGroupAssignUserCommand(Guid.NewGuid(), _fixture.Create<string>(), "fullname@email.com")
			{
				GroupReferenceNumber = referenceNumber
			};	
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CommandSuccessResult());

			// Action
			var result = await _controller.AssignProjectGroupUser(referenceNumber, command, _cancellationToken) as OkResult;

			// Assert
			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.OK.GetHashCode());
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task AssignProjectGroupUser_ReturnsBadRequest()
		{
			// Arrange
			var referenceNumber = "34234233";
			var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();
			var command = new SetProjectGroupAssignUserCommand(Guid.NewGuid(), _fixture.Create<string>(), "")
			{
				GroupReferenceNumber = referenceNumber
			};
			_mediatrMock.Setup(x => x.Send(command, _cancellationToken))
				.ReturnsAsync(new CommandValidationErrorResult(expectedValidationErrors));

			// Action
			var result = await _controller.AssignProjectGroupUser(referenceNumber, command, _cancellationToken);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var validationErrors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
			Assert.Equal(expectedValidationErrors, validationErrors);
			_mediatrMock.Verify(x => x.Send(command, _cancellationToken), Times.Once());
		}

		//[Fact]
		//public async Task GetProjectGroups_ReturnsOk()
		//{
		//	// Arrange 
		//	var searchModel = new ProjectGroupSearchModel(1, 10, "34234233", null, null);
		//	var projectGroupResponse = new List<ProjectGroupResponseModel> { new(1, searchModel.ReferenceNumber!, _trustReferenceNumber, "trustName", null) };
		//	var pagingResponse = new PagedDataResponse<ProjectGroupResponseModel>(projectGroupResponse, new PagingResponse()
		//	{
		//		Page = 1,
		//		RecordCount = 1
		//	});
		//	_projectGroupQueryServiceMock.Setup(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.ReferenceNumber == searchModel.ReferenceNumber), _cancellationToken))
		//		.ReturnsAsync(pagingResponse);

		//	// Action
		//	var result = await _controller.GetProjectGroups(searchModel, _cancellationToken);

		//	// Assert
		//	var okObjectResult = Assert.IsType<OkObjectResult>(result.Result); 
		//	var response = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(okObjectResult.Value);
		//	Assert.Equal(response.Data.Count(), projectGroupResponse.Count);
		//	Assert.Equal(response.Paging.RecordCount, pagingResponse.Paging.RecordCount);
		//	Assert.Equal(response.Paging.Page, pagingResponse.Paging.Page);
		//	_projectGroupQueryServiceMock.Verify(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.ReferenceNumber == searchModel.ReferenceNumber), _cancellationToken), Times.Once());
		//}

		//[Fact]
		//public async Task GetProjectGroups_ReturnsNotFound()
		//{
		//	// Arrange
		//	var searchModel = new ProjectGroupSearchModel(1, 10, "34234233", null, null);
		//	var pagingResponse = new PagedDataResponse<ProjectGroupResponseModel>([], new PagingResponse()
		//	{
		//		Page = 1,
		//		RecordCount = 1
		//	});
		//	_projectGroupQueryServiceMock.Setup(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.ReferenceNumber == searchModel.ReferenceNumber), _cancellationToken))
		//		.ReturnsAsync(pagingResponse);

		//	// Action
		//	var result = await _controller.GetProjectGroups(searchModel, _cancellationToken);

		//	// Assert
		//	Assert.IsType<NotFoundResult>(result.Result);
		//	_projectGroupQueryServiceMock.Verify(x => x.GetProjectGroupsAsync(It.Is<ProjectGroupSearchModel>(x => x.ReferenceNumber == searchModel.ReferenceNumber), _cancellationToken), Times.Once());
		//}
	}
}
