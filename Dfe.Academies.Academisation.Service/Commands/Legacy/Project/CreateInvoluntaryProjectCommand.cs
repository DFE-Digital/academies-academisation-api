using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class CreateInvoluntaryProjectCommand : ICreateInvoluntaryProjectCommand
	{
		private readonly IMapper _mapper;
		private readonly ICreateInvoluntaryProjectDataCommand _createInvoluntaryProjectDataCommand;

		public CreateInvoluntaryProjectCommand(
			IMapper mapper,
			ICreateInvoluntaryProjectDataCommand createInvoluntaryProjectDataCommand)
		{
			_mapper = mapper;
			_createInvoluntaryProjectDataCommand = createInvoluntaryProjectDataCommand;
		}

		public async Task<CommandResult> Execute(InvoluntaryProjectServiceModel model)
		{
			var result = await _createInvoluntaryProjectDataCommand.Execute(_mapper.Map<InvoluntaryProject>(model));

			if (result is CommandValidationErrorResult)
			{
				return result;
			}
			if (result is not CommandSuccessResult)
			{
				throw new NotImplementedException();
			}

			return result;
		}
	}
}
