using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	/// The cy error handling command.
	/// </summary>
	public class CyErrorHandlingCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		/// Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new (string, object[])[]
		{
			("DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = {0}", new object[] { this.Id! })
		};

		public override bool HasValidArguments => !string.IsNullOrWhiteSpace(this.Id);
		public string? Id { get; set; }
	}
}
