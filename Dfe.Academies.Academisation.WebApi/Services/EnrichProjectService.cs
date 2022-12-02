using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;

namespace Dfe.Academies.Academisation.WebApi.Services
{
	public sealed class EnrichProjectService : BackgroundService
	{
		private readonly ILogger<EnrichProjectService> _logger;
		private readonly IServiceScopeFactory _factory;

		public EnrichProjectService(ILogger<EnrichProjectService> logger, IServiceScopeFactory factory)
		{
			_logger = logger;
			_factory = factory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _factory.CreateScope())
				{
					_logger.LogInformation("Enrich Project Service running at: {time}", DateTimeOffset.Now);
					var enrichProjectCommand = scope.ServiceProvider.GetRequiredService<IEnrichProjectCommand>();

					try
					{
						await enrichProjectCommand.Execute();
					}
					catch (Exception ex)
					{
						_logger.LogError("Error enriching project", ex);
					}

					await Task.Delay(60_000, stoppingToken);
				}
			}
		}
	}
}
