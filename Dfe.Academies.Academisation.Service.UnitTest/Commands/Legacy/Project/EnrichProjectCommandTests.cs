using AutoFixture;
using Dfe.Academies.Academisation.IData.Establishment;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project
{
	public class EnrichProjectCommandTests
	{
		private readonly EnrichProjectCommand _subject;
		private readonly Mock<IIncompleteProjectsGetDataQuery> _incompleteProjectGetDataQuery;
		private readonly Mock<IEstablishmentGetDataQuery> _establishmentGetDataQuery;
		private readonly Mock<IProjectUpdateDataCommand> _updateCommand;
		private readonly Mock<ILogger<EnrichProjectCommand>> _logger;

		private readonly Fixture _fixture = new();

		public EnrichProjectCommandTests()
		{
			_incompleteProjectGetDataQuery = new Mock<IIncompleteProjectsGetDataQuery>();
			_establishmentGetDataQuery = new Mock<IEstablishmentGetDataQuery>();
			_updateCommand = new Mock<IProjectUpdateDataCommand>();
			_logger = new Mock<ILogger<EnrichProjectCommand>>();

			_subject = new EnrichProjectCommand(
				_logger.Object,
				_incompleteProjectGetDataQuery.Object,
				_establishmentGetDataQuery.Object,
				_updateCommand.Object);
		}

		[Fact]
		public async Task NoIncompleteProjects__LogAndReturn()
		{
			await _subject.Execute();

			Assert.Multiple(
				() => VerifyLogging(_logger, "No projects requiring enrichment found."),
				() => _establishmentGetDataQuery.VerifyNoOtherCalls(),
				() => _updateCommand.VerifyNoOtherCalls());

		}

		[Fact]
		public async Task IncompleteProjects__UnknownSchool__LogAndContinue()
		{
			var projects = _fixture.CreateMany<Domain.ProjectAggregate.Project>();
			_incompleteProjectGetDataQuery.Setup(m => m.GetIncompleteProjects())
				.ReturnsAsync(projects);

			_establishmentGetDataQuery.SetupSequence(m => m.GetEstablishment(It.IsAny<int>()))
				.ReturnsAsync((Establishment?)null)
				.ReturnsAsync(_fixture.Create<Establishment>())
				.ReturnsAsync(_fixture.Create<Establishment>());

			await _subject.Execute();

			Assert.Multiple(
				() => VerifyLogging(_logger, $"No schools found for project - {projects.First().Id}, urn - {projects.First().Details.Urn}",
					LogLevel.Warning),
				() => _updateCommand.Verify(m => m.Execute(It.IsAny<IProject>()), Times.Exactly(2)));

		}

		[Fact]
		public async Task IncompleteProjects__KnownSchool__UpdateDb()
		{
			var project = _fixture.Create<Domain.ProjectAggregate.Project>();
			_incompleteProjectGetDataQuery.Setup(m => m.GetIncompleteProjects())
				.ReturnsAsync(new List<IProject> { project });

			_establishmentGetDataQuery.Setup(m => m.GetEstablishment(It.IsAny<int>()))
				.ReturnsAsync(_fixture.Create<Establishment>());

			await _subject.Execute();

			_updateCommand.Verify(m => m.Execute(project), Times.Once);
		}

		public static Mock<ILogger<T>> VerifyLogging<T>(Mock<ILogger<T>> logger, string? expectedMessage, LogLevel expectedLogLevel = LogLevel.Information, Times? times = null)
		{
			times ??= Times.Once();

			Func<object, Type, bool> state = (v, t) => v?.ToString()?.CompareTo(expectedMessage) == 0;

			logger.Verify(
				x => x.Log(
					It.Is<LogLevel>(l => l == expectedLogLevel),
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => state(v, t)),
					It.IsAny<Exception>(),
					It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), (Times)times);

			return logger;
		}
	}
}
