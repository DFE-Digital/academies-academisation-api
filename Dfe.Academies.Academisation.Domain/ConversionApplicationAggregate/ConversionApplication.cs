using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate
{
	internal class ConversionApplication : IConversionApplication
	{
		private readonly List<IContributor> _contributors = new();

		public ConversionApplication(ApplicationType applicationType, IContributorDetails initialContributor)
		{
			ApplicationType = applicationType;
			_contributors.Add(new Contributor(initialContributor));
		}

		public ApplicationType ApplicationType { get; init; }

		public IReadOnlyCollection<IContributor> Contributors => _contributors.AsReadOnly();
	}
}
