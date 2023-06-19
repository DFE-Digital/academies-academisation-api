using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	public class CyCreateDeferredDecisionCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		/// Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new (string, object[])[]
		{
			("DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = {0}", new object[] {this.Id})
		};

		public override bool HasValidArguments => !string.IsNullOrEmpty(this.Id);

		public string Id { get; set; }
	}
}
