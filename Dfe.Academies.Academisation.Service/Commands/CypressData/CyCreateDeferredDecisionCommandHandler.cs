using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy create deferred decision command handler.
	/// </summary>
	public class CyCreateDeferredDecisionCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyCreateDeferredDecisionCommand, CommandResult>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CyCreateDeferredDecisionCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyCreateDeferredDecisionCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public Task<CommandResult> Handle(CyCreateDeferredDecisionCommand request, CancellationToken cancellationToken)
		{
			return Handle(request as CypressDataCommandAbstractBase, cancellationToken);
		}
	}
}
