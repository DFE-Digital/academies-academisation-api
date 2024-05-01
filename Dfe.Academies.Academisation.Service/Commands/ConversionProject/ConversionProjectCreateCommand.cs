using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectCreateCommand : IRequest<CreateResult>
	{
		public NewProjectServiceModel Conversion { get; set; }
		public ConversionProjectCreateCommand(NewProjectServiceModel conversion)
		{
			Conversion = conversion;
		}
	}
}


