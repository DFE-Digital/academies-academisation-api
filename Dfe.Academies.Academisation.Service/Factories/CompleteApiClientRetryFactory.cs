using System.Net;
using System.Net.Http.Json;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Complete.Client.Contracts;
using Microsoft.Extensions.Logging;
using Polly;

namespace Dfe.Academies.Academisation.Service.Factories;

public class CompleteApiClientRetryFactory(IPollyPolicyFactory pollyPolicyFactory, IProjectsClient projectsClient): ICompleteApiClientRetryFactory
{
	public IAsyncPolicy<HttpResponseMessage> GetCompleteHttpClientRetryPolicy(ILogger logger) 
		=> pollyPolicyFactory.GetCompleteHttpClientRetryPolicy(logger);

	public async Task<HttpResponseMessage> CreateConversionProjectAsync(CreateConversionProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken)
	{ 
		return await ExecuteCompleteClientCallAsync(
					command,
					projectsClient.CreateConversionProjectAsync!,
					id => new CreateCompleteConversionProjectSuccessResponse(id!.Value.GetValueOrDefault()),
					retryPolicy,
					cancellationToken);
	}
	public async Task<HttpResponseMessage> CreateConversionMatProjectAsync(CreateConversionMatProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken)
	{ 
		return await ExecuteCompleteClientCallAsync(
					command,
					projectsClient.CreateConversionMatProjectAsync!,
					id => new CreateCompleteConversionProjectSuccessResponse(id!.Value.GetValueOrDefault()),
					retryPolicy,
					cancellationToken);
	}
	public async Task<HttpResponseMessage> CreateTransferProjectAsync(CreateTransferProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken)
	{ 
		return await ExecuteCompleteClientCallAsync(
					command,
					projectsClient.CreateTransferProjectAsync!,
					id => new CreateCompleteTransferProjectSuccessResponse(id!.Value.GetValueOrDefault()),
					retryPolicy,
					cancellationToken);
	}
	public async Task<HttpResponseMessage> CreateTransferMatProjectAsync(CreateTransferMatProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken)
	{ 
		return await ExecuteCompleteClientCallAsync(
					command,
					projectsClient.CreateTransferMatProjectAsync!,
					id => new CreateCompleteTransferProjectSuccessResponse(id!.Value.GetValueOrDefault()),
					retryPolicy,
					cancellationToken);
	}

	private static async Task<HttpResponseMessage> ExecuteCompleteClientCallAsync<TRequest>(
			TRequest request,
			Func<TRequest, CancellationToken, Task<ProjectId?>> clientCall,
			Func<ProjectId?, object> successContentFactory,
			IAsyncPolicy<HttpResponseMessage> retryPolicy,
			CancellationToken cancellationToken)
	{
		return await retryPolicy.ExecuteAsync(async () =>
		{
			try
			{
				var projectId = await clientCall(request, cancellationToken).ConfigureAwait(false);

				return new HttpResponseMessage(HttpStatusCode.Created)
				{
					Content = JsonContent.Create(successContentFactory(projectId))
				};
			}
			catch (CompleteApiException ex)
			{
				return new HttpResponseMessage((HttpStatusCode)ex.StatusCode)
				{
					Content = JsonContent.Create(new CreateCompleteProjectErrorResponse(ex.Response))
				};
			}
		}).ConfigureAwait(false);
	}
}
