using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project
{
	public class LegacyProjectDeleteNoteCommandTests
	{
		private readonly Mock<IConversionProjectRepository> _repo;
		private readonly Fixture _fixture;

		public LegacyProjectDeleteNoteCommandTests()
		{
			_fixture = new Fixture();
			_fixture.Customize(new AutoMoqCustomization());

			_repo = new Mock<IConversionProjectRepository>();

			_repo.Setup(x => x.UnitOfWork).Returns(Mock.Of<IUnitOfWork>());
		}

		private ConversionProjectDeleteNoteCommandHandler System_under_test()
		{
			return new ConversionProjectDeleteNoteCommandHandler(_repo.Object);
		}

		//[Fact]
		//public async Task Should_pass_the_request_to_the_data_layer_command()
		//{
		//	int projectId = Random.Shared.Next();
		//	var note = _fixture.Create<ConversionProjectDeleteNoteCommand>();

		//	note.ProjectId = projectId;

		//	var command = System_under_test();

		//	await command.Handle(note, default);

		//	_repo.Verify(
		//		x => x.Update(
		//			It.Is<Project>(n => n.Subject == note.Subject &&
		//									n.Note == note.Note &&
		//									n.Author == note.Author &&
		//									n.Date == note.Date)
		//		)
		//	);
		//}

		[Fact]
		public async Task Should_return_the_result_produced_by_the_data_layer_command()
		{
			var note = _fixture.Create<ProjectNote>();
			var project = _fixture.Create<IProject>();

			Mock.Get(project).Setup(x => x.Notes).Returns(new List<ProjectNote> { note });

			_repo.Setup(x => x.GetConversionProject(It.IsAny<int>())).ReturnsAsync(project);

			var command = System_under_test();

			CommandResult result = await command.Handle(new ConversionProjectDeleteNoteCommand()
			{
				Author = note.Author,
				Date = note.Date,
				Note = note.Note,
				ProjectId = note.ProjectId,
				Subject = note.Subject
			}, default);

			result.Should().BeOfType(typeof(CommandSuccessResult));
		}
	}
}
