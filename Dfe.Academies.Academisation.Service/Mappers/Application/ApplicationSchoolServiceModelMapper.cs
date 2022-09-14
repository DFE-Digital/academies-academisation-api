using System.Linq;
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
			school.Details.Performance.ToServiceModel(),
			school.Details.LocalAuthority.ToServiceModel(),
			school.Details.PartnershipsAndAffliations.ToServiceModel(),
			school.Details.ReligiousEducation.ToServiceModel(),
			school.Details.PreviousFinancialYear.ToServiceModel(),
			school.Details.CurrentFinancialYear.ToServiceModel(),
			school.Details.NextFinancialYear.ToServiceModel(),
			Loans: school.Loans.Select(c=> c.ToServiceModel()).ToList()
		)
		{
			SchoolContributionToTrust = school.Details.SchoolContributionToTrust,
			GoverningBodyConsentEvidenceDocumentLink = school.Details.GoverningBodyConsentEvidenceDocumentLink,
			AdditionalInformationAdded = school.Details.AdditionalInformationAdded,
			AdditionalInformation = school.Details.AdditionalInformation,
			EqualitiesImpactAssessmentCompleted = school.Details.EqualitiesImpactAssessmentCompleted,
			EqualitiesImpactAssessmentDetails = school.Details.EqualitiesImpactAssessmentDetails,
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
		};
	}

	internal static SchoolDetails ToDomain(this ApplicationSchoolServiceModel serviceModel)
	{
		return new(serviceModel.Urn, 
			serviceModel.SchoolName, 
			serviceModel.LandAndBuildings.ToDomain(),
			serviceModel.Performance.ToDomain(),
			serviceModel.LocalAuthority.ToDomain(),
			serviceModel.PartnershipsAndAffliations.ToDomain(),
			serviceModel.ReligiousEducation.ToDomain(),
			serviceModel.PreviousFinancialYear.ToDomain(),
			serviceModel.CurrentFinancialYear.ToDomain(),
			serviceModel.NextFinancialYear.ToDomain()
			// TODO MR:- what to do with SchoolDetails obj & loans
			//Loans: serviceModel.Loans.ToDictionary(
			//		c => c.LoanId,
			//	c => c.ToDomain())
			)
		{
			SchoolContributionToTrust = serviceModel.SchoolContributionToTrust,
			GoverningBodyConsentEvidenceDocumentLink = serviceModel.GoverningBodyConsentEvidenceDocumentLink,
			AdditionalInformationAdded = serviceModel.AdditionalInformationAdded,
			AdditionalInformation = serviceModel.AdditionalInformation,
			EqualitiesImpactAssessmentCompleted = serviceModel.EqualitiesImpactAssessmentCompleted,
			EqualitiesImpactAssessmentDetails = serviceModel.EqualitiesImpactAssessmentDetails,
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
		};
	}
}
