namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public class TestProjectContext : TestAcademisationContext
{
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
