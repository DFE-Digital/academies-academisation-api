using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands;

namespace Dfe.Academies.Academisation.Service.Commands
{
	public class ApplicationSubmitCommand : IApplicationSubmitCommand
	{
		private readonly IApplicationGetDataQuery _dataQuery;
		private readonly IApplicationUpdateDataCommand _dataCommand;

		public ApplicationSubmitCommand(IApplicationGetDataQuery dataQuery, IApplicationUpdateDataCommand dataCommand)
		{
			_dataQuery = dataQuery;
			_dataCommand = dataCommand;
		}

		public async Task<CommandResult> Execute(int applicationId)
		{
			var application = await _dataQuery.Execute(applicationId);

			var domainResult = application.Submit();

			switch (domainResult)
			{
				case CommandValidationErrorResult:
					return domainResult;
				case CommandSuccessResult:
					break;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}

			await _dataCommand.Execute(application);
			return domainResult;
		}
	}
}
