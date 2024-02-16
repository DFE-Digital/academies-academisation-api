using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dfe.Academies.Academisation.IntegrationTest;

public class TestWebApplicationFactory : WebApplicationFactory<WebApi.Program>
{
	private readonly string _authKey;
	private AcademisationContext _dbContext = null!;
	private readonly SqliteConnection _connection;

	public TestWebApplicationFactory()
	{
		_authKey = Guid.NewGuid().ToString();
		_connection = new("Filename=:memory:");
		_connection.Open();
	}

	protected override void ConfigureClient(HttpClient client)
	{
		client.DefaultRequestHeaders.Add("x-api-key", _authKey);
		base.ConfigureClient(client);
	}
	
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("production");
		builder.ConfigureTestServices(services =>
		{
			var context = services.Single(d => d.ServiceType == typeof(DbContextOptions<AcademisationContext>));
			services.Remove(context);
			services.AddDbContext<AcademisationContext>(options => options.UseSqlite(_connection));
			
			var optionsConfig = Options.Create<AuthenticationConfig>(new() {ApiKeys = new List<string> {_authKey}});
			services.AddScoped<IOptions<AuthenticationConfig>>(_ => optionsConfig);
		
			var serviceProvider = services.BuildServiceProvider();

			_dbContext = serviceProvider.GetRequiredService<AcademisationContext>();

			_dbContext.Database.EnsureCreated();

			SeedDecisionData();
		});
	}

	private void SeedDecisionData()
	{
		_dbContext.AddRange(new AdvisoryBoardDecisionState
		{
			ConversionProjectId = 1000,
			TransferProjectId = null,
			Decision = AdvisoryBoardDecision.Approved,
			ApprovedConditionsSet = true,
			ApprovedConditionsDetails = "TestData",
			DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
			AdvisoryBoardDecisionDate = DateTime.UtcNow.AddMonths(-1),
			CreatedOn = new(2022, 02, 02),
			LastModifiedOn = new(2022, 02, 02)
		});
		
		_dbContext.SaveChanges();
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposing) return;
		
		_dbContext.Database.EnsureDeleted();
		
		if(_dbContext.Database.IsSqlite()) _connection.Dispose();
		
		base.Dispose(disposing);
	}
}
