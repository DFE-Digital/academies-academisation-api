using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate
{
	public class ApplicationGetDataQuery : IApplicationGetDataQuery
	{
		public async Task<IConversionApplication> Execute(int id)
		{
			await Task.CompletedTask;
			return new ConversionApplication(1, ApplicationType.JoinAMat, new Dictionary<int, ContributorDetails>());
		}
	}
}
