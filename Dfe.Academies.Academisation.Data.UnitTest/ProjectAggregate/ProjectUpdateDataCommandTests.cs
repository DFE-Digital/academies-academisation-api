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
			// arrange
			Project? existingProject = _fixture.Build<Project>().Create();
			await _context.Projects.AddAsync(existingProject);
			await _context.SaveChangesAsync();

			ProjectDetails projectDetails = _fixture.Create<ProjectDetails>();

			IProject project = new Project(existingProject.Id, projectDetails);
			await _context.Projects.LoadAsync();

			// act
			await _subject.Execute(project);

			_context.ChangeTracker.Clear();

			Project updatedProject = await _context.Projects.SingleAsync(p => p.Id == project.Id);

			// assert
			Assert.True(projectDetails.Equals(updatedProject));
		}
	}
}
