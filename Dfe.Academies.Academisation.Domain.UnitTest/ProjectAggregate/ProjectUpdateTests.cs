using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using FluentAssertions;
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
		var updatedProject = _fixture.Build<ProjectDetails>()
			.With(p => p.Urn, initialProject.Urn).Without(x => x.ConversionSupportGrantChangeReason).Create();
		updatedProject.Notes.Clear();

		// Act
		var result = sut.Update(updatedProject);

		// Assert
		Assert.Multiple(
			() => Assert.IsType<CommandSuccessResult>(result),
			() => Assert.True(updatedProject.Equals(sut.Details))
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
	public static IEnumerable<object[]> TypeChangedData => new List<object[]>
		{
			new object[] { "fast track", "intermediate", 70000m, 90000m, false, "primary" },
			new object[] { "fast track", "full", 70000m, 110000m , false, "primary" },
			new object[] { "intermediate", "full", 90000m, 110000m, false , "primary" }
		};

	[Theory]
	[MemberData(nameof(TypeChangedData))]
	public void CalculateDefaultSponsoredGrant_TypeChanged_ReturnsNewDefault(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}

	public static IEnumerable<object[]> TypeUnchangedData => new List<object[]>
		{
			new object[] { "fast track", "fast track", 70000m, 70000m , false, "primary" },
			new object[] { "intermediate", "intermediate", 90000m, 90000m, false, "primary" },
			new object[] { "full", "full", 110000m, 110000m, false , "primary" }
		};

	[Theory]
	[MemberData(nameof(TypeUnchangedData))]
	public void CalculateDefaultSponsoredGrant_TypeUnchanged_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}
	public static IEnumerable<object[]> DefaultWantedCheckedData => new List<object[]>
	{
		new object[] { "fast track", "fast track", 80000m, 70000m , true, "primary" },
		new object[] { "intermediate", "intermediate", 70000m, 90000m, true , "primary" },
		new object[] { "full", "full", 100000m, 110000m, true , "primary" }
	};

	[Theory]
	[MemberData(nameof(DefaultWantedCheckedData))]
	public void CalculateDefaultSponsoredGrant_DefaultWantedChecked_ReturnsDefaults(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}

	public static IEnumerable<object[]> EmptyExistingTypeData => new List<object[]>
		{
			new object[] { null, "fast track", null, 70000m, false, "primary"},
			new object[] { null, "intermediate", null, 90000m, false , "primary" },
			new object[] { null, "full", null, 110000m, false , "primary" }
		};

	[Theory]
	[MemberData(nameof(EmptyExistingTypeData))]
	public void CalculateDefaultSponsoredGrant_EmptyExistingType_ReturnsNewDefault(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}
	public static IEnumerable<object[]> EmptyExistingTypeDataSecondary => new List<object[]>
	{
		new object[] { null, "fast track", null, 80000m, false, "secondary"},
		new object[] { null, "intermediate", null, 115000m, false , "secondary" },
		new object[] { null, "full", null, 150000m, false , "secondary" }
	};

	[Theory]
	[MemberData(nameof(EmptyExistingTypeDataSecondary))]
	public void CalculateDefaultSponsoredGrantSecondary_EmptyExistingType_ReturnsNewDefault(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}

	public static IEnumerable<object[]> NullNewTypeData => new List<object[]>
		{
			new object[] { "fast track", null, 70000m, 70000m , false , "primary" },
			new object[] { "intermediate", null, 90000m, 90000m, false , "primary" },
			new object[] { "full", null, 110000m, 110000m, false , "primary" }
		};

	[Theory]
	[MemberData(nameof(NullNewTypeData))]
	public void CalculateDefaultSponsoredGrant_NullNewType_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}

	public static IEnumerable<object[]> InvalidNewTypeData => new List<object[]>
		{
			new object[] { "fast track", "invalid", 70000m, 70000m, false, "primary" }
		};

	[Theory]
	[MemberData(nameof(InvalidNewTypeData))]
	public void CalculateDefaultSponsoredGrant_InvalidNewType_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}

	public static IEnumerable<object[]> NullExistingAndNewTypeData => new List<object[]>
		{
			new object[] { null, null, 50000m, 50000m, false, "primary" }
		};

	[Theory]
	[MemberData(nameof(NullExistingAndNewTypeData))]
	public void CalculateDefaultSponsoredGrant_NullExistingAndNewType_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected, bool amountChangedFromDefault, string? phase)
	{
		var result = Project.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount, amountChangedFromDefault, phase);
		result.Should().Be(expected);
	}
}
