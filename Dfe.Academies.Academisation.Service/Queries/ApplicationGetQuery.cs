using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Mappers;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ApplicationGetQuery : IApplicationGetQuery
	{
		private readonly IApplicationRepository _applicationRepository;
		private readonly IMapper _mapper;

		public ApplicationGetQuery(IApplicationRepository applicationRepository, IMapper mapper)
		{
			_applicationRepository = applicationRepository;
			_mapper = mapper;
		}

		public async Task<ApplicationServiceModel?> Execute(int id)
		{
			var application = await _applicationRepository.GetByIdAsync(id);
			return application?.MapFromDomain(_mapper);
		}
	}
}
