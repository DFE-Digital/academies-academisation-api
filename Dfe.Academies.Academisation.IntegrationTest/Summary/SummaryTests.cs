using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Summary;
using Dfe.Academies.Academisation.IntegrationTest.Extensions;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.Summary
{

	[Collection("AdvisoryBoardDecision")]
	public class SummaryTests
	{
		private readonly TestWebApplicationFactory _factory;
		private readonly Fixture _fixture = new();

		public SummaryTests(TestWebApplicationFactory factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task Get_WhenProjectFound___ReturnsOk_AndDecisionIsRetrievedFromDatabase()
		{
			const string testEmail = "a@b.com";

			var client = _factory.CreateClient();
			
			_fixture.Customize<ProjectDetails>(
				composer => composer.With(x => x.ProjectStatus, "Converter Pre-AO (C)")
			);

			var conversionProjectDetails = _fixture.Create<ProjectDetails>();

			conversionProjectDetails.AssignedUser = new User(Guid.NewGuid(), "Test User", testEmail);

			var newProject = new Project(0, conversionProjectDetails);

			_factory.Context.Projects.Add(newProject);

			await _factory.Context.SaveChangesAsync();

			var result = await client.GetDeserialized<IEnumerable<ProjectSummary>>(
				$"/summary/projects?email=a%40b.com");

			Assert.Multiple(() =>
			{
				Assert.Equal(HttpStatusCode.OK, result.StatusCode);
				Assert.NotNull(result.Result);
				Assert.Equal(testEmail, result.Result.First().ConversionsSummary?.AssignedUserEmailAddress);
			});
		}

	}
}
