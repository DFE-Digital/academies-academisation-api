using Dfe.Academies.Academisation.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public class AcademisationModelSnapshotTests
{
	[Fact]
	public void ModelSnapshot_MatchesCurrentModel()
	{
		var options = new DbContextOptionsBuilder<AcademisationContext>()
			.UseSqlServer("Server=localhost;Database=SnapshotValidation;Integrated Security=true")
			.Options;
		using var context = new AcademisationContext(options, Mock.Of<IMediator>());

		Assert.False(context.Database.HasPendingModelChanges());
	}
}
