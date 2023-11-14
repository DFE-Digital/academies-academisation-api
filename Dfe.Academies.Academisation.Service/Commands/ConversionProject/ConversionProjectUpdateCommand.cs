using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectUpdateCommand : IRequest<CommandResult>
	{
		public ConversionProjectUpdateCommand(int id, LegacyProjectServiceModel updateModel)
		{			
			Id = id;
			UpdateModel = updateModel;
		}
		
		public int Id { get; set; }
		public LegacyProjectServiceModel UpdateModel { get; set; }

	}
}
