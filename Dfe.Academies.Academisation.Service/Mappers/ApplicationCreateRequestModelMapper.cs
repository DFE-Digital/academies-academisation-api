using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.Service.Mappers
{
	internal static class ApplicationCreateRequestModelMapper
	{
		internal static (ApplicationType, ContributorDetails) AsDomain(this ApplicationCreateRequestModel requestModel)
		{
			ContributorDetails contributorDetails = new(
				requestModel.Contributor.FirstName,
				requestModel.Contributor.LastName,
				requestModel.Contributor.EmailAddress,
				requestModel.Contributor.Role,
				requestModel.Contributor.OtherRoleName);

			return (requestModel.ApplicationType, contributorDetails);
		}
	}
}
