using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
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
		public async Task<IEnumerable<IApplication>> GetAllAsync()
		{
			return await DefaultIncludes().ToListAsync();
		}
		
		public async Task<IApplication?> GetByIdAsync(object id)
		{
			return (await DefaultIncludes()
				.FirstOrDefaultAsync(x => x.Id == (int)id));
		}

		public async Task Insert(IApplication obj)
		{
			await _context.Applications.AddAsync(obj as Application);
		}

		public void Update(IApplication obj)
		{
			_context.Entry(obj as Application).State = EntityState.Modified;
			_context.Update(obj as Application);
		}

		public async Task Delete(object id)
		{
			var entity = await _context.Applications.FindAsync(id);
			if(entity != null)
				_context.Applications.Remove(entity);
		}

		public async Task<List<IApplication>> GetByUserEmail(string userEmail)
		{
			var applications = await DefaultIncludes()
				.Where(a => a.Contributors.Any(c => c.Details.EmailAddress == userEmail))
				.Cast<IApplication>()
				.ToListAsync();

			return applications;
		}

		public async Task<Application?> GetByApplicationReference(string applicationReference)
		{
			return await DefaultIncludes().Where(x => x.ApplicationReference == applicationReference).FirstOrDefaultAsync();
		}

		private IQueryable<Application> DefaultIncludes()
		{
			var x =  _context.Applications
				//.Include(x => x.Contributors)
				//.Include(x => x.Schools)
				//.ThenInclude(x => x.Loans)
				//.Include(x => x.Schools)
				//.ThenInclude(x => x.Leases)
				//.Include(x => x.JoinTrust)
				//.Include(x => x.FormTrust)
				//.ThenInclude(x => x.KeyPeople)
				//.ThenInclude(x => x.Roles)
				.AsQueryable();

			return x;
		}
	}
}
