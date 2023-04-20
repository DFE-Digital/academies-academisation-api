using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ApplicationQueryService : IApplicationQueryService
	{
		private readonly IApplicationRepository _applicationRepository;
		private readonly IMapper _mapper;

		public ApplicationQueryService(IApplicationRepository applicationRepository, IMapper mapper)
		{
			_applicationRepository = applicationRepository;
			_mapper = mapper;
		}

		public async Task<ApplicationServiceModel?> GetById(int id)
		{
			var application = await _applicationRepository.GetByIdAsync(id);
			return application?.MapFromDomain(_mapper);
		}

		public async Task<List<ApplicationServiceModel>> GetByUserEmail(string email)
		{
			var applications = await _applicationRepository.GetByUserEmail(email);

			return applications.Select(a => a.MapFromDomain(_mapper)).ToList();
		}

		public async Task<ApplicationServiceModel?> GetByApplicationReference(string applicationReference)
		{
			var application = await _applicationRepository.GetByApplicationReference(applicationReference);
			return application?.MapFromDomain(_mapper);
		}

		public async Task<List<ApplicationSchoolSharepointServiceModel>> GetAllApplications()
		{
			var applications = await _applicationRepository.GetAllAsync();
			return applications.Select(x => _mapper.Map<ApplicationSchoolSharepointServiceModel>(x)).ToList();
		}
	}
}
