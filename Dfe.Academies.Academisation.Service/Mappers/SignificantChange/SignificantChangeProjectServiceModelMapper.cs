using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

namespace Dfe.Academies.Academisation.Service.Mappers.SignificantChange;

internal static class SignificantChangeProjectServiceModelMapper
{
	internal static SignificantChangeProjectSearchResponse MapToServiceModel(this SignificantChangeProject project)
	{
		var assignedUser = project.AssignedUserId != null
			? new User(
				project.AssignedUserId.Value,
				project.AssignedUserFullName ?? string.Empty,
				project.AssignedUserEmailAddress ?? string.Empty)
			: null;

		return new SignificantChangeProjectSearchResponse
		{
			Id = project.Id,
			Urn = project.Urn,
			SchoolName = project.SchoolName,
			Tier = project.Tier,
			TrustName = project.TrustName,
			TrustUkprn = project.TrustUkprn,
			AssignedUser = assignedUser,
			TypeOfSignificantChange = project.TypeOfSignificantChange,
			Status = project.Status.ToString()
		};
	}
}