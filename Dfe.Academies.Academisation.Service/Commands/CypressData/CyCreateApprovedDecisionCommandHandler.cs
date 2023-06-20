using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy create approved decision command command handler.
	/// </summary>
	public class CyCreateApprovedDecisionCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyCreateApprovedDecisionCommand, CommandResult>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CyCreateApprovedDecisionCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyCreateApprovedDecisionCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public Task<CommandResult> Handle(CyCreateApprovedDecisionCommand request, CancellationToken cancellationToken)
		{
			return Handle(request as CypressDataCommandAbstractBase, cancellationToken);
		}
	}
}
