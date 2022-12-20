using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
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

		public async Task<IList<Application>> Execute(string userEmail)
		{
			var applicationStates = await _context.Applications
				.AsNoTracking()
				.Include(a => a.Contributors)
				.Include(a => a.Schools)
					.ThenInclude(a => a.Loans)
				.Where(a => a.Contributors.Any(c => c.Details.EmailAddress == userEmail))
				.ToListAsync();

			return applicationStates.ToList();
		}
	}
}
