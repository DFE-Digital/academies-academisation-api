using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

public class ApplicationUpdateDataCommand : IApplicationUpdateDataCommand
{
	private readonly AcademisationContext _context;

	public ApplicationUpdateDataCommand(AcademisationContext context)
	{
		_context = context;
	}

	public async Task Execute(IApplication application)
	{
		ApplicationState state = ApplicationState.MapFromDomain(application);

		await _context.Applications
			.Include(a => a.Contributors)
			.Include(a => a.Schools)
			.SingleOrDefaultAsync(a => a.Id == application.ApplicationId);

		_context.ReplaceTracked(state);

		await _context.SaveChangesAsync();
	}
}
