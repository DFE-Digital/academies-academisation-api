using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy new plus edit declined decision command handler.
	/// </summary>
	public class CyNewPlusEditDeclinedDecisionCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyNewPlusEditDeclinedDecisionCommand, CommandResult>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CyNewPlusEditDeclinedDecisionCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyNewPlusEditDeclinedDecisionCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public Task<CommandResult> Handle(CyNewPlusEditDeclinedDecisionCommand request,
			CancellationToken cancellationToken)
		{
			return Handle(request as CypressDataCommandAbstractBase, cancellationToken);
		}
	}
}
