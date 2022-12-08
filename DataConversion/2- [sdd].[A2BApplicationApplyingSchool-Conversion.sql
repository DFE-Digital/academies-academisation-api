/** 
enums values from c#->
public enum SelectOption
{
	Yes = 1,
	No = 0
}

public enum SchoolEqualitiesProtectedCharacteristics
{
	[Description("That the Secretary of State's decision is unlikely to disproportionately affect any particular person or group who share protected characteristics")]
	Unlikely = 1,
	[Description("That there are some impacts but on balance the changes will not disproportionately affect any particular person or group who share protected characteristics")]
	WillNot = 0
}

public enum MainConversionContact
{
	[Description("The headteacher")]
	HeadTeacher = 1,
	[Description("The chair of the governing body")]
	ChairOfGoverningBody = 2,
	[Description("Someone else")]
	Other = 3
}

public enum PayFundsTo
{
	[Description("To the school")]
	School = 1,
	[Description("To the trust the school is joining")]
	Trust = 2
}

public enum EqualityImpact
{
	ConsideredUnlikely,
	ConsideredWillNot,
	NotConsidered
}

public enum RevenueType
{
	Surplus = 1,
	Deficit = 2
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
			   ,[LocalAuthorityReoganisationDetails]
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
			   ,[DynamicsApplyingSchoolId])
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
			ASS.[SchoolConversionContactRole], -- TODO MR:- need mapper - string -> int !
			ASS.[SchoolConversionTargetDateExplained],
			ASS.[SchoolConversionReasonsForJoining] as 'JoinTrustReason',
			ASS.[SchoolConversionMainContactOtherEmail],
			ASS.[SchoolConversionMainContactOtherName],
			ASS.[SchoolConversionMainContactOtherRole],
			ASS.[Name] as 'SchoolName',
			--**** land & buildings ****
			ASS.[SchoolBuildLandSharedFacilities],
			ASS.[SchoolBuildLandSharedFacilitiesExplained],
			ASS.[SchoolBuildLandGrants],
			ASS.[SchoolBuildLandGrantsBody],
			ASS.[SchoolBuildLandOwnerExplained],
			ASS.[SchoolBuildLandFutureProgramme],
			ASS.[SchoolBuildLandPFIScheme],
			ASS.[SchoolBuildLandPFISchemeType],
			ASS.[SchoolBuildLandPriorityBuildingProgramme],
			ASS.[SchoolBuildLandWorksPlanned],
			ASS.[SchoolBuildLandWorksPlannedDate],
			ASS.[SchoolBuildLandWorksPlannedExplained],
			-- ****
			ASS.[SchoolConversionTargetDateDate],
			ASS.[SchoolConversionTargetDateDifferent], -- BIT
			ASS.[SchoolConversionChangeName], -- BIT
			0 as 'ConfirmPaySupportGrantToSchool', -- BIT TODO:- ????
			ASS.[SchoolSupportGrantFundsPaidTo] as 'SupportGrantFundsPaidTo', -- TODO:- mapper strint -> int
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
			CASE ASS.[SchoolCFYCapitalIsDeficit]
				WHEN 0 THEN 1
				WHEN 1 THEN 2
			END as 'CurrentFinancialYearCapitalCarryForwardStatus',
			ASS.[SchoolCFYEndDate],
			ASS.[SchoolCFYRevenue],
			--ASS.[SchoolCFYRevenueIsDeficit], -- BIT -> int v1.5
			CASE ASS.[SchoolCFYRevenueIsDeficit]
				WHEN 0 THEN 1
				WHEN 1 THEN 2
			END as 'CurrentFinancialYearRevenueStatus',
			ASS.[SchoolCFYRevenueStatusExplained],
			ASS.[SchoolNFYCapitalForward],
			ASS.[SchoolNFYCapitalForwardStatusExplained],
			--ASS.[SchoolNFYCapitalIsDeficit], BIT -> int v1.5
			CASE ASS.[SchoolNFYCapitalIsDeficit]
				WHEN 0 THEN 1
				WHEN 1 THEN 2
			END as 'NextFinancialYearCapitalCarryForwardStatus',
			ASS.[SchoolNFYEndDate],
			ASS.[SchoolNFYRevenue],
			--ASS.[SchoolNFYRevenueIsDeficit],  -- BIT -> int v1.5
			CASE ASS.[SchoolNFYRevenueIsDeficit]
				WHEN 0 THEN 1
				WHEN 1 THEN 2
			END as 'NextFinancialYearRevenueStatus',
			ASS.[SchoolNFYRevenueStatusExplained],
			ASS.[SchoolPFYCapitalForward],
			ASS.[SchoolPFYCapitalForwardStatusExplained],
			--ASS.[SchoolPFYCapitalIsDeficit], -- BIT -> int v1.5
			CASE ASS.[SchoolPFYCapitalIsDeficit]
				WHEN 0 THEN 1
				WHEN 1 THEN 2
			END as 'PreviousFinancialYearCapitalCarryForwardStatus',
			ASS.[SchoolPFYEndDate],
			ASS.[SchoolPFYRevenue],
			ASS.[SchoolPFYRevenueIsDeficit],
			ASS.[SchoolPFYRevenueStatusExplained],
			-- ****
			ASS.[SchoolConsultationStakeholders],
			ASS.[SchoolConsultationStakeholdersConsult],
			ASS.[SchoolFinancialInvestigations],
			ASS.[SchoolFinancialInvestigationsExplain],
			ASS.[SchoolFinancialInvestigationsTrustAware],
			-- ****
			ASS.[SchoolDeclarationBodyAgree],
			ASS.[SchoolDeclarationTeacherChair],
			ASS.[SchoolDeclarationSignedByEmail] as 'DeclarationSignedByName', -- TODO:- check spreadsheet.
			-- ****
			ASS.[SchoolConversionReasonsForJoining],
			-- **** more additional info
			--ASS.[SchoolSACREExemption], BIT - not in v1.5 schema
			ASS.[SchoolSACREExemptionEndDate], -- TODO MR:- need data conversion, string in as-is !!!
			-- ****
			ASS.[SchoolAdFeederSchools],
			ASS.[SchoolPartOfFederation],
			ASS.SchoolAdEqualitiesImpactAssessmentDetails as 'ProtectedCharacteristics', -- TODO MR:- need mapper - string -> int !
			ASS.[DynamicsApplyingSchoolId]
	FROM [sdd].[A2BApplicationApplyingSchool] as ASS
	INNER JOIN [academisation].[ConversionApplication] As APP ON APP.DynamicsApplicationId = ASS.DynamicsApplicationId

	--COMMIT TRAN PortDynamicsSchoolData
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
