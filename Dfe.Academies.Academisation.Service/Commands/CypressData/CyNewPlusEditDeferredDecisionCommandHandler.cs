using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy new plus edit deferred decision handler.
	/// </summary>
	public class CyNewPlusEditDeferredDecisionCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyNewPlusEditDeferredDecisionCommand, CommandResult>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CyNewPlusEditDeferredDecisionCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyNewPlusEditDeferredDecisionCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public Task<CommandResult> Handle(CyNewPlusEditDeferredDecisionCommand request,
			CancellationToken cancellationToken)
		{
			return Handle(request as CypressDataCommandAbstractBase, cancellationToken);
		}
	}
}
