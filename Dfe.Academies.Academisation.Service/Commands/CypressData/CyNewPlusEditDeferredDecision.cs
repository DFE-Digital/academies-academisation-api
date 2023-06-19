using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	/// The cy new plus edit deferred decision command.
	/// </summary>
	public class CyNewPlusEditDeferredDecision : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		public override IEnumerable<(string, object[])> SqlStatements => new (string, object[])[]
		{
			(@"delete from academisation.ConversionAdvisoryBoardDecisionDeferredReason where AdvisoryBoardDecisionId = (select Id from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = {0})", new object[] {this.Id}),
			(@"delete from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = {0}", new object[]{this.Id}),
			(@"insert into academisation.ConversionAdvisoryBoardDecision values ({0}, 'Deferred', null, null, {1}, 'None', {2}, {3})", new object[]{ this.Id, this.Date, this.Date, this.Date})
		};

		public override bool HasValidArguments => !string.IsNullOrWhiteSpace(this.Id) && this.Date != default(DateTime);
		public string Id { get; set; }
		public DateTime Date { get; set; }
	}
}
