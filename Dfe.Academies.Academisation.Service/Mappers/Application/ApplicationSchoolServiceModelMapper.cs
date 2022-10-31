using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

internal static class ApplicationSchoolServiceModelMapper
{
	internal static ApplicationSchoolServiceModel FromDomain(ISchool school)
	{
		return new(
			school.Id,
			school.Details.Urn,
			school.Details.SchoolName,
			school.Details.LandAndBuildings.ToServiceModel(),
			school.Details.TrustBenefitDetails,
			school.Details.OfstedInspectionDetails,
			school.Details.SafeguardingDetails,
			school.Details.LocalAuthorityReorganisationDetails,
			school.Details.LocalAuthorityClosurePlanDetails,
			school.Details.DioceseName,
			school.Details.DioceseFolderIdentifier,
			school.Details.PartOfFederation,
			school.Details.FoundationTrustOrBodyName,
			school.Details.FoundationConsentFolderIdentifier,
			school.Details.ExemptionEndDate,
			school.Details.MainFeederSchools,
			school.Details.ResolutionConsentFolderIdentifier,
			school.Details.ProtectedCharacteristics,
			school.Details.FurtherInformation,
			school.Details.PreviousFinancialYear.ToServiceModel(),
			school.Details.CurrentFinancialYear.ToServiceModel(),
			school.Details.NextFinancialYear.ToServiceModel(),
			Loans: school.Loans.Select(c=> c.ToServiceModel()).ToList(),
			Leases: school.Leases.Select(c => c.ToServiceModel()).ToList()
		)
		{
			SchoolConversionApproverContactEmail = school.Details.ApproverContactEmail,
			SchoolConversionApproverContactName = school.Details.ApproverContactName,
			SchoolConversionMainContactOtherEmail = school.Details.MainContactOtherEmail,
			SchoolConversionMainContactOtherName = school.Details.MainContactOtherName,
			SchoolConversionMainContactOtherTelephone = school.Details.MainContactOtherTelephone,
			SchoolConversionMainContactOtherRole = school.Details.MainContactOtherRole,
			SchoolConversionContactHeadTel = school.Details.ContactHeadTel,
			SchoolConversionContactHeadName = school.Details.ContactHeadName,
			SchoolConversionContactHeadEmail = school.Details.ContactHeadEmail,
			SchoolConversionContactChairEmail = school.Details.ContactChairEmail,
			SchoolConversionContactChairName = school.Details.ContactChairName,
			SchoolConversionContactChairTel = school.Details.ContactChairTel,
			SchoolConversionContactRole = school.Details.ContactRole,
			ApplicationJoinTrustReason = school.Details.ApplicationJoinTrustReason,
			SchoolConversionTargetDateSpecified = school.Details.ConversionTargetDateSpecified,
			SchoolConversionTargetDate = school.Details.ConversionTargetDate,
			SchoolConversionTargetDateExplained = school.Details.ConversionTargetDateExplained,
			ConversionChangeNamePlanned = school.Details.ConversionChangeNamePlanned,
			ProposedNewSchoolName = school.Details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = school.Details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = school.Details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = school.Details.ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = school.Details.CapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = school.Details.CapacityPublishedAdmissionsNumber,
			SchoolSupportGrantFundsPaidTo = school.Details.SchoolSupportGrantFundsPaidTo,
			ConfirmPaySupportGrantToSchool = school.Details.ConfirmPaySupportGrantToSchool,
			SchoolHasConsultedStakeholders = school.Details.SchoolHasConsultedStakeholders,
			SchoolPlanToConsultStakeholders = school.Details.SchoolPlanToConsultStakeholders,
			FinanceOngoingInvestigations = school.Details.FinanceOngoingInvestigations,
			FinancialInvestigationsExplain = school.Details.FinancialInvestigationsExplain,
			FinancialInvestigationsTrustAware = school.Details.FinancialInvestigationsTrustAware,
			DeclarationBodyAgree = school.Details.DeclarationBodyAgree,
			DeclarationIAmTheChairOrHeadteacher = school.Details.DeclarationIAmTheChairOrHeadteacher,
			DeclarationSignedByName = school.Details.DeclarationSignedByName,
			SchoolConversionReasonsForJoining = school.Details.SchoolConversionReasonsForJoining
		};
	}

