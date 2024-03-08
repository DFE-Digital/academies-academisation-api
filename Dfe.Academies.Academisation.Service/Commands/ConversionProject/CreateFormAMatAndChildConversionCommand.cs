using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class CreateFormAMatAndChildConversionCommand : IRequest<CommandResult>
	{
		public NewProjectServiceModel Conversion { get; set; }
		public CreateFormAMatAndChildConversionCommand(NewProjectServiceModel conversion)
		{
			Conversion = conversion;
		}
	}
}


