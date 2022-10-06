using AutoMapper;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

public class ApplicationCreateDataCommand : IApplicationCreateDataCommand
{
	private readonly AcademisationContext _context;
	private readonly IMapper mapper;

	public ApplicationCreateDataCommand(AcademisationContext context, IMapper mapper)
	{
		_context = context;
		this.mapper = mapper;
	}

	public async Task Execute(IApplication application)
	{
		var applicationState = ApplicationState.MapFromDomain(application, this.mapper);

		_context.Applications.Add(applicationState);
		await _context.SaveChangesAsync();

		application.SetIdsOnCreate(
			applicationState.Id,
			applicationState.Contributors.Single().Id);
	}
}
