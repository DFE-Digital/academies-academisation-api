using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
    public class SignificantChangeControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<SignificantChangeController>> _loggerMock;
        private readonly SignificantChangeController _controller;

        public SignificantChangeControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<SignificantChangeController>>();
            _controller = new SignificantChangeController(_mockMediator.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateSignificantProject_ReturnsCreated_WhenCreateIsSuccessful()
        {
            var command = CreateValidCommand();
            var payload = new SignificantChangeProjectResponse
            {
                Urn = command.Urn,
                Tier = command.Tier,
                TrustName = "Test Trust",
                TrustUkprn = command.TrustUkprn,
                TypeOfSignificantChange = command.Route,
                Status = "InProgress"
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<CreateSignificantProjectCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateSuccessResult<SignificantChangeProjectResponse>(payload));

            var result = await _controller.CreateSignificantProject(command, CancellationToken.None);

            result.Result.Should().BeOfType<CreatedResult>()
                .Which.Value.Should().BeEquivalentTo(payload);
        }

        [Fact]
        public async Task CreateSignificantProject_ReturnsBadRequest_WhenMediatorReturnsNull()
        {
            var command = CreateValidCommand();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<CreateSignificantProjectCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CreateResult)null!);

            var result = await _controller.CreateSignificantProject(command, CancellationToken.None);

            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetSignificantProjects_ReturnsResults_WhenFound()
        {
            var query = CreateValidQuery();
            var assignedUserId = Guid.NewGuid();
            var results = new List<SignificantChangeProjectSearchResponse>
            {
                new SignificantChangeProjectSearchResponse
                {
                    Urn = 123456,
                    SchoolName = "Test School",
                    Tier = 2,
                    TrustName = "Test Trust",
                    TrustUkprn = "12345678",
                    AssignedUser = new User(assignedUserId, "Assigned User", "assigned.user@test.local"),
                    TypeOfSignificantChange = "Change of age range",
                    Status = "InProgress"
                }
            };

            var expectedResponse = new PagedDataResponse<SignificantChangeProjectSearchResponse>(
                results,
                new PagingResponse { Page = query.Page, RecordCount = 1, NextPageUrl = null });

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetSignificantProjectsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GetSignificantChangeProjects(query, CancellationToken.None);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetSignificantProjects_ReturnsEmptyArray_WhenNoResultsFound()
        {
            var query = CreateValidQuery();
            var expectedResponse = new PagedDataResponse<SignificantChangeProjectSearchResponse>(
                [],
                new PagingResponse { Page = query.Page, RecordCount = 0, NextPageUrl = null });

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetSignificantProjectsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GetSignificantChangeProjects(query, CancellationToken.None);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetSignificantChangeProject_ReturnsProject_WhenFound()
        {
            var assignedUserId = Guid.NewGuid();
            var expectedResponse = new SignificantChangeProjectSearchResponse
            {
                Id = 100,
                Urn = 123456,
                SchoolName = "Test School",
                Tier = 2,
                TrustName = "Test Trust",
                TrustUkprn = "12345678",
                AssignedUser = new User(assignedUserId, "Assigned User", "assigned.user@test.local"),
                TypeOfSignificantChange = "Change of age range",
                Status = "InProgress"
            };

            _mockMediator
				.Setup(m => m.Send(It.IsAny<GetSignificantChangeProjectByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GetSignificantChangeProject(100, CancellationToken.None);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetSignificantChangeProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            _mockMediator
				.Setup(m => m.Send(It.IsAny<GetSignificantChangeProjectByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SignificantChangeProjectSearchResponse?)null);

            var result = await _controller.GetSignificantChangeProject(999, CancellationToken.None);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetFilterParameters_ReturnsOk_WithFilterParameters()
        {
            var expectedResponse = new SignificantChangeFilterParameters
            {
                Statuses = [new FilterValueDisplay("PreDecision", "Pre decision")],
                Tiers =
                [
                    new FilterValueDisplay("1", "Tier 1"),
                    new FilterValueDisplay("2", "Tier 2"),
                    new FilterValueDisplay("3", "Tier 3")
                ],
                AssignedUsers = [new FilterValueDisplay("Assigned User", "Assigned User")],
                Routes = [new FilterValueDisplay("Change of age range", "Change of age range")]
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetSignificantChangeFilterParametersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GetFilterParameters(CancellationToken.None);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResponse);
        }

        private static CreateSignificantProjectCommand CreateValidCommand()
        {
            return new CreateSignificantProjectCommand(
                Urn: 123456,
                Tier: 2,
                Route: "Change of age range",
                TrustUkprn: "12345678");
        }

        private static GetSignificantProjectsQuery CreateValidQuery()
        {
            return new GetSignificantProjectsQuery(
                Page: 1,
                Count: 10);
        }
    }
}
