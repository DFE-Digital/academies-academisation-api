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
			school.TrustBenefitDetails,
			school.OfstedInspectionDetails,
			school.Safeguarding,
			school.LocalAuthorityReorganisationDetails,
			school.LocalAuthorityClosurePlanDetails,
			school.DioceseName,
			school.DioceseFolderIdentifier,
			school.PartOfFederation,
			school.FoundationTrustOrBodyName,
			school.FoundationConsentFolderIdentifier,
			school.ExemptionEndDate,
			school.MainFeederSchools,
			school.ResolutionConsentFolderIdentifier,
			school.ProtectedCharacteristics,
			school.FurtherInformation,
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
			SchoolConversionMainContactOtherRole = school.Details.MainContactOtherRole,
			SchoolConversionContactHeadName = school.Details.ContactHeadName,
			SchoolConversionContactHeadEmail = school.Details.ContactHeadEmail,
			SchoolConversionContactChairEmail = school.Details.ContactChairEmail,
			SchoolConversionContactChairName = school.Details.ContactChairName,
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
			SchoolSupportGrantJoiningInAGroup = school.Details.SchoolSupportGrantJoiningInAGroup,
			SchoolSupportGrantBankDetailsProvided = school.Details.SchoolSupportGrantBankDetailsProvided,
			ConfirmPaySupportGrantToSchool = school.Details.ConfirmPaySupportGrantToSchool,
			SchoolsInGroup = school.Details.SchoolsInGroup,
			SchoolHasConsultedStakeholders = school.Details.SchoolHasConsultedStakeholders,
			SchoolPlanToConsultStakeholders = school.Details.SchoolPlanToConsultStakeholders,
			FinanceOngoingInvestigations = school.Details.FinanceOngoingInvestigations,
			FinancialInvestigationsExplain = school.Details.FinancialInvestigationsExplain,
			FinancialInvestigationsTrustAware = school.Details.FinancialInvestigationsTrustAware,
			DeclarationBodyAgree = school.Details.DeclarationBodyAgree,
			DeclarationIAmTheChairOrHeadteacher = school.Details.DeclarationIAmTheChairOrHeadteacher,
			DeclarationSignedByName = school.Details.DeclarationSignedByName,
			SchoolConversionReasonsForJoining = school.Details.SchoolConversionReasonsForJoining,
			HasLoans = school.HasLoans,
			HasLeases = school.HasLeases,
			EntityId = school.EntityId
		};
	}

	internal static SchoolDetails ToDomain(this ApplicationSchoolServiceModel serviceModel)
	{
		return new(serviceModel.Urn, 
			serviceModel.SchoolName, 
			serviceModel.LandAndBuildings?.ToDomain(),
			serviceModel.PreviousFinancialYear?.ToDomain(),
			serviceModel.CurrentFinancialYear?.ToDomain(),
			serviceModel.NextFinancialYear?.ToDomain()
			
			)
		{
			ApproverContactEmail = serviceModel.SchoolConversionApproverContactEmail,
			ApproverContactName = serviceModel.SchoolConversionApproverContactName,
			MainContactOtherRole = serviceModel.SchoolConversionMainContactOtherRole,
			MainContactOtherName = serviceModel.SchoolConversionMainContactOtherName,
			MainContactOtherEmail = serviceModel.SchoolConversionMainContactOtherEmail,
			ContactHeadName = serviceModel.SchoolConversionContactHeadName,
			ContactHeadEmail = serviceModel.SchoolConversionContactHeadEmail,
			ContactChairEmail = serviceModel.SchoolConversionContactChairEmail,
			ContactChairName = serviceModel.SchoolConversionContactChairName,
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
			SchoolSupportGrantJoiningInAGroup = serviceModel.SchoolSupportGrantJoiningInAGroup,
			SchoolSupportGrantBankDetailsProvided = serviceModel.SchoolSupportGrantBankDetailsProvided,
			ConfirmPaySupportGrantToSchool = serviceModel.ConfirmPaySupportGrantToSchool,
			SchoolsInGroup = serviceModel.SchoolsInGroup,
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
