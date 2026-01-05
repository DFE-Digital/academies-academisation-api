using Dfe.Complete.Client.Contracts;
using Microsoft.Extensions.Logging;
using Polly;

namespace Dfe.Academies.Academisation.Service.Factories;

public interface ICompleteApiClientRetryFactory
{
	IAsyncPolicy<HttpResponseMessage> GetCompleteHttpClientRetryPolicy(ILogger logger);
	Task<HttpResponseMessage> CreateConversionProjectAsync(CreateConversionProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken);
	Task<HttpResponseMessage> CreateConversionMatProjectAsync(CreateConversionMatProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken);
	Task<HttpResponseMessage> CreateTransferProjectAsync(CreateTransferProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken);
	Task<HttpResponseMessage> CreateTransferMatProjectAsync(CreateTransferMatProjectCommand command, IAsyncPolicy<HttpResponseMessage> retryPolicy, CancellationToken cancellationToken);
}
