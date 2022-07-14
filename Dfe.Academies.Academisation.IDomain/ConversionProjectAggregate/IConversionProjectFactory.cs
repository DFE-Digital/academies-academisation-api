namespace Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

public interface IConversionProjectFactory
{
	Task<IConversionProject> Create(int projectId);
}