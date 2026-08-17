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
                SchoolName = "Test School",
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
        public async Task CreateSignificantProject_ReturnsNotFound_WhenValidationErrorReturned()
        {
            var command = CreateValidCommand();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<CreateSignificantProjectCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateValidationErrorResult(
                    [new ValidationError("TrustUkprn", $"Trust with UKPRN {command.TrustUkprn} not found")]));

            var result = await _controller.CreateSignificantProject(command, CancellationToken.None);

            result.Result.Should().BeOfType<NotFoundResult>();
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
        public async Task SetAssignedUser_ReturnsOk_AndUsesRouteId_WhenCommandIsSuccessful()
        {
            var routeId = 100;
            var request = new SetSignificantChangeAssignedUserPublicCommand(
                userId: Guid.NewGuid(),
                fullName: "Assigned User",
                emailAddress: "assigned.user@test.local");

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeAssignedUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandSuccessResult());

            var result = await _controller.SetSignificantChangeAssignedUser(routeId, request);

            result.Should().BeOfType<OkResult>();
            _mockMediator.Verify(m => m.Send(
                It.Is<SetSignificantChangeAssignedUserCommand>(c =>
                    c.Id == routeId
                    && c.UserId == request.UserId
                    && c.FullName == request.FullName
                    && c.EmailAddress == request.EmailAddress),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SetAssignedUser_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            var request = new SetSignificantChangeAssignedUserPublicCommand(
                userId: Guid.NewGuid(),
                fullName: "Assigned User",
                emailAddress: "assigned.user@test.local");

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeAssignedUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotFoundCommandResult());

            var result = await _controller.SetSignificantChangeAssignedUser(100, request);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SetAssignedUser_ReturnsBadRequest_WhenValidationFails()
        {
            var request = new SetSignificantChangeAssignedUserPublicCommand(
                userId: Guid.NewGuid(),
                fullName: string.Empty,
                emailAddress: "not-an-email");

            var validationErrors = new[]
            {
                new ValidationError("FullName", "Full name is required")
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeAssignedUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandValidationErrorResult(validationErrors));

            var result = await _controller.SetSignificantChangeAssignedUser(100, request);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(validationErrors);
        }

        [Fact]
        public async Task SetStakeholderConsultation_ReturnsOk_AndUsesRouteId_WhenCommandIsSuccessful()
        {
            var routeId = 100;
            var request = new SetSignificantChangeStakeholderConsultationPublicCommand(
                trustConsultedStakeholders: false,
                trustConsultedStakeholdersNotConsultedReason: "Trust has not consulted stakeholders yet");

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeStakeholderConsultationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandSuccessResult());

            var result = await _controller.SetSignificantChangeStakeholderConsultation(routeId, request);

            result.Should().BeOfType<OkResult>();
            _mockMediator.Verify(m => m.Send(
                It.Is<SetSignificantChangeStakeholderConsultationCommand>(c =>
                    c.Id == routeId
                    && c.TrustConsultedStakeholders == request.TrustConsultedStakeholders
                    && c.TrustConsultedStakeholdersNotConsultedReason == request.TrustConsultedStakeholdersNotConsultedReason),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SetStakeholderConsultation_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            var request = new SetSignificantChangeStakeholderConsultationPublicCommand(
                trustConsultedStakeholders: true,
                trustConsultedStakeholdersNotConsultedReason: null);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeStakeholderConsultationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotFoundCommandResult());

            var result = await _controller.SetSignificantChangeStakeholderConsultation(100, request);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SetStakeholderConsultation_ReturnsBadRequest_WhenValidationFails()
        {
            var request = new SetSignificantChangeStakeholderConsultationPublicCommand(
                trustConsultedStakeholders: null,
                trustConsultedStakeholdersNotConsultedReason: null);

            var validationErrors = new[]
            {
                new ValidationError("TrustConsultedStakeholders", "Trust consulted stakeholders is required")
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeStakeholderConsultationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandValidationErrorResult(validationErrors));

            var result = await _controller.SetSignificantChangeStakeholderConsultation(100, request);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(validationErrors);
        }

        [Fact]
        public async Task SetAdmissionVariationConsultation_ReturnsOk_AndUsesRouteId_WhenCommandIsSuccessful()
        {
            var routeId = 100;
            var request = new SetSignificantChangeAdmissionVariationConsultationPublicCommand(
                consultationIncludeAdmissionVariation: false,
                consultationIncludeAdmissionVariationNotApplicable: false,
                noAdmissionVariationReason: "No admission variation required");

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeAdmissionVariationConsultationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandSuccessResult());

            var result = await _controller.SetSignificantChangeAdmissionVariationConsultation(routeId, request);

            result.Should().BeOfType<OkResult>();
            _mockMediator.Verify(m => m.Send(
                It.Is<SetSignificantChangeAdmissionVariationConsultationCommand>(c =>
                    c.Id == routeId
                    && c.ConsultationIncludeAdmissionVariation == request.ConsultationIncludeAdmissionVariation
                    && c.ConsultationIncludeAdmissionVariationNotApplicable == request.ConsultationIncludeAdmissionVariationNotApplicable
                    && c.NoAdmissionVariationReason == request.NoAdmissionVariationReason),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SetAdmissionVariationConsultation_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            var request = new SetSignificantChangeAdmissionVariationConsultationPublicCommand(
                consultationIncludeAdmissionVariation: true,
                consultationIncludeAdmissionVariationNotApplicable: false,
                noAdmissionVariationReason: null);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeAdmissionVariationConsultationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotFoundCommandResult());

            var result = await _controller.SetSignificantChangeAdmissionVariationConsultation(100, request);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SetAdmissionVariationConsultation_ReturnsBadRequest_WhenValidationFails()
        {
            var request = new SetSignificantChangeAdmissionVariationConsultationPublicCommand(
                consultationIncludeAdmissionVariation: null,
                consultationIncludeAdmissionVariationNotApplicable: null,
                noAdmissionVariationReason: null);

            var validationErrors = new[]
            {
                new ValidationError("ConsultationIncludeAdmissionVariation", "Consultation include admission variation is required")
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetSignificantChangeAdmissionVariationConsultationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandValidationErrorResult(validationErrors));

            var result = await _controller.SetSignificantChangeAdmissionVariationConsultation(100, request);

            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(validationErrors);
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
