using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dfe.Academies.Academisation.IntegrationTest;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
	private readonly string _authKey;
	private AcademisationContext _dbContext = null!;

	public TestWebApplicationFactory()
	{
		_authKey = Guid.NewGuid().ToString();
	}

	protected override void ConfigureClient(HttpClient client)
	{
		client.DefaultRequestHeaders.Add("x-api-key", _authKey);
		base.ConfigureClient(client);
	}
	
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("production");

		var config = new ConfigurationBuilder()
			.AddUserSecrets<Program>()
			.AddEnvironmentVariables()
			.AddJsonFile("appsettings.json")
			.Build();
		
		var academiesDbConnectionString = config.GetValue<string>("AcademiesDatabaseConnectionString");
		
		var regex = new Regex("(?<=Initial Catalog=)(.*?)(?=;)");
		var testDbConnectionString = regex.Replace(academiesDbConnectionString, $"testDb_{Guid.NewGuid()}");
		
		builder.ConfigureTestServices(services =>
		{
			var context = services.Single(d => d.ServiceType == typeof(DbContextOptions<AcademisationContext>));
			services.Remove(context);
			services.AddDbContext<AcademisationContext>(options => options.UseSqlServer(testDbConnectionString));

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
		_dbContext.AddRange(new ConversionAdvisoryBoardDecisionState
		{
			ConversionProjectId = 1000,
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
		_dbContext.Database.EnsureDeleted();
		base.Dispose(disposing);
	}
}
