using AutoMapper;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	public class ApplicationsListByUserDataQuery : IApplicationsListByUserDataQuery
	{
		private readonly AcademisationContext _context;
		private readonly IMapper mapper;

		public ApplicationsListByUserDataQuery(AcademisationContext context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		public async Task<IList<IApplication>> Execute(string userEmail)
		{
			List<ApplicationState> applicationStates = await _context.Applications
				.AsNoTracking()
				.Include(a => a.Contributors)
				.Include(a => a.Schools)
					.ThenInclude(a => a.Loans)
				.Where(a => a.Contributors.Any(c => c.EmailAddress == userEmail))
				.ToListAsync();

			return applicationStates.Select(a => a.MapToDomain(this.mapper)).ToList();
		}
	}
}
