using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	public class CyNewPlusEditApprovedDecisionCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		public override IEnumerable<(string, object[])> SqlStatements => new (string, object[])[]
		{
		("delete from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = {0}", new object[] { this.Id }),

		(@"insert into academisation.ConversionAdvisoryBoardDecision values ({0}, 'Approved', null, null, {1}, 'None', {2}, {3})",
			new object[] { this.Id, this.Date, this.Date, this.Date })

		};

		public override bool HasValidArguments => !string.IsNullOrWhiteSpace(this.Id) && this.Date != default(DateTime);
		public string Id { get; set; }
		public DateTime Date { get; set; }
	}
}
