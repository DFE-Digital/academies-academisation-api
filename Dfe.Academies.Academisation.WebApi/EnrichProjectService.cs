namespace Dfe.Academies.Academisation.WebApi
{

	public sealed class EnrichProjectService : BackgroundService
	{
		private readonly ILogger<EnrichProjectService> _logger;

		public EnrichProjectService(ILogger<EnrichProjectService> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				//probably best to open a new DI scope here an resolve stateful dependencies (anything using dbcontext)
				//simulate per request lifetime
				//get projects that need LA data
				//get LA data from academisation api, handle service failure
				//log any issues
				//update projects
				_logger.LogInformation("Enrich Project Service running at: {time}", DateTimeOffset.Now);
				//run every min
				await Task.Delay(60_000, stoppingToken);
			}
		}
	}
}
