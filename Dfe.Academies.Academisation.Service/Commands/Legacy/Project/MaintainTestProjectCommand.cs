using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class MaintainTestProjectCommand
	{
		private readonly ITestProjectGetDataQuery _testProjectGetDataQuery;
		private readonly ITestProjectDeleteDataQuery _testProjectDeleteDataQuery;
		private readonly ITestProjectCreateDataCommand _testProjectCreateDataCommand;

		public MaintainTestProjectCommand(ITestProjectGetDataQuery testProjectGetDataQuery, ITestProjectCreateDataCommand testProjectCreateDataCommand, ITestProjectDeleteDataQuery testProjectDeleteDataQuery)
		{
			_testProjectGetDataQuery = testProjectGetDataQuery;
			_testProjectCreateDataCommand = testProjectCreateDataCommand;
			_testProjectDeleteDataQuery = testProjectDeleteDataQuery;
		}

		public async Task<CommandResult> Execute()
		{
			await FindAndDeleteExistingTestProjectIfPresent();
			await _testProjectCreateDataCommand.Execute();

			return new CommandSuccessResult();
		}

		private async Task FindAndDeleteExistingTestProjectIfPresent()
		{
			var existingProject = await _testProjectGetDataQuery.Execute("Cypress Project");
			if (existingProject is not null)
			{
				await _testProjectDeleteDataQuery.Execute(existingProject.Id);
			}
		}
	}
}
