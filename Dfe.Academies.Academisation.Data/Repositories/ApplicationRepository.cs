using AutoMapper;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	//TODO: Change the AcademisationContext Applications return an Application entity type instead of mapping it to a domain
	public class ApplicationRepository : IApplicationRepository
	{
		private readonly IMapper _mapper;
		private readonly AcademisationContext _context;

		public ApplicationRepository(AcademisationContext context, IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper;
		}

		public IUnitOfWork UnitOfWork => _context;
		public async Task<IEnumerable<Application>> GetAllAsync()
		{
			return await _context.Applications.ToListAsync();
		}
		
		public async Task<Application?> GetByIdAsync(object id)
		{
			return (await _context.Applications
				.Include(x => x.Schools)
				.ThenInclude(x => x.Loans)
				.Include(x => x.Schools)
				.ThenInclude(x => x.Leases)
				.Include(x => x.FormTrust)
				.ThenInclude(x => x.KeyPeople)
				.ThenInclude(x => x.Roles)
				.FirstOrDefaultAsync(x => x.Id == (int)id));
		}

		public async Task Insert(Application obj)
		{
			await _context.Applications.AddAsync(obj);
		}

		public void Update(Application obj)
		{
			_context.Update(obj);
		}

		public async Task Delete(object id)
		{
			var entity = await _context.Applications.FindAsync(id);
			if(entity != null)
				_context.Applications.Remove(entity);
		}

		public async Task DeleteChildObjectById<T>(object id) where T : class
		{
			 var entity = await _context.FindAsync<T>(id);
			 _context.Remove(entity);
		}
	}
}
