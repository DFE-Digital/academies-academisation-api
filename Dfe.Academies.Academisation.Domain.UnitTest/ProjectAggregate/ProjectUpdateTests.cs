using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ProjectAggregate;

public class ProjectUpdateTests
{
	private readonly Fixture _fixture = new();

	[Fact]
	public void Update___ReturnsUpdateSuccessResult_AndSetsProjectDetails()
	{
		// Arrange
		var initialProject = _fixture.Create<ProjectDetails>();
		var sut = new Project(1, initialProject);
		var updatedProject = _fixture.Build<ProjectDetails>().With(p => p.Urn, initialProject.Urn).Create();
		updatedProject.Notes.Clear();

		// Act
		var result = sut.Update(updatedProject);

		// Assert
		Assert.Multiple(
			() => Assert.IsType<CommandSuccessResult>(result),
			() => Assert.Equivalent(updatedProject, sut.Details)
		);
	}

	[Fact]
	public void Update_WithDifferentUrn__ReturnsCommandValidationErrorResult_AndDoesNotUpdateProjectDetails()
	{
		// Arrange
		var existingProject = _fixture.Create<ProjectDetails>();
		var sut = new Project(1, existingProject);
		var updatedProject = new ProjectDetails { Urn = 1 };

		// Act
		var result = sut.Update(updatedProject);

		// Assert
		var validationErrors = Assert.IsType<CommandValidationErrorResult>(result).ValidationErrors;

		Assert.Multiple(
			() => Assert.Equal("Urn", validationErrors.First().PropertyName),
			() => Assert.Equal("Urn in update model must match existing record", validationErrors.First().ErrorMessage),
			() => Assert.Equivalent(existingProject, sut.Details)
		);
	}
}
