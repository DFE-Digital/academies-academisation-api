using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class CreateInvoluntaryProjectCommand : ICreateInvoluntaryProjectCommand
	{
		
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;


		public CreateInvoluntaryProjectCommand(IProjectCreateDataCommand projectCreateDataCommand)
		{
			_projectCreateDataCommand = projectCreateDataCommand;
		}

		public async Task<CommandResult> Execute(InvoluntaryProject project)
		{
			var domainServiceResult = Project.CreateInvoluntaryProject(project);

			switch (domainServiceResult)
			{
				case CreateValidationErrorResult createValidationErrorResult:
					return new CommandValidationErrorResult(createValidationErrorResult.ValidationErrors);
				case CreateSuccessResult<IProject> createSuccessResult:
					await _projectCreateDataCommand.Execute(createSuccessResult.Payload);
					return new CommandSuccessResult();
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}



		}
	}
}

