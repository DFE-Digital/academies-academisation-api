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
			return await _context.Applications.Select(x => x.MapToDomain(_mapper)).ToListAsync();
		}
		
		public async Task<Application?> GetByIdAsync(object id)
		{
			return (await _context.Applications.FindAsync(id))?.MapToDomain(_mapper);
		}

		public async Task Insert(Application obj)
		{
			await _context.Applications.AddAsync(ApplicationState.MapFromDomain(obj, _mapper));
		}

		public void Update(Application obj)
		{
			_context.Applications.Update(ApplicationState.MapFromDomain(obj, _mapper));
		}

		public async Task Delete(object id)
		{
			var entity = await _context.Applications.FindAsync(id);
			
			if(entity != null)
				_context.Applications.Remove(entity);
		}
	}
}
