using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using System.Threading;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ProjectGroupControllerTests
	{
		private readonly Mock<ILogger<ProjectGroupController>> _loggerMock;
		private readonly Mock<IMediator> _mediatrMock;
		private CancellationToken _cancellationToken;
		public ProjectGroupControllerTests()
		{
			_mediatrMock = new Mock<IMediator>();
			_loggerMock = new Mock<ILogger<ProjectGroupController>>();
			_cancellationToken = CancellationToken.None;
		}

		[Fact]
		public async Task CreateProjectGroup_ReturnsOk()
		{
			var command = new CreateProjectGroupCommand("12345679", "UK3423423");
			_mediatrMock.Setup(x => x.Send(command, new CancellationToken()))
				.ReturnsAsync(new CommandSuccessResult());

			var controller = new ProjectGroupController(_mediatrMock.Object, _loggerMock.Object);

			var result = await controller.CreateProjectGroup(command, _cancellationToken) as OkResult;

			Assert.Equal(result!.StatusCode, System.Net.HttpStatusCode.OK.GetHashCode());
		}
	}
}
