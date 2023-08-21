using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using AutoMapper;
using Dfe.Academies.Academisation.Data.Migrations;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
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
 
		public async Task<PagedResultResponse<AcademyTransferProjectSummaryResponse>> GetTransferProjects(int page, int count, int? urn,
        string title)
        {
			IEnumerable<ITransferProject> transferProjects = FilterByUrn(
            await _transferProjectRepository.GetAllTransferProjects(), urn).ToList();
	
			//the logic retrieving the trust data goes here
			IEnumerable<AcademyTransferProjectSummaryResponse> projects = null;
            //this is placeholder code
            var recordTotal = projects.Count();
			
			return await Task.FromResult(new PagedResultResponse<AcademyTransferProjectSummaryResponse>(null, recordTotal));
		}
			
		private static IEnumerable<ITransferProject> FilterByUrn(IEnumerable<ITransferProject> queryable,
        int? urn)
        {
         if (urn.HasValue) queryable = queryable.Where(p => p.Urn == urn);

         return queryable;
        
		}
	}

}
