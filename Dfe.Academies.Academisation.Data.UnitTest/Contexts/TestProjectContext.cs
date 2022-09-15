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
	}
}
