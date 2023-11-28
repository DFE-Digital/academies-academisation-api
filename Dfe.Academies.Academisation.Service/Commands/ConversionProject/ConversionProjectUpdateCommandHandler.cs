using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject;

public class ConversionProjectUpdateCommandHandler : IRequestHandler<ConversionProjectUpdateCommand, CommandResult>
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly IProjectUpdateDataCommand _projectUpdateDataCommand;

	public ConversionProjectUpdateCommandHandler(IConversionProjectRepository conversionProjectRepository, IProjectUpdateDataCommand projectUpdateDataCommand)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_projectUpdateDataCommand = projectUpdateDataCommand;
	}

	public async Task<CommandResult> Handle(ConversionProjectUpdateCommand request, CancellationToken cancellation)
	{
		var existingProject = await _conversionProjectRepository.GetConversionProject(request.UpdateModel.Id);

		if (existingProject is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingProject.Update(request.UpdateModel.MapNonEmptyFields(existingProject));

		if (result is CommandValidationErrorResult)
		{
			return result;
		}
		if (result is not CommandSuccessResult)
		{
			throw new NotImplementedException();
		}

		await _projectUpdateDataCommand.Execute(existingProject);

		return result;
	}
}
