using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public interface IAdvisoryBoardDecisionRepository : IRepository<ConversionAdvisoryBoardDecision>, IGenericRepository<ConversionAdvisoryBoardDecision>
	{
		Task<IConversionAdvisoryBoardDecision?> GetAdvisoryBoardDecisionById(int decisionId);
		public Task<IEnumerable<IConversionAdvisoryBoardDecision?>> GetAllAdvisoryBoardDecisions();
		public Task<IEnumerable<IConversionAdvisoryBoardDecision?>> GetAllAdvisoryBoardDecisionsForTransfers();
		Task<ConversionAdvisoryBoardDecision?> GetConversionProjectDecsion(int projectId);
		Task<ConversionAdvisoryBoardDecision?> GetTransferProjectDecsion(int projectId);
	}
}
