using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.Application;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class TransferProjectQueryService : ITransferProjectQueryService
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IMapper _mapper;

		public TransferProjectQueryService(ITransferProjectRepository transferProjectRepository, IMapper mapper)
		{
			_transferProjectRepository = transferProjectRepository;
			_mapper = mapper;
		}

		public async Task<AcademyTransferProjectResponse?> GetByUrn(int id)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(id);
			
			return  AcademyTransferProjectResponseFactory.Create(transferProject);
		}

		public async Task<AcademyTransferProjectResponse?> GetById(int id)
		{
			var transferProject = await _transferProjectRepository.GetById(id);

			return AcademyTransferProjectResponseFactory.Create(transferProject);
		}


	}
}
