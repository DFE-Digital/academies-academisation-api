using System;
using System.Collections.Generic;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionLegalRequirement;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts
{
	public class TestLegalRequirementContext : TestAcademisationContext
	{
		private readonly Fixture _fixture = new();

		public TestLegalRequirementContext()
		{
			SeedData();
		}

		protected override void SeedData()
		{
			using var context = CreateContext();
			context.Database.EnsureCreated();

			var timestamp = DateTime.UtcNow;

			var seed = new List<LegalRequirementState>
		{
			_fixture.Build<LegalRequirementState>()
				.With(d => d.Id, 1)
				.With(d => d.ProjectId, 1)
				.With(d => d.CreatedOn, timestamp)
				.With(d => d.LastModifiedOn, timestamp)
				.Create(),			
		};

			context.AddRange(seed);
			context.SaveChanges();
		}
	}
}
