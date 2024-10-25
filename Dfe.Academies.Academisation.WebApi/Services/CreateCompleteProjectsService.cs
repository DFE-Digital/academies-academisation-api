using Dfe.Academies.Academisation.Service.Commands.CompleteProject;
using Dfe.Academies.Academisation.Service.Commands.FormAMat;
using Dfe.Academisation.CorrelationIdMiddleware;
using MediatR;

namespace Dfe.Academies.Academisation.WebApi.Services
{
	public sealed class CreateCompleteProjectsService : BackgroundService
	{
		private readonly ILogger<CreateCompleteProjectsService> _logger;
		private readonly IServiceScopeFactory _factory;
		private readonly int _delayInMilliseconds;

		public CreateCompleteProjectsService(ILogger<CreateCompleteProjectsService> logger, IServiceScopeFactory factory,
			IConfiguration config)
		{
			_logger = logger;
			_factory = factory;
			_delayInMilliseconds = config.GetValue<int?>("DatabasePollingDelay") ?? 10_000;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _factory.CreateScope())
				{
					// CorrelationId middleware isn't able to handle making sure a correlation context is available for outgoing http requests. So resolve the context and set the correlation Id scope manually.
					// Use the same correlation id for each execution of the command.
					// Also use the correlation in a logger scope to make sure it's output.
					var correlationContext = scope.ServiceProvider.GetRequiredService<ICorrelationContext>();
					correlationContext.SetContext(Guid.NewGuid());

					using (_logger.BeginScope("x-correlationId: {x-correlationId}", correlationContext.CorrelationId.ToString()))
					{
						_logger.LogInformation("Create Complete Projects Service running at: {time}", DateTimeOffset.Now);
						var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

						try
						{ 
							await mediator.Send(new CreateCompleteConversionProjectsCommand(), stoppingToken);
							await mediator.Send(new CreateCompleteTransferProjectsCommand(), stoppingToken);
							await mediator.Send(new CreateCompleteFormAMatConversionProjectsCommand(), stoppingToken);
						}
						catch (Exception ex)
						{
							_logger.LogError("Error enriching conversions complete project", ex);
						}

						await Task.Delay(_delayInMilliseconds, stoppingToken);
						
						try
						{ 
							await mediator.Send(new CreateCompleteTransferProjectsCommand(), stoppingToken);
						}
						catch (Exception ex)
						{
							_logger.LogError("Error enriching transfers complete project", ex);
						}

						await Task.Delay(_delayInMilliseconds, stoppingToken);
					}
				}
			}
		}
	}
}
