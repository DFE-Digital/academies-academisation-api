using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy create approved decision command.
	/// </summary>
	public class CyCreateApprovedDecisionCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		///     Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new[]
		{
			(@"DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = {0}",
				new object[] { Id! })
		};


		/// <summary>
		///     Gets a value indicating whether has valid arguments.
		/// </summary>
		public override bool HasValidArguments => !string.IsNullOrWhiteSpace(Id);

		/// <summary>
		///     Gets the id.
		/// </summary>
		public string? Id { get; set; }
	}
}
