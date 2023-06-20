using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy add form a mat project command handler.
	/// </summary>
	public class CyAddFormAMatProjectCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyAddFormAMatProjectCommand, CommandResult>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CyAddFormAMatProjectCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAddFormAMatProjectCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public Task<CommandResult> Handle(CyAddFormAMatProjectCommand request, CancellationToken cancellationToken)
		{
			return Handle(request as CypressDataCommandAbstractBase, cancellationToken);
		}
	}
}
