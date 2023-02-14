using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class CreateInvoluntaryProjectCommandTests
	{
		private readonly InvoluntaryProject _newProject;
		private readonly AcademisationContext _context;
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		private readonly Fixture _fixture;

		public CreateInvoluntaryProjectCommandTests()
		{
			_fixture = new Fixture();
			ProjectState projectState = _fixture.Create<ProjectState>();

			var testProjectContext = new TestProjectContext();
			_context = testProjectContext.CreateContext();
			_projectCreateDataCommand = new ProjectCreateDataCommand(_context);
			_context.Projects.Add(projectState);
			_context.SaveChanges();

			_newProject = _fixture.Create<InvoluntaryProject>();
		}

		private CreateInvoluntaryProjectDataCommand System_under_test()
		{
			return new CreateInvoluntaryProjectDataCommand(_projectCreateDataCommand);
		}

		[Fact]
		public async Task Should_add_the_new_involuntary_project()
		{
			CreateInvoluntaryProjectDataCommand command = System_under_test();

			await command.Execute(_newProject);

			_context.Projects.Count(x => x.SchoolName == _newProject!.School!.Name).Should().Be(1);
		}

		[Fact]
		public async Task Should_return_error_if_school_is_null()
		{
			CreateInvoluntaryProjectDataCommand command = System_under_test();

			var result = await command.Execute(new InvoluntaryProject(null, _fixture.Create<ProjectTrust>()));

			result.Should().BeOfType<CommandValidationErrorResult>();
		}
		[Fact]
		public async Task Should_return_error_if_join_trust_is_null()
		{
			CreateInvoluntaryProjectDataCommand command = System_under_test();

			var result = await command.Execute(new InvoluntaryProject(_fixture.Create<ProjectSchool>(), null));

			result.Should().BeOfType<CommandValidationErrorResult>();
		}
	}
}
