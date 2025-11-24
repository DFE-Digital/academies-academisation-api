using System.Net;
using System.Net.Http.Json; 
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Complete.Client.Contracts;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject.Helpers;

public static class CompleteApiHelper
{
	public static async Task<HttpResponseMessage> ExecuteCompleteClientCallAsync<TRequest>(
			TRequest request,
			Func<TRequest, CancellationToken, Task<ProjectId?>> clientCall,
			Func<ProjectId?, object> successContentFactory,
			Polly.IAsyncPolicy<HttpResponseMessage> retryPolicy,
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
