using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

public class ApplicationCreateDataCommand : IApplicationCreateDataCommand
{
	private readonly AcademisationContext _context;

	public ApplicationCreateDataCommand(AcademisationContext context)
	{
		_context = context;
	}

	public async Task Execute(IApplication application)
	{
		var applicationState = ApplicationState.MapFromDomain(application);

		_context.Applications.Add(applicationState);
		await _context.SaveChangesAsync();

		application.SetIdsOnCreate(
			applicationState.Id,
			applicationState.Contributors.Single().Id);
	}
}
