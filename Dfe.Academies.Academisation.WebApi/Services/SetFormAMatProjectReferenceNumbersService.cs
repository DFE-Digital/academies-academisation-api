﻿using Dfe.Academies.Academisation.Service.Commands.FormAMat;
using Dfe.Academisation.CorrelationIdMiddleware;
using MediatR;

namespace Dfe.Academies.Academisation.WebApi.Services
{
	public sealed class SetFormAMatProjectReferenceNumbersService : BackgroundService
	{
		private readonly ILogger<SetFormAMatProjectReferenceNumbersService> _logger;
		private readonly IServiceScopeFactory _factory;
		private readonly int _delayInMilliseconds;

		public SetFormAMatProjectReferenceNumbersService(ILogger<SetFormAMatProjectReferenceNumbersService> logger, IServiceScopeFactory factory,
			IConfiguration config)
		{
			_logger = logger;
			_factory = factory;
			_delayInMilliseconds = config.GetValue<int?>("DatabasePollingDelay") ?? 60_000;
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
						_logger.LogInformation("Setting Form A Mat Projects Reference Numbers Service running at: {time}", DateTimeOffset.Now);
						var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

						try
						{
							await mediator.Send(new SetFormAMatReferenceNumberCommand(), stoppingToken);
						}
						catch (Exception ex)
						{
							_logger.LogError("Error setting project reference numbers", ex);
						}

						await Task.Delay(_delayInMilliseconds, stoppingToken);
					}
				}
			}
		}
	}
}
