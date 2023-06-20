using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy advisory board urls command handler.
	/// </summary>
	public class CyAdvisoryBoardUrlsCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyAdvisoryBoardUrlsCommand, CommandResult>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CyAdvisoryBoardUrlsCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAdvisoryBoardUrlsCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public Task<CommandResult> Handle(CyAdvisoryBoardUrlsCommand request, CancellationToken cancellationToken)
		{
			return Handle(request as CypressDataCommandAbstractBase, cancellationToken);
		}
	}
}
