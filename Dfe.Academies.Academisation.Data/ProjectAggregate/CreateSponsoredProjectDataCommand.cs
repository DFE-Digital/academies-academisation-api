using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class CreateSponsoredProjectDataCommand : ICreateSponsoredProjectDataCommand
	{
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		public CreateSponsoredProjectDataCommand(IProjectCreateDataCommand projectCreateDataCommand)
		{
			_projectCreateDataCommand = projectCreateDataCommand;
		}

		public async Task<CommandResult> Execute(SponsoredProject project)
		{
			var domainServiceResult = Project.CreateSponsoredProject(project);

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

