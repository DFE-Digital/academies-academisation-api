using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class CreateNewProjectDataCommand : ICreateNewProjectDataCommand
	{
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		public CreateNewProjectDataCommand(IProjectCreateDataCommand projectCreateDataCommand)
		{
			_projectCreateDataCommand = projectCreateDataCommand;
		}

		public async Task<CreateResult> Execute(NewProject project)
		{
			var domainServiceResult = Project.CreateNewProject(project);

			switch (domainServiceResult)
			{
				case CreateValidationErrorResult createValidationErrorResult:
					return new CreateValidationErrorResult(createValidationErrorResult.ValidationErrors);
				case CreateSuccessResult<IProject> createSuccessResult:
					await _projectCreateDataCommand.Execute(createSuccessResult.Payload);
					return createSuccessResult;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
		}
	}
}

