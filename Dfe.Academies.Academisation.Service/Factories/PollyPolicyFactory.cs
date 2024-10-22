using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Dfe.Academies.Academisation.Service.Factories
{
	public class PollyPolicyFactory : IPollyPolicyFactory
	{
		public IAsyncPolicy<HttpResponseMessage> GetCompleteHttpClientRetryPolicy(ILogger logger)
		{

			return Policy.Handle<HttpRequestException>() // Handle HttpRequestException
									.OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && r.StatusCode != HttpStatusCode.BadRequest) // Retry if response is not successful
									.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
			(exception, timeSpan, retryCount, context) =>
			{
				logger.LogInformation($"Retry {retryCount} after {timeSpan.Seconds} seconds due to: {exception?.Exception?.Message}");
			});
		}
	}
}
