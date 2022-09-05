using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

internal static class LandAndBuildingsServiceModelMapper
{
	internal static LandAndBuildingsServiceModel ToServiceModel(this LandAndBuildings landAndBuildings)
	{
		return new LandAndBuildingsServiceModel
		{
			OwnerExplained = landAndBuildings.OwnerExplained,
			WorksPlanned = landAndBuildings.WorksPlanned,
			WorksPlannedDate = landAndBuildings.WorksPlannedDate,
			WorksPlannedExplained = landAndBuildings.WorksPlannedExplained,
			FacilitiesShared = landAndBuildings.FacilitiesShared,
			FacilitiesSharedExplained = landAndBuildings.FacilitiesSharedExplained,
			Grants = landAndBuildings.Grants,
			GrantsAwardingBodies = landAndBuildings.GrantsAwardingBodies,
			PartOfPfiScheme = landAndBuildings.PartOfPfiScheme,
			PartOfPfiSchemeType = landAndBuildings.PartOfPfiSchemeType,
			PartOfPrioritySchoolsBuildingProgramme = landAndBuildings.PartOfPrioritySchoolsBuildingProgramme,
			PartOfBuildingSchoolsForFutureProgramme = landAndBuildings.PartOfBuildingSchoolsForFutureProgramme
		};
	}

	internal static LandAndBuildings ToDomain(this LandAndBuildingsServiceModel serviceModel)
	{
		return new LandAndBuildings
		{
			OwnerExplained = serviceModel.OwnerExplained,
			WorksPlanned = serviceModel.WorksPlanned,
			WorksPlannedDate = serviceModel.WorksPlannedDate,
			WorksPlannedExplained = serviceModel.WorksPlannedExplained,
			FacilitiesShared = serviceModel.FacilitiesShared,
			FacilitiesSharedExplained = serviceModel.FacilitiesSharedExplained,
			Grants = serviceModel.Grants,
			GrantsAwardingBodies = serviceModel.GrantsAwardingBodies,
			PartOfPfiScheme = serviceModel.PartOfPfiScheme,
			PartOfPfiSchemeType = serviceModel.PartOfPfiSchemeType,
			PartOfPrioritySchoolsBuildingProgramme = serviceModel.PartOfPrioritySchoolsBuildingProgramme,
			PartOfBuildingSchoolsForFutureProgramme = serviceModel.PartOfBuildingSchoolsForFutureProgramme
		};
	}
}
