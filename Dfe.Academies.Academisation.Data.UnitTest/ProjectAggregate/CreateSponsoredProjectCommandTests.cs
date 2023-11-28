using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class CreateSponsoredProjectCommandTests
	{
		private readonly SponsoredProject _newProject;
		private readonly AcademisationContext _context;
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		private readonly Fixture _fixture;

		public CreateSponsoredProjectCommandTests()
		{
			_fixture = new Fixture();
			Project projectState = _fixture.Create<Project>();

			var testProjectContext = new TestProjectContext();
			_context = testProjectContext.CreateContext();
			_projectCreateDataCommand = new ProjectCreateDataCommand(_context);
			_context.Projects.Add(projectState);
			_context.SaveChanges();

			_newProject = _fixture.Create<SponsoredProject>();
		}

		private CreateSponsoredProjectDataCommand System_under_test()
		{
			return new CreateSponsoredProjectDataCommand(_projectCreateDataCommand);
		}

		[Fact]
		public async Task Should_add_the_new_sponsored_project()
		{
			CreateSponsoredProjectDataCommand command = System_under_test();

			await command.Execute(_newProject);

			_context.Projects.Count(x => x.Details.SchoolName == _newProject!.School!.Name).Should().Be(1);
		}

		[Fact]
		public async Task Should_return_error_if_school_is_null()
		{
			CreateSponsoredProjectDataCommand command = System_under_test();

			var result = await command.Execute(new SponsoredProject(null, _fixture.Create<SponsoredProjectTrust>()));

			result.Should().BeOfType<CommandValidationErrorResult>();
		}
		[Fact]
		public async Task Should_return_error_if_join_trust_is_null()
		{
			CreateSponsoredProjectDataCommand command = System_under_test();

			var result = await command.Execute(new SponsoredProject(_fixture.Create<SponsoredProjectSchool>(), null));

			result.Should().BeOfType<CommandValidationErrorResult>();
		}
	}
}
