using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy create deferred decision command.
	/// </summary>
	public class CyCreateDeferredDecisionCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		///     Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new[]
		{
			("DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = {0}",
				new object[] { Id! })
		};

		/// <summary>
		///     Gets a value indicating whether has valid arguments.
		/// </summary>
		public sealed override bool HasValidArguments => !string.IsNullOrEmpty(Id);

		/// <summary>
		///     Gets or sets the id.
		/// </summary>
		public string? Id { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CyCreateDeferredDecisionCommand"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public CyCreateDeferredDecisionCommand(string id)
		{
			Id = id;
			if (!HasValidArguments) throw new ArgumentException();
		}
	}
}
