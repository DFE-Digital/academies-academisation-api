using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class SignificantChangeDecisionControllerGetTests
{
	private readonly Fixture _fixture = new();
	private readonly Mock<IMediator> _mockMediator = new();

	[Fact]
	public async Task QueryReturnsData_ReturnsOkResult()
	{
		var projectId = _fixture.Create<int>();
		var expected = _fixture.Create<SignificantChangeDecisionServiceModel>();
		using var cancellationTokenSource = new CancellationTokenSource();
		var cancellationToken = cancellationTokenSource.Token;

		_mockMediator
			.Setup(m => m.Send(
				It.Is<GetSignificantChangeDecisionQuery>(q => q.ProjectId == projectId),
				cancellationToken))
			.ReturnsAsync(expected);

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.GetByProjectId(projectId, cancellationToken);

		var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public async Task QueryReturnsNull_ReturnsNotFoundResult()
	{
		var projectId = _fixture.Create<int>();

		_mockMediator
			.Setup(m => m.Send(
				It.Is<GetSignificantChangeDecisionQuery>(q => q.ProjectId == projectId),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeDecisionServiceModel?)null);

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.GetByProjectId(projectId, default);

		result.Result.Should().BeOfType<NotFoundResult>();
	}
}
