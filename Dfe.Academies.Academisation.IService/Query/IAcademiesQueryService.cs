using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IAcademiesQueryService
	{
        Task<EstablishmentDto?> GetEstablishmentByUkprn(string ukprn);
        Task<EstablishmentDto?> GetEstablishment(int urn);
		Task<TrustDto?> GetTrust(string ukprn);
		Task<TrustDto?> GetTrustByReferenceNumber(string trustReferenceNumber);
		Task<IEnumerable<EstablishmentDto>> GetBulkEstablishmentsByUkprn(IEnumerable<string> ukprns);
		Task<IEnumerable<EstablishmentDto>> PostBulkEstablishmentsByUkprns(IEnumerable<string> ukprns);
	}
}
