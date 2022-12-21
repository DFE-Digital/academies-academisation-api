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
			var applicationStateQuery = _context.Applications
				.Include(a => a.Contributors)
				.Include(a => a.Schools)
				.ThenInclude(a => a.Loans)
				.Include(a => a.Schools)
				.ThenInclude(a => a.Leases)
				.Include(a => a.JoinTrust)
				.Include(a => a.FormTrust)
				.ThenInclude(a => a.KeyPeople)
				.ThenInclude(a => a.Roles);

			return await applicationStateQuery.SingleOrDefaultAsync(a => a.Id == id);
		}
	}
}
