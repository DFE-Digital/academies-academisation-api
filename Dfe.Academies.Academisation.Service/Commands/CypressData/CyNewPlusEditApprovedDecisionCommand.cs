using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy new plus edit approved decision command.
	/// </summary>
	public class CyNewPlusEditApprovedDecisionCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		///     Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new[]
		{
			("delete from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = {0}",
				new object[] { Id! }),
			(
				@"insert into academisation.ConversionAdvisoryBoardDecision values ({0}, 'Approved', null, null, {1}, 'None', {2}, {3})",
				new object[] { Id!, Date, Date, Date })
		};

		/// <summary>
		///     Gets a value indicating whether has valid arguments.
		/// </summary>
		public sealed override bool HasValidArguments => !string.IsNullOrWhiteSpace(Id) && Date != default;

		/// <summary>
		///     Gets or sets the id.
		/// </summary>
		public string? Id { get; set; }

		/// <summary>
		///     Gets or sets the date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CyNewPlusEditApprovedDecisionCommand"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="date">The date.</param>
		public CyNewPlusEditApprovedDecisionCommand(string id, DateTime date)
		{
			Id = id;
			Date = date;
			if (!HasValidArguments) throw new ArgumentException();
		}
	}
}
