using MediatR;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public class TestProjectContext : TestAcademisationContext
{
	public TestProjectContext(IMediator mediator) : base(mediator)
	{
		Seed();
	}

	protected override void SeedData()
	{
		using var context = CreateContext();
		context.Database.EnsureCreated();
	}
}
