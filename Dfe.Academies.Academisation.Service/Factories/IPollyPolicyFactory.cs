using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Dfe.Academies.Academisation.Service.Factories
{
	public interface IPollyPolicyFactory
	{
		IAsyncPolicy<HttpResponseMessage> GetCompleteHttpClientRetryPolicy(ILogger logger);
	}
}
