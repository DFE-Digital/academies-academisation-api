using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class TrustQueryService : ITrustQueryService
	{
		private readonly AcademisationContext _context;
		private readonly IMapper _mapper;

		public TrustQueryService(AcademisationContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public Task<TrustKeyPerson> GetTrustKeyPerson(int applicationId, int keyPersonId)
		{
			//List<ApplicationState> applicationStates = await _context.Applications
			//	.AsNoTracking()
			//	.Include(a => a.Contributors)
			//	.Include(a => a.Schools)
			//	.ThenInclude(a => a.Loans)
			//	.Where(a => a.Contributors.Any(c => c.EmailAddress == userEmail))
			//	.ToListAsync();

			//return applicationStates.Select(a => a.MapToDomain(this.mapper)).ToList();
			return null;
		}

		public Task<List<TrustKeyPerson>> GetAllTrustKeyPeople(int applicationId)
		{
			//List<ApplicationState> applicationStates = await _context.Applications
			//	.AsNoTracking()
			//	.Include(a => a.Contributors)
			//	.Include(a => a.Schools)
			//	.ThenInclude(a => a.Loans)
			//	.Where(a => a.Contributors.Any(c => c.EmailAddress == userEmail))
			//	.ToListAsync();

			//return applicationStates.Select(a => a.MapToDomain(this.mapper)).ToList();
			return null;
		}
	}
}
