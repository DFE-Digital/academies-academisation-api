//using System.Linq;
//using System.Threading.Tasks;
//using AutoFixture;
//using Dfe.Academies.Academisation.Core;
//using Dfe.Academies.Academisation.Data.ProjectAggregate;
//using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
//using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
//using Dfe.Academies.Academisation.Domain.ProjectAggregate;
//using Dfe.Academies.Academisation.IData.ProjectAggregate;
//using FluentAssertions;
//using Xunit;

//namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
//{
//	public class CreateNewProjectCommandTests
//	{
//		private readonly NewProject _newProject;
//		private readonly AcademisationContext _context;
//		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
//		private readonly Fixture _fixture;

//		public CreateNewProjectCommandTests()
//		{
//			_fixture = new Fixture();
//			Project projectState = _fixture.Create<Project>();

//			var testProjectContext = new TestProjectContext();
//			_context = testProjectContext.CreateContext();
//			_projectCreateDataCommand = new ProjectCreateDataCommand(_context);
//			_context.Projects.Add(projectState);
//			_context.SaveChanges();

//			_newProject = _fixture.Create<NewProject>();
//		}

//		private CreateNewProjectDataCommand System_under_test()
//		{
//			return new CreateNewProjectDataCommand(_projectCreateDataCommand);
//		}

//		[Fact]
//		public async Task Should_add_the_new_sponsored_project()
//		{
//			CreateNewProjectDataCommand command = System_under_test();

//			await command.Execute(_newProject);

//			_context.Projects.Count(x => x.Details.SchoolName == _newProject!.School!.Name).Should().Be(1);
//		}
//		[Fact]
//		public async Task Should_add_the_new_conversion_project()
//		{
//			CreateNewProjectDataCommand command = System_under_test();
//			await command.Execute(new NewProject(_newProject.School, _newProject.Trust, "yes", "yes", false));

//			_context.Projects.Count(x => x.Details.AcademyTypeAndRoute == "Converter").Should().Be(1);
//		}

//		[Fact]
//		public async Task Should_return_error_if_school_is_null()
//		{
//			CreateNewProjectDataCommand command = System_under_test();

//			var result = await command.Execute(new NewProject(null, _fixture.Create<NewProjectTrust>(), null, null, false));

//			result.Should().BeOfType<CreateValidationErrorResult>();
//		}
//		[Fact]
//		public async Task Should_return_error_if_join_trust_is_null_and_preferred_trust_is_yes()
//		{
//			CreateNewProjectDataCommand command = System_under_test();

//			var result = await command.Execute(new NewProject(_fixture.Create<NewProjectSchool>(), null, null, "yes", false));

//			result.Should().BeOfType<CreateValidationErrorResult>();
//		}
//	}
//}
