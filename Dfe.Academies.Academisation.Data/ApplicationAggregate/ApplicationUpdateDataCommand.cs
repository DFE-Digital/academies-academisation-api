using AutoMapper;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

public class ApplicationUpdateDataCommand : IApplicationUpdateDataCommand
{
	private readonly AcademisationContext _context;
	private readonly IMapper mapper;

	public ApplicationUpdateDataCommand(AcademisationContext context, IMapper mapper)
	{
		_context = context;
		this.mapper = mapper;
	}

	public async Task Execute(IApplication application)
	{
		var state = ApplicationState.MapFromDomain(application, this.mapper);

		await _context.Applications
			.Include(a => a.Contributors)
			.Include(a => a.Schools)
				.ThenInclude(a => a.Loans)
			.SingleOrDefaultAsync(a => a.Id == application.ApplicationId);

		_context.ReplaceTracked(state);

		await _context.SaveChangesAsync();
	}
}
