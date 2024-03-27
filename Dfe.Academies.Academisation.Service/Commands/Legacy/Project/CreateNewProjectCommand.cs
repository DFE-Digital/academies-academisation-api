using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class CreateNewProjectCommand : ICreateNewProjectCommand
	{
		private readonly IMapper _mapper;
		private readonly ICreateNewProjectDataCommand _createNewProjectDataCommand;

		public CreateNewProjectCommand(
			IMapper mapper,
			ICreateNewProjectDataCommand createSponsoredProjectDataCommand)
		{
			_mapper = mapper;
			_createNewProjectDataCommand = createSponsoredProjectDataCommand;
		}

		public async Task<CreateResult> Execute(NewProjectServiceModel model)
		{
			var result = await _createNewProjectDataCommand.Execute(_mapper.Map<NewProject>(model));

			if (result is CreateValidationErrorResult)
			{
				return result;
			}
			if (result is not CreateSuccessResult<IProject>)
			{
				throw new NotImplementedException();
			}

			return result;
		}
	}
}
