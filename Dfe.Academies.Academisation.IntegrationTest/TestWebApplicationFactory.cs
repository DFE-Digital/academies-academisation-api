using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dfe.Academies.Academisation.IntegrationTest;

public class TestWebApplicationFactory : WebApplicationFactory<WebApi.Program>
{
	private readonly string _authKey;
	private AcademisationContext _dbContext = null!;
	private readonly SqliteConnection _connection;

	public AcademisationContext Context
	{
		get
		{
			return _dbContext ?? throw new InvalidOperationException("Database context is not initialized. Ensure the factory is properly configured before use.");
		}
	}

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
			// Remove the options AND the underlying configure-action descriptor, otherwise the
			// original UseSqlServer configuration still gets applied alongside UseSqlite below.
			services.RemoveAll<DbContextOptions<AcademisationContext>>();
			services.RemoveAll<IDbContextOptionsConfiguration<AcademisationContext>>();
			services.AddDbContext<AcademisationContext>(options => options.UseSqlite(_connection));

			var optionsConfig = Options.Create<AuthenticationConfig>(new() { ApiKeys = new List<string> { _authKey } });
			services.AddScoped(_ => optionsConfig);

			var serviceProvider = services.BuildServiceProvider();

			_dbContext = serviceProvider.GetRequiredService<AcademisationContext>();

			_dbContext.Database.EnsureCreated();

			SeedDecisionData();
		});
	}
	private void SeedDecisionData()
	{
		_dbContext.AddRange(new Domain.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecision(
			1,
			new AdvisoryBoardDecisionDetails(1000, null, null, AdvisoryBoardDecision.Approved, true, "TestData", System.DateTime.UtcNow.AddMonths(-1), System.DateTime.UtcNow.AddMonths(-1), DecisionMadeBy.DirectorGeneral, "John Smith"),
			null!, null!, null!, null, null, new(2022, 02, 02),
			new(2022, 02, 02)
		));

		_dbContext.SaveChanges();
	}

	public async Task<SignificantChangeProject> CreateSignificantChangeProjectWithCompletedTasksAsync()
	{
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);

		project.AssignUser(Guid.NewGuid(), "test.user@example.com", "Test User");
		project.SetConsultationDuration(ConsultationDurationAnswer.Yes, null);
		project.SetEqualitiesImpactAssessment(true, null, EqualitiesImpact.Unlikely, null, null, null);
		project.SetReligiousBodyConsultation(
			YesNoNotApplicableAnswer.Yes, null,
			YesNoNotApplicableAnswer.No, null,
			YesNoNotApplicableAnswer.Yes, null, null);
		project.SetProjectDates(DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));
		project.SetLandTransaction(
			YesNoNotApplicableAnswer.NotApplicable, null,
			YesNoNotApplicableAnswer.NotApplicable, null, null);
		project.SetOtherStakeholderEngagement(
			true, null, false, null, YesNoNotApplicableAnswer.NotApplicable, null, null);
		project.SetFunding(FundingAnswer.Yes, null, null);
		project.SetPlanningPermission(PlanningPermissionAnswer.NotApplicable, null, null);
		project.SetRecommendation(Recommendation.Approve, null);
		project.SetHqtfConsideration(HqtfConsiderationAnswer.NotApplicable, null, null);
		project.SetFhaRisksOrIssuesRaised(false, null, null);
		project.SetAdmissionVariationDetails(false, null, null, null, null, null, null, null);
		project.SetAdmissionsVariationRecommendation(AdmissionsVariationRecommendationAnswer.NotApplicable);
		project.SetOfstedInspection(DateTime.UtcNow.AddMonths(-1), true, null, null);
		project.SetLocalAuthorityObjections(false, null, null);
		project.SetSpecialSchoolSatelliteConsiderations(YesNoNotApplicableAnswer.NotApplicable, null, null);

		Context.Add(project);
		await Context.SaveChangesAsync();

		return project;
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposing) return;

		_dbContext.Database.EnsureDeleted();

		if (_dbContext.Database.IsSqlite()) _connection.Dispose();

		base.Dispose(disposing);
	}
}
