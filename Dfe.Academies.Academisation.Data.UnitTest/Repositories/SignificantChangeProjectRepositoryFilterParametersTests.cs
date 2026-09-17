
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.Repositories;

public class SignificantChangeProjectRepositoryFilterParametersTests : TestAcademisationContext
{
	public SignificantChangeProjectRepositoryFilterParametersTests() : base(new Mock<IMediator>().Object)
	{
	}

	protected override void SeedData()
	{
		using AcademisationContext context = CreateContext();
		context.Database.EnsureCreated();

		SignificantChangeProject assigned = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
			urn: 123456, tier: 2, trustName: "Trust A", trustUkprn: "10000001",
			typeOfSignificantChange: "Change of age range", schoolName: "School A"), createdOn: new DateTime(2026, 1, 1));
		assigned.AssignUser(Guid.NewGuid(), "assigned.user@test.local", "Assigned User");

		SignificantChangeProject unassigned = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
			urn: 654321, tier: 2, trustName: "Trust B", trustUkprn: "10000002",
			typeOfSignificantChange: "Change of age range", schoolName: "School B"), createdOn: new DateTime(2026, 1, 2));

		context.AddRange(assigned, unassigned);
		context.SaveChanges();
	}

	[Fact]
	public async Task GetFilterParameters_ReturnsEveryStatus_EvenWhenNoProjectHasIt()
	{
		Seed();
		using AcademisationContext context = CreateContext();
		SignificantChangeProjectRepository sut = new(context);

		SignificantChangeFilterParameters result = await sut.GetFilterParameters(CancellationToken.None);

		result.Statuses.Select(status => status.Value)
			.Should().BeEquivalentTo(Enum.GetNames<SignificantChangeStatus>());
		result.Statuses.Should().Contain(status => status.Value == "PreDecision" && status.Display == "Pre decision");
	}

	[Fact]
	public async Task GetFilterParameters_ReturnsAllThreeTiers_EvenThoughOnlyTierTwoIsSeeded()
	{
		Seed();
		using AcademisationContext context = CreateContext();
		SignificantChangeProjectRepository sut = new(context);

		SignificantChangeFilterParameters result = await sut.GetFilterParameters(CancellationToken.None);

		result.Tiers.Should().BeEquivalentTo(new List<FilterValueDisplay>
		{
			new("1", "Tier 1"),
			new("2", "Tier 2"),
			new("3", "Tier 3")
		}, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task GetFilterParameters_ReturnsDistinctAssignedUsers_ExcludingUnassigned()
	{
		Seed();
		using AcademisationContext context = CreateContext();
		SignificantChangeProjectRepository sut = new(context);

		SignificantChangeFilterParameters result = await sut.GetFilterParameters(CancellationToken.None);

		result.AssignedUsers.Should().ContainSingle();
		result.AssignedUsers[0].Value.Should().Be("Assigned User");
		result.AssignedUsers[0].Display.Should().Be("Assigned User");
	}

	[Fact]
	public async Task GetFilterParameters_ReturnsDistinctRoutes_WithValueEqualToDisplay()
	{
		Seed();
		using AcademisationContext context = CreateContext();
		SignificantChangeProjectRepository sut = new(context);

		SignificantChangeFilterParameters result = await sut.GetFilterParameters(CancellationToken.None);

		result.Routes.Should().ContainSingle();
		result.Routes[0].Value.Should().Be("Change of age range");
		result.Routes[0].Display.Should().Be("Change of age range");
	}

	[Fact]
	public async Task GetProjectsToSendToComplete_ReturnsOnlyReadOnlyProjectsThatHaveNotBeenSent()
	{
		Seed();
		using AcademisationContext context = CreateContext();
		SignificantChangeProject eligible = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
			111111, 2, "Trust C", "10000003", "Change of age range", "School C"), DateTime.UtcNow);
		eligible.SetReadOnlyDate(DateTime.UtcNow);
		SignificantChangeProject notReadOnly = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
			222222, 2, "Trust D", "10000004", "Change of age range", "School D"), DateTime.UtcNow);
		SignificantChangeProject alreadySent = SignificantChangeProject.Create(new SignificantChangeProjectOptions(
			333333, 2, "Trust E", "10000005", "Change of age range", "School E"), DateTime.UtcNow);
		alreadySent.SetReadOnlyDate(DateTime.UtcNow);
		alreadySent.SetProjectSentToComplete(Guid.NewGuid());
		context.AddRange(eligible, notReadOnly, alreadySent);
		await context.SaveChangesAsync();

		SignificantChangeProjectRepository sut = new(context);

		var result = await sut.GetProjectsToSendToCompleteAsync(CancellationToken.None);

		result.Should().ContainSingle(project => project.Id == eligible.Id);
	}
}