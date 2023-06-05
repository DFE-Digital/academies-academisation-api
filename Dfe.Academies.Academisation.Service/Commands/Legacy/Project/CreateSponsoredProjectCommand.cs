using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class CreateSponsoredProjectCommand : ICreateSponsoredProjectCommand
	{
		private readonly IMapper _mapper;
		private readonly ICreateSponsoredProjectDataCommand _createSponsoredProjectDataCommand;

		public CreateSponsoredProjectCommand(
			IMapper mapper,
			ICreateSponsoredProjectDataCommand createSponsoredProjectDataCommand)
		{
			_mapper = mapper;
			_createSponsoredProjectDataCommand = createSponsoredProjectDataCommand;
		}

		public async Task<CommandResult> Execute(SponsoredProjectServiceModel model)
		{
			var result = await _createSponsoredProjectDataCommand.Execute(_mapper.Map<SponsoredProject>(model));

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
