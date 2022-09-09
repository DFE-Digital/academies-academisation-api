using System;
using System.Collections.Generic;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public class TestProjectContext : TestAcademisationContext
{
	private readonly Fixture _fixture = new();

	public TestProjectContext()
	{
		Seed();
	}

	protected override void SeedData()
	{
		using var context = CreateContext();
		context.Database.EnsureCreated();

		var timestamp = DateTime.UtcNow;

		var seed = new List<ProjectState>
		{
			_fixture.Build<ProjectState>()
				.With(d => d.Id, 1)
				.With(d => d.CreatedOn, timestamp)
				.With(d => d.LastModifiedOn, timestamp)
				.Create(),
			_fixture.Build<ProjectState>()
				.With(d => d.Id, 2)
				.With(d => d.CreatedOn, timestamp)
				.With(d => d.LastModifiedOn, timestamp)
				.Create(),
			_fixture.Build<ProjectState>()
				.With(d => d.Id, 3)
				.With(d => d.CreatedOn, timestamp)
				.With(d => d.LastModifiedOn, timestamp)
				.Create()
		};

		context.AddRange(seed);
		context.SaveChanges();
	}
}
