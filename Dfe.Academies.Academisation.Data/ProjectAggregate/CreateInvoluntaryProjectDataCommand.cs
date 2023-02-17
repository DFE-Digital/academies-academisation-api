using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class CreateInvoluntaryProjectDataCommand : ICreateInvoluntaryProjectDataCommand
	{
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		public CreateInvoluntaryProjectDataCommand(IProjectCreateDataCommand projectCreateDataCommand)
		{
			_projectCreateDataCommand = projectCreateDataCommand;
		}

		public async Task<CommandResult> Execute(Core.ProjectAggregate.InvoluntaryProject project)
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

