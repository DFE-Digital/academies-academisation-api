using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy new plus edit approved decision command handler.
	/// </summary>
	public class CyNewPlusEditApprovedDecisionCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyNewPlusEditApprovedDecisionCommand, CommandResult>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CyNewPlusEditApprovedDecisionCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyNewPlusEditApprovedDecisionCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public Task<CommandResult> Handle(CyNewPlusEditApprovedDecisionCommand request,
			CancellationToken cancellationToken)
		{
			return Handle(request as CypressDataCommandAbstractBase, cancellationToken);
		}
	}
}
