using Dfe.Academies.Academisation.Data;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cypress data base command handler abstract base.
	/// </summary>
	public abstract class CypressDataBaseCommandHandlerAbstractBase
	{
		protected AcademisationContext DbContext { get; }

		/// <summary>
		///     Initializes a new instance of the <see cref="CypressDataBaseCommandHandlerAbstractBase" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		protected CypressDataBaseCommandHandlerAbstractBase(AcademisationContext dbContext)
		{
			DbContext = dbContext;
		}

	}
}
