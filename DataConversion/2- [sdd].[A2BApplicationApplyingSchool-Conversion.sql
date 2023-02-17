Use [<database_name, sysname, sip>]
/** 
enums values from c#->
public enum SelectOption
{
	Yes = 1,
	No = 0
}

as-is data->
Headteacher
Other

public enum MainConversionContact = DONE
{
	HeadTeacher = 1,
	ChairOfGoverningBody = 2,
	Other = 3
}

public enum PayFundsTo = DONE
{
	[Description("To the school")]
	School = 1,
	[Description("To the trust the school is joining")]
	Trust = 2
}

public enum RevenueType = DONE
{
	Surplus = 1,
	Deficit = 2
}

public enum SchoolEqualitiesProtectedCharacteristics = DONE
{
	[Description("That the Secretary of State's decision is unlikely to disproportionately affect any particular person or group who share protected characteristics")]
	Unlikely = 1,
	[Description("That there are some impacts but on balance the changes will not disproportionately affect any particular person or group who share protected characteristics")]
	WillNot = 0
}

**/

BEGIN TRY
BEGIN TRANSACTION PortDynamicsSchoolData

	/*** populate [academisation].[ApplicationSchool] ***/
	-- MR:- below are nullable - backfill afterwards - as part of leases && loans conversion
	--,[HasLeases]
	--,[HasLoans]

	-- MR: grabbing DB generated [ConversionApplicationId] by joining onto [academisation].[ConversionApplication]
	INSERT INTO [academisation].[ApplicationSchool]
			   ([Urn]
			   ,[ConversionApplicationId]
			   ,[CreatedOn]
			   ,[LastModifiedOn]
			   ,[ProjectedPupilNumbersYear1]
			   ,[ProjectedPupilNumbersYear2]
			   ,[ProjectedPupilNumbersYear3]
			   ,[ProposedNewSchoolName]
			   ,[MainContactOtherTelephone]
			   ,[CapacityPublishedAdmissionsNumber]
			   ,[ApproverContactEmail]
			   ,[ApproverContactName]
			   ,[CapacityAssumptions]
			   ,[ContactChairEmail]
			   ,[ContactChairName]
			   ,[ContactChairTel]
			   ,[ContactHeadEmail]
			   ,[ContactHeadName]
			   ,[ContactHeadTel]
			   ,[ContactRole]
			   ,[ConversionTargetDateExplained]
			   ,[JoinTrustReason]
			   ,[MainContactOtherEmail]
			   ,[MainContactOtherName]
			   ,[MainContactOtherRole]
			   ,[SchoolName]
			   ,[FacilitiesShared]
			   ,[FacilitiesSharedExplained]
			   ,[Grants]
			   ,[GrantsAwardingBodies]
			   ,[OwnerExplained]
			   ,[PartOfBuildingSchoolsForFutureProgramme]
			   ,[PartOfPfiScheme]
			   ,[PartOfPfiSchemeType]
			   ,[PartOfPrioritySchoolsBuildingProgramme]
			   ,[WorksPlanned]
			   ,[WorksPlannedDate]
			   ,[WorksPlannedExplained]
			   ,[ConversionTargetDate]
			   ,[ConversionTargetDateSpecified]
			   ,[ConversionChangeNamePlanned]
			   ,[ConfirmPaySupportGrantToSchool]
			   ,[SupportGrantFundsPaidTo]
			   ,[DioceseName]
			   ,[FoundationTrustOrBodyName]
			   ,[FurtherInformation]
			   ,[LocalAuthorityClosurePlanDetails]
			   ,[LocalAuthorityReorganisationDetails]
			   ,[OfstedInspectionDetails]
			   ,[SafeguardingDetails]
			   ,[TrustBenefitDetails]
			   ,[CurrentFinancialYearCapitalCarryForward]
			   ,[CurrentFinancialYearCapitalCarryForwardExplained]
			   ,[CurrentFinancialYearCapitalCarryForwardStatus]
			   ,[CurrentFinancialYearEndDate]
			   ,[CurrentFinancialYearRevenue]
			   ,[CurrentFinancialYearRevenueStatus]
			   ,[CurrentFinancialYearRevenueStatusExplained]
			   ,[NextFinancialYearCapitalCarryForward]
			   ,[NextFinancialYearCapitalCarryForwardExplained]
			   ,[NextFinancialYearCapitalCarryForwardStatus]
			   ,[NextFinancialYearEndDate]
			   ,[NextFinancialYearRevenue]
			   ,[NextFinancialYearRevenueStatus]
			   ,[NextFinancialYearRevenueStatusExplained]
			   ,[PreviousFinancialYearCapitalCarryForward]
			   ,[PreviousFinancialYearCapitalCarryForwardExplained]
			   ,[PreviousFinancialYearCapitalCarryForwardStatus]
			   ,[PreviousFinancialYearEndDate]
			   ,[PreviousFinancialYearRevenue]
			   ,[PreviousFinancialYearRevenueStatus]
			   ,[PreviousFinancialYearRevenueStatusExplained]
			   ,[SchoolHasConsultedStakeholders]
			   ,[SchoolPlanToConsultStakeholders]
			   ,[FinanceOngoingInvestigations]
			   ,[FinancialInvestigationsExplain]
			   ,[FinancialInvestigationsTrustAware]
			   ,[DeclarationBodyAgree]
			   ,[DeclarationIAmTheChairOrHeadteacher]
			   ,[DeclarationSignedByName]
			   ,[SchoolConversionReasonsForJoining]
			   ,[ExemptionEndDate]
			   ,[MainFeederSchools]
			   ,[PartOfFederation]
			   ,[ProtectedCharacteristics]
			   ,[DynamicsApplyingSchoolId]
			   --MR:- below not in v1.5
			--,[DiocesePermissionEvidenceDocumentLink]
			--,[FoundationEvidenceDocumentLink]
			--,[GoverningBodyConsentEvidenceDocumentLink]			   
			)
	SELECT 
			ASS.[Urn],
			APP.Id as [ConversionApplicationId],
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			ASS.[ProjectedPupilNumbersYear1],
			ASS.[ProjectedPupilNumbersYear2],
			ASS.[ProjectedPupilNumbersYear3],
			ASS.[SchoolConversionChangeNameValue] as 'ProposedNewSchoolName',
			ASS.[SchoolConversionMainContactOtherTelephone],
			ASS.[SchoolCapacityPublishedAdmissionsNumber],
			ASS.[SchoolConversionApproverContactEmail],
			ASS.[SchoolConversionApproverContactName],
			ASS.[SchoolCapacityAssumptions],
			ASS.[SchoolConversionContactChairEmail],
			ASS.[SchoolConversionContactChairName],
			ASS.[SchoolConversionContactChairTel],
			ASS.[SchoolConversionContactHeadEmail],
			ASS.[SchoolConversionContactHeadName],
			ASS.[SchoolConversionContactHeadTel],
			--ASS.[SchoolConversionContactRole], -- MR:- need mapper - string -> int !
			CASE ASS.[SchoolConversionContactRole]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 2
				WHEN 907660002 THEN 3
			END as 'ContactRole',
			ASS.[SchoolConversionTargetDateExplained],
			ASS.[SchoolConversionReasonsForJoining] as 'JoinTrustReason',
			ASS.[SchoolConversionMainContactOtherEmail],
			ASS.[SchoolConversionMainContactOtherName],
			ASS.[SchoolConversionMainContactOtherRole],
			ASS.[Name] as 'SchoolName',
			--**** land & buildings ****
			CASE ASS.[SchoolBuildLandSharedFacilities]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolBuildLandSharedFacilities',
			ASS.[SchoolBuildLandSharedFacilitiesExplained],
			CASE ASS.[SchoolBuildLandGrants]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolBuildLandGrants',
			ASS.[SchoolBuildLandGrantsBody],
			ASS.[SchoolBuildLandOwnerExplained],			
			CASE ASS.[SchoolBuildLandFutureProgramme]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolBuildLandFutureProgramme',
			CASE ASS.[SchoolBuildLandPFIScheme]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolBuildLandPFIScheme',
			ASS.[SchoolBuildLandPFISchemeType],
			CASE ASS.[SchoolBuildLandPriorityBuildingProgramme]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolBuildLandPriorityBuildingProgramme',
			CASE ASS.[SchoolBuildLandWorksPlanned]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolBuildLandWorksPlanned',
			ASS.[SchoolBuildLandWorksPlannedDate],
			ASS.[SchoolBuildLandWorksPlannedExplained],
			-- ****
			ASS.[SchoolConversionTargetDateDate],
			CASE ASS.[SchoolConversionTargetDateDifferent]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolConversionTargetDateDifferent',
			CASE ASS.[SchoolConversionChangeName]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolConversionChangeName',
			0 as 'ConfirmPaySupportGrantToSchool', -- BIT TODO:- ????
			--ASS.[SchoolSupportGrantFundsPaidTo] as 'SupportGrantFundsPaidTo', MR:- need mapper - string -> int !
			CASE ASS.SchoolSupportGrantFundsPaidTo
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SupportGrantFundsPaidTo',
			--**** additional info ****
			--ASS.[SchoolFaithSchool] - not in v1.5 schema ??
			ASS.[SchoolFaithSchoolDioceseName] AS 'DioceseName',
			ASS.[SchoolSupportedFoundationBodyName] AS 'FoundationTrustOrBodyName',
			ASS.[SchoolFurtherInformation] as 'FurtherInformation',
			--ASS.[SchoolLaClosurePlans], - BIT - not in v1.5 schema
			ASS.[SchoolLaClosurePlansExplain],
			--ASS.[SchoolLaReorganization], BIT - not in v1.5 schema
			ASS.[SchoolLaReorganizationExplain],
			--ASS.[SchoolAdInspectedButReportNotPublished], BIT - not in v1.5 schema
			ASS.[SchoolAdInspectedReportNotPublishedExplain],
			--ASS.[SchoolAdSafeguarding], BIT - not in v1.5 schema
			ASS.[SchoolAdSafeguardingExplained] as 'SafeguardingDetails',
			ASS.[SchoolAdSchoolContributionToTrust] as 'TrustBenefitDetails',
			-- **** CFY / NFY / PFY details ****
			ASS.[SchoolCFYCapitalForward],
			ASS.[SchoolCFYCapitalForwardStatusExplained],
			--ASS.[SchoolCFYCapitalIsDeficit], -- BIT -> int v1.5 - Surplus = 1,Deficit = 2
			CASE ASS.[SchoolCFYCapitalForwardSurplusOrDeficit]--[SchoolCFYCapitalIsDeficit]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 2
			END as 'CurrentFinancialYearCapitalCarryForwardStatus',
			ASS.[SchoolCFYEndDate],
			ASS.[SchoolCFYRevenue],
			--ASS.[SchoolCFYRevenueIsDeficit], -- BIT -> int v1.5
			CASE ASS.[SchoolCFYRevenueSurplusOrDeficit]--[SchoolCFYRevenueIsDeficit]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 2
			END as 'CurrentFinancialYearRevenueStatus',
			ASS.[SchoolCFYRevenueStatusExplained],
			ASS.[SchoolNFYCapitalForward],
			ASS.[SchoolNFYCapitalForwardStatusExplained],
			--ASS.[SchoolNFYCapitalIsDeficit], BIT -> int v1.5
			CASE ASS.[SchoolNFYCapitalForwardSurplusOrDeficit]--[SchoolNFYCapitalIsDeficit]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 2
			END as 'NextFinancialYearCapitalCarryForwardStatus',
			ASS.[SchoolNFYEndDate],
			ASS.[SchoolNFYRevenue],
			--ASS.[SchoolNFYRevenueIsDeficit],  -- BIT -> int v1.5
			CASE ASS.[SchoolNFYRevenueSurplusOrDeficit]--[SchoolNFYRevenueIsDeficit]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 2
			END as 'NextFinancialYearRevenueStatus',
			ASS.[SchoolNFYRevenueStatusExplained],
			ASS.[SchoolPFYCapitalForward],
			ASS.[SchoolPFYCapitalForwardStatusExplained],
			--ASS.[SchoolPFYCapitalIsDeficit], -- BIT -> int v1.5
			CASE ASS.[SchoolPFYCapitalForwardSurplusOrDeficit]--[SchoolPFYCapitalIsDeficit]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 2
			END as 'PreviousFinancialYearCapitalCarryForwardStatus',
			ASS.[SchoolPFYEndDate],
			ASS.[SchoolPFYRevenue],
			CASE ASS.[SchoolPFYRevenueSurplusOrDeficit]--[SchoolPFYRevenueIsDeficit]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 2
			END as 'PreviousFinancialYearRevenueStatus',
			ASS.[SchoolPFYRevenueStatusExplained],
			-- ****
			CASE ASS.[SchoolConsultationStakeholders]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolConsultationStakeholders',
			ASS.[SchoolConsultationStakeholdersConsult],
			CASE ASS.[SchoolFinancialInvestigations]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolFinancialInvestigations',
			ASS.[SchoolFinancialInvestigationsExplain],
			CASE ASS.[SchoolFinancialInvestigationsTrustAware]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolFinancialInvestigationsTrustAware',
			-- ****
			CASE ASS.[SchoolDeclarationBodyAgree]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolDeclarationBodyAgree',
			CASE ASS.[SchoolDeclarationTeacherChair]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolDeclarationTeacherChair',
			ASS.[SchoolDeclarationSignedByName] as 'DeclarationSignedByName',
			-- ****
			ASS.[SchoolConversionReasonsForJoining],
			-- **** more additional info
			--ASS.[SchoolSACREExemption], BIT - not in v1.5 schema
			--CAST('12/01/2019' as date)
			--CONVERT(DATETIME,'13/12/2019')
			--ASS.[SchoolSACREExemptionEndDate], -- MR:- need data conversion, string in as-is !!!
			CONVERT(datetimeoffset(7),ASS.[SchoolSACREExemptionEndDate]) as '[ExemptionEndDate]',
			-- ****
			ASS.[SchoolAdFeederSchools],
			CASE ASS.[SchoolPartOfFederation]
				WHEN 907660000 THEN 1
				WHEN 907660001 THEN 0
			END as 'SchoolPartOfFederation',
			--ASS.SchoolAdEqualitiesImpactAssessmentDetails as 'ProtectedCharacteristics', -- MR:- need mapper - string -> int !
			CASE ASS.SchoolAdEqualitiesImpactAssessmentDetails
				WHEN 'That the Secretary of State''s decision is unlikely to disproportionately affect any particular person or group who share protected characteristics' THEN 1
				WHEN 'That there are some impacts but on balance the changes will not disproportionately affect any particular person or group who share protected characteristics' THEN 0
			END as 'ProtectedCharacteristics',
			ASS.[DynamicsApplyingSchoolId]
			--MR:- below not in v1.5
			--ASS.[DiocesePermissionEvidenceDocumentLink],
			--ASS.[FoundationEvidenceDocumentLink],
			--ASS.[GoverningBodyConsentEvidenceDocumentLink]
	FROM [a2b].[stg_ApplyingSchool] as ASS
	INNER JOIN [academisation].[ConversionApplication] As APP ON APP.DynamicsApplicationId = ASS.DynamicsApplicationId

	COMMIT TRAN PortDynamicsSchoolData
	--ROLLBACK TRAN PortDynamicsSchoolData

END TRY
BEGIN CATCH
  SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_SEVERITY() AS ErrorSeverity,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;

-- Transaction uncommittable
    IF (XACT_STATE()) = -1
      ROLLBACK TRANSACTION
 
-- Transaction committable
    IF (XACT_STATE()) = 1
      COMMIT TRANSACTION
END CATCH;
GO