	internal static SchoolDetails ToDomain(this ApplicationSchoolServiceModel serviceModel)
	{
		return new(serviceModel.Urn, 
			serviceModel.SchoolName, 
			serviceModel.LandAndBuildings?.ToDomain(),
			serviceModel.PreviousFinancialYear?.ToDomain(),
			serviceModel.CurrentFinancialYear?.ToDomain(),
			serviceModel.NextFinancialYear?.ToDomain(),
			serviceModel.TrustBenefitDetails,
			serviceModel.OfstedInspectionDetails,
			serviceModel.SafeguardingDetails,
			serviceModel.LocalAuthorityReorganisationDetails,
			serviceModel.LocalAuthorityClosurePlanDetails,
			serviceModel.DioceseName,
			serviceModel.DioceseFolderIdentifier,
			serviceModel.PartOfFederation,
			serviceModel.FoundationTrustOrBodyName,
			serviceModel.FoundationConsentFolderIdentifier,
			serviceModel.ExemptionEndDate,
			serviceModel.MainFeederSchools,
			serviceModel.ResolutionConsentFolderIdentifier,
			serviceModel.ProtectedCharacteristics,
			serviceModel.FurtherInformation
			
			)
		{
			ApproverContactEmail = serviceModel.SchoolConversionApproverContactEmail,
			ApproverContactName = serviceModel.SchoolConversionApproverContactName,
			MainContactOtherTelephone = serviceModel.SchoolConversionMainContactOtherTelephone,
			MainContactOtherRole = serviceModel.SchoolConversionMainContactOtherRole,
			MainContactOtherName = serviceModel.SchoolConversionMainContactOtherName,
			MainContactOtherEmail = serviceModel.SchoolConversionMainContactOtherEmail,
			ContactHeadTel = serviceModel.SchoolConversionContactHeadTel,
			ContactHeadName = serviceModel.SchoolConversionContactHeadName,
			ContactHeadEmail = serviceModel.SchoolConversionContactHeadEmail,
			ContactChairEmail = serviceModel.SchoolConversionContactChairEmail,
			ContactChairName = serviceModel.SchoolConversionContactChairName,
			ContactChairTel = serviceModel.SchoolConversionContactChairTel,
			ContactRole = serviceModel.SchoolConversionContactRole,
			ApplicationJoinTrustReason = serviceModel.ApplicationJoinTrustReason,
			ConversionTargetDateSpecified = serviceModel.SchoolConversionTargetDateSpecified,
			ConversionTargetDate = serviceModel.SchoolConversionTargetDate,
			ConversionTargetDateExplained = serviceModel.SchoolConversionTargetDateExplained,
			ConversionChangeNamePlanned = serviceModel.ConversionChangeNamePlanned,
			ProposedNewSchoolName = serviceModel.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = serviceModel.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = serviceModel.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = serviceModel.ProjectedPupilNumbersYear3,
			CapacityAssumptions = serviceModel.SchoolCapacityAssumptions,
			CapacityPublishedAdmissionsNumber = serviceModel.SchoolCapacityPublishedAdmissionsNumber,
			SchoolSupportGrantFundsPaidTo = serviceModel.SchoolSupportGrantFundsPaidTo,
			ConfirmPaySupportGrantToSchool = serviceModel.ConfirmPaySupportGrantToSchool,
			SchoolHasConsultedStakeholders = serviceModel.SchoolHasConsultedStakeholders,
			SchoolPlanToConsultStakeholders = serviceModel.SchoolPlanToConsultStakeholders,
			FinanceOngoingInvestigations = serviceModel.FinanceOngoingInvestigations,
			FinancialInvestigationsExplain = serviceModel.FinancialInvestigationsExplain,
			FinancialInvestigationsTrustAware = serviceModel.FinancialInvestigationsTrustAware,
			DeclarationBodyAgree = serviceModel.DeclarationBodyAgree,
			DeclarationIAmTheChairOrHeadteacher = serviceModel.DeclarationIAmTheChairOrHeadteacher,
			DeclarationSignedByName = serviceModel.DeclarationSignedByName,
			SchoolConversionReasonsForJoining = serviceModel.SchoolConversionReasonsForJoining
		};
	}
}
