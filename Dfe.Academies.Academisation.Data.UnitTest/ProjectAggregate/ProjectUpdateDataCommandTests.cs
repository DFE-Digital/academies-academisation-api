using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class ProjectUpdateDataCommandTests
	{
		private readonly AcademisationContext _context;
		private readonly Fixture _fixture = new();

		private readonly ProjectUpdateDataCommand _subject;

		public ProjectUpdateDataCommandTests()
		{
			_context = new TestProjectContext().CreateContext();
			_subject = new ProjectUpdateDataCommand(_context);
		}

		[Fact]
		public async Task ProjectExists___ProjectUpdated()
		{
			_context.ChangeTracker.AutoDetectChangesEnabled = false;
			// arrange
			Project? existingProject = _fixture.Build<Project>().Create();

			await _context.Projects.AddAsync(existingProject);

			await _context.SaveChangesAsync();

			IProject updatedProject = await _context.Projects.SingleAsync(p => p.Id == existingProject.Id);


			ProjectDetails projectDetails = _fixture.Build<ProjectDetails>()
				.With(p => p.Urn, updatedProject.Details.Urn)
				.With(x => x.ExternalApplicationFormSaved, true)
				.With(x => x.ExternalApplicationFormUrl, "test//url")
				.Create();


			updatedProject.Update(projectDetails);


			await _context.Projects.LoadAsync();

			// act
			await _subject.Execute(updatedProject);

			// assert
			Assert.True(projectDetails.Equals(updatedProject.Details));
		}
	}
}
