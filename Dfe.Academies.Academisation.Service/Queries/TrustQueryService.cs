using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Microsoft.EntityFrameworkCore;

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

		public async Task<TrustKeyPerson> GetTrustKeyPerson(int applicationId, int keyPersonId)
		{
			var applicationState = await _context.Applications
				.AsNoTracking()
				.Include(a => a.FormTrust)
				.ThenInclude(x => x.KeyPeople)
				.Include(a => a.Schools)
				.ThenInclude(a => a.Loans)
				.Where(a => a.Id == applicationId)
				.SingleOrDefaultAsync();

			var keyPerson =
				_mapper.Map<Domain.ApplicationAggregate.Trusts.TrustKeyPerson>(applicationState.FormTrust
					.KeyPeople.SingleOrDefault(kp => kp.Id == keyPersonId));

			return _mapper.Map<TrustKeyPerson>(keyPerson);
		}

		public async Task<List<TrustKeyPerson>> GetAllTrustKeyPeople(int applicationId)
		{
			var applicationState = await _context.Applications
				.AsNoTracking()
				.Include(a => a.FormTrust)
				.ThenInclude(x => x.KeyPeople)
				.Include(a => a.Schools)
				.ThenInclude(a => a.Loans)
				.Where(a => a.Id == applicationId)
				.SingleOrDefaultAsync();

			var keyPeople =
				_mapper.Map<List<Domain.ApplicationAggregate.Trusts.TrustKeyPerson>>(applicationState.FormTrust
					.KeyPeople);

			return _mapper.Map<List<TrustKeyPerson>>(keyPeople);
		}
	}
}
