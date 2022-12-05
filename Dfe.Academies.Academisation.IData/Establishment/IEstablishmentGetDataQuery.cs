namespace Dfe.Academies.Academisation.IData.Establishment
{
	public interface IEstablishmentGetDataQuery
	{
		Task<Establishment?> GetEstablishment(int urn);
	}
}
