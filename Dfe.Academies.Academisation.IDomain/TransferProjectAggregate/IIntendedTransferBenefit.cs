namespace Dfe.Academies.Academisation.IDomain.TransferProjectAggregate
{
	public interface IIntendedTransferBenefit
	{
		int Id { get; }
		string SelectedBenefit { get; }
		int TransferProjectId { get; }
	}
}