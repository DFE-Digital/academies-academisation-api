using System.Threading;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

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

        private static CreateSignificantProjectCommand CreateValidCommand()
        {
            return new CreateSignificantProjectCommand(
                Urn: 123456,
                Tier: 2,
                Route: "Change of age range",
                TrustUkprn: "12345678");
        }
    }
}
