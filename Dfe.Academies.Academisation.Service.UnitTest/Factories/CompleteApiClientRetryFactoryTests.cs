using System.Net;
using System.Net.Http.Json;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Complete.Client.Contracts;
using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Factories;

public class CompleteApiClientRetryFactoryTests
{
	[Fact]
	public async Task CreateSignificantChangeProjectAsync_ReturnsSignificantChangeProjectIdResponse()
	{
		var pollyPolicyFactory = new Mock<IPollyPolicyFactory>();
		var projectsClient = new Mock<IProjectsClient>();
		var significantChangeProjectsClient = new Mock<ISignificantChangeProjectsClient>();
		var completeProjectId = Guid.NewGuid();
		pollyPolicyFactory
			.Setup(factory => factory.GetCompleteHttpClientRetryPolicy(It.IsAny<ILogger>()))
			.Returns(Policy.NoOpAsync<HttpResponseMessage>());
		significantChangeProjectsClient
			.Setup(client => client.CreateSignificantChangeProjectAsync(It.IsAny<CreateSignificantChangeProjectCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ProjectId { Value = completeProjectId });

		var factory = new CompleteApiClientRetryFactory(
			pollyPolicyFactory.Object, projectsClient.Object, significantChangeProjectsClient.Object);

		using var response = await factory.CreateSignificantChangeProjectAsync(
			new CreateSignificantChangeProjectCommand(),
			Policy.NoOpAsync<HttpResponseMessage>(),
			CancellationToken.None);
		var content = await response.Content.ReadFromJsonAsync<CreateCompleteSignificantChangeSuccessResponse>();

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		Assert.NotNull(content);
		Assert.Equal(completeProjectId, content.significantchange_project_id);
	}
}