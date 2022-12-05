using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.WebApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Services
{
	public class EnrichProjectServiceTests
	{
		private readonly EnrichProjectService _subject;
		private readonly Mock<IEnrichProjectCommand> _enrichProjectCommand;

		public EnrichProjectServiceTests()
		{
			_enrichProjectCommand = new Mock<IEnrichProjectCommand>();

			var serviceProvider = new Mock<IServiceProvider>();
			serviceProvider.Setup(x => x.GetService(typeof(IEnrichProjectCommand))).Returns(_enrichProjectCommand.Object);
			var serviceScope = new Mock<IServiceScope>();
			serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);
			var serviceScopeFactory = new Mock<IServiceScopeFactory>();
			serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
			serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactory.Object);

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(new Dictionary<string, string> { { "DatabasePollingDelay", "50" } })
				.Build();

			_subject = new EnrichProjectService(Mock.Of<ILogger<EnrichProjectService>>(), serviceScopeFactory.Object,
				configuration);
		}

		[Fact]
		public async Task StartAsync__ExecutesCommand__WaitsAppropriateTime__AndGoesAgain()
		{
			await _subject.StartAsync(new CancellationTokenSource(550).Token);

			await Task.Delay(520);

			_enrichProjectCommand.Verify(m => m.Execute(), Times.AtLeast(5));

			_subject.Dispose();
		}

		[Fact]
		public async Task StartAsync__ThrowsException__WaitsAppropriateTime__AndGoesAgain()
		{
			_enrichProjectCommand.SetupSequence(m => m.Execute())
				.Throws<Exception>()
				.PassAsync();

			await _subject.StartAsync(new CancellationTokenSource(550).Token);

			await Task.Delay(350);

			_enrichProjectCommand.Verify(m => m.Execute(), Times.AtLeast(2));

			_subject.Dispose();
		}
	}
}
