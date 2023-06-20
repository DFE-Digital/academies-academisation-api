using Ardalis.GuardClauses;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cypress data base command handler abstract base.
	/// </summary>
	public abstract class CypressDataBaseCommandHandlerAbstractBase
	{
		private readonly AcademisationContext _dbContext;

		/// <summary>
		///     Initializes a new instance of the <see cref="CypressDataBaseCommandHandlerAbstractBase" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		protected CypressDataBaseCommandHandlerAbstractBase(AcademisationContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		///     Handles the command by executing each sql statement within a transaction, if the command has valid arguments
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public async Task<CommandResult> Handle(CypressDataCommandAbstractBase request,
			CancellationToken cancellationToken)
		{
			_ = Guard.Against.Null(request);

			if (!request.HasValidArguments)
			{
				return new BadRequestCommandResult();
			}

			await using IDbContextTransaction txn = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
			foreach ((string statement, object[] parameters) in request.SqlStatements)
			{
				await _dbContext.Database.ExecuteSqlRawAsync(statement, parameters, cancellationToken);
			}

			return new CommandSuccessResult();
		}
	}
}
