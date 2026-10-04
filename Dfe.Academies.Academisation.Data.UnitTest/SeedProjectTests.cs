using System.Linq;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Seed;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest;

public class SeedProjectTests
{
	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public async Task CreateProject_WhenCountIsNotPositive_DoesNotCreateProjects(int projectsPerType)
	{
		using var testContext = new TestProjectContext(new Mock<IMediator>().Object);
		await using var context = testContext.CreateContext();

		await SeedProject.CreateProject(context, projectsPerType);

		Assert.Equal(0, await context.Projects.CountAsync());
		Assert.Equal(0, await context.TransferProjects.CountAsync());
		Assert.Equal(0, await context.SignificantChangeProjects.CountAsync());
		Assert.Equal(0, await context.ProjectNotes.CountAsync());
	}

	[Fact]
	public async Task CreateProject_PersistsRequestedProjectsAndLinkedNotes()
	{
		const int projectsPerType = 2;
		using var testContext = new TestProjectContext(new Mock<IMediator>().Object);
		await using var context = testContext.CreateContext();

		await SeedProject.CreateProject(context, projectsPerType);

		var conversionProjectIds = await context.Projects.Select(project => project.Id).ToListAsync();
		var noteProjectIds = await context.ProjectNotes.Select(note => note.ProjectId).ToListAsync();

		Assert.Equal(projectsPerType, conversionProjectIds.Count);
		Assert.Equal(projectsPerType, await context.TransferProjects.CountAsync());
		Assert.Equal(projectsPerType, await context.SignificantChangeProjects.CountAsync());
		Assert.Equal(projectsPerType, noteProjectIds.Count);
		Assert.Equal(conversionProjectIds.Order(), noteProjectIds.Order());
	}

	[Fact]
	public async Task CreateProject_WhenCountExceedsBatchSize_PersistsAllProjectsAndNotes()
	{
		const int projectsPerType = 41;
		using var testContext = new TestProjectContext(new Mock<IMediator>().Object);
		await using var context = testContext.CreateContext();

		await SeedProject.CreateProject(context, projectsPerType);

		Assert.Equal(projectsPerType, await context.Projects.CountAsync());
		Assert.Equal(projectsPerType, await context.TransferProjects.CountAsync());
		Assert.Equal(projectsPerType, await context.SignificantChangeProjects.CountAsync());
		Assert.Equal(projectsPerType, await context.ProjectNotes.CountAsync());
	}
}
