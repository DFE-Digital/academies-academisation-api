using AutoMapper;
using Dfe.Academies.Academisation.Data;
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

		public async Task<TrustKeyPersonServiceModel> GetTrustKeyPerson(int applicationId, int keyPersonId)
		{
			var applicationState = await _context.Applications
				.AsNoTracking()
				.Include(a => a.FormTrust)
				.ThenInclude(x => x.KeyPeople)
				.ThenInclude(x => x.Roles)
				.Include(a => a.Schools)
				.ThenInclude(a => a.Loans)
				.Where(a => a.Id == applicationId)
				.SingleOrDefaultAsync();

			var keyPerson =
				_mapper.Map<Domain.ApplicationAggregate.Trusts.TrustKeyPerson>(applicationState.FormTrust
					.KeyPeople.SingleOrDefault(kp => kp.Id == keyPersonId));

			return _mapper.Map<TrustKeyPersonServiceModel>(keyPerson);
		}

		public async Task<List<TrustKeyPersonServiceModel>> GetAllTrustKeyPeople(int applicationId)
		{
			var applicationState = await _context.Applications
				.AsNoTracking()
				.Include(a => a.FormTrust)
				.ThenInclude(x => x.KeyPeople)
				.ThenInclude(x => x.Roles)
				.Include(a => a.Schools)
				.ThenInclude(a => a.Loans)
				.Where(a => a.Id == applicationId)
				.SingleOrDefaultAsync();

			var keyPeople =
				_mapper.Map<List<Domain.ApplicationAggregate.Trusts.TrustKeyPerson>>(applicationState.FormTrust
					.KeyPeople);

			return _mapper.Map<List<TrustKeyPersonServiceModel>>(keyPeople);
		}
	}
}
