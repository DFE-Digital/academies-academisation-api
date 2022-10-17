using AutoMapper;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	public class ApplicationGetDataQuery : IApplicationGetDataQuery
	{
		private readonly AcademisationContext _context;
		private readonly IMapper mapper;

		public ApplicationGetDataQuery(AcademisationContext context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		public async Task<IApplication?> Execute(int id)
		{
			var applicationState = await _context.Applications
				.AsNoTracking()
				.Include(a => a.Contributors)
				.Include(a => a.Schools)
					.ThenInclude(a => a.Loans)
				.Include(a => a.Schools)
					.ThenInclude(a => a.Leases)
				.Include(a => a.JoinTrust)
				.Include(a => a.FormTrust)
				.SingleOrDefaultAsync(a => a.Id == id);

			return applicationState?.MapToDomain(this.mapper);
		}
	}
}
