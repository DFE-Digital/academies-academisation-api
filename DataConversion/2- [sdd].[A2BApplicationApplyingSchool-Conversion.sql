BEGIN TRY
BEGIN TRANSACTION PortDynamicsSchoolData

	/*** populate [academisation].[ApplicationSchool] ***/
	-- MR:- below are nullable - backfill afterwards - as part of leases && loans conversion
	--,[HasLeases]
	--,[HasLoans]

	-- TODO MR: will need to grab DB generated [ConversionApplicationId]
	-- by joining onto [academisation].[ConversionApplication]

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
			   ,[DioceseFolderIdentifier]
			   ,[DioceseName]
			   ,[FoundationConsentFolderIdentifier]
			   ,[FoundationTrustOrBodyName]
			   ,[FurtherInformation]
			   ,[LocalAuthorityClosurePlanDetails]
			   ,[LocalAuthorityReoganisationDetails]
			   ,[OfstedInspectionDetails]
			   ,[ResolutionConsentFolderIdentifier]
			   ,[SafeguardingDetails]
			   ,[TrustBenefitDetails]
			   ,[CurrentFinancialYearCapitalCarryForward]
			   ,[CurrentFinancialYearCapitalCarryForwardExplained]
			   ,[CurrentFinancialYearCapitalCarryForwardFileLink]
			   ,[CurrentFinancialYearCapitalCarryForwardStatus]
			   ,[CurrentFinancialYearEndDate]
			   ,[CurrentFinancialYearRevenue]
			   ,[CurrentFinancialYearRevenueStatus]
			   ,[CurrentFinancialYearRevenueStatusExplained]
			   ,[CurrentFinancialYearRevenueStatusFileLink]
			   ,[NextFinancialYearCapitalCarryForward]
			   ,[NextFinancialYearCapitalCarryForwardExplained]
			   ,[NextFinancialYearCapitalCarryForwardFileLink]
			   ,[NextFinancialYearCapitalCarryForwardStatus]
			   ,[NextFinancialYearEndDate]
			   ,[NextFinancialYearRevenue]
			   ,[NextFinancialYearRevenueStatus]
			   ,[NextFinancialYearRevenueStatusExplained]
			   ,[NextFinancialYearRevenueStatusFileLink]
			   ,[PreviousFinancialYearCapitalCarryForward]
			   ,[PreviousFinancialYearCapitalCarryForwardExplained]
			   ,[PreviousFinancialYearCapitalCarryForwardFileLink]
			   ,[PreviousFinancialYearCapitalCarryForwardStatus]
			   ,[PreviousFinancialYearEndDate]
			   ,[PreviousFinancialYearRevenue]
			   ,[PreviousFinancialYearRevenueStatus]
			   ,[PreviousFinancialYearRevenueStatusExplained]
			   ,[PreviousFinancialYearRevenueStatusFileLink]
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
			   ,[HasLeases]
			   ,[HasLoans]
			   ,[DynamicsApplyingSchoolId])
		 --VALUES
			--   (<Urn, int,>
			--   ,<ConversionApplicationId, int,>
			--   ,<CreatedOn, datetime2(7),>
			--   ,<LastModifiedOn, datetime2(7),>
			--   ,<ProjectedPupilNumbersYear1, int,>
			--   ,<ProjectedPupilNumbersYear2, int,>
			--   ,<ProjectedPupilNumbersYear3, int,>
			--   ,<ProposedNewSchoolName, nvarchar(max),>
			--   ,<MainContactOtherTelephone, nvarchar(max),>
			--   ,<CapacityPublishedAdmissionsNumber, int,>
			--   ,<ApproverContactEmail, nvarchar(max),>
			--   ,<ApproverContactName, nvarchar(max),>
			--   ,<CapacityAssumptions, nvarchar(max),>
			--   ,<ContactChairEmail, nvarchar(max),>
			--   ,<ContactChairName, nvarchar(max),>
			--   ,<ContactChairTel, nvarchar(max),>
			--   ,<ContactHeadEmail, nvarchar(max),>
			--   ,<ContactHeadName, nvarchar(max),>
			--   ,<ContactHeadTel, nvarchar(max),>
			--   ,<ContactRole, nvarchar(max),>
			--   ,<ConversionTargetDateExplained, nvarchar(max),>
			--   ,<JoinTrustReason, nvarchar(max),>
			--   ,<MainContactOtherEmail, nvarchar(max),>
			--   ,<MainContactOtherName, nvarchar(max),>
			--   ,<MainContactOtherRole, nvarchar(max),>
			--   ,<SchoolName, nvarchar(max),>
			--   ,<FacilitiesShared, bit,>
			--   ,<FacilitiesSharedExplained, nvarchar(max),>
			--   ,<Grants, bit,>
			--   ,<GrantsAwardingBodies, nvarchar(max),>
			--   ,<OwnerExplained, nvarchar(max),>
			--   ,<PartOfBuildingSchoolsForFutureProgramme, bit,>
			--   ,<PartOfPfiScheme, bit,>
			--   ,<PartOfPfiSchemeType, nvarchar(max),>
			--   ,<PartOfPrioritySchoolsBuildingProgramme, bit,>
			--   ,<WorksPlanned, bit,>
			--   ,<WorksPlannedDate, datetime2(7),>
			--   ,<WorksPlannedExplained, nvarchar(max),>
			--   ,<ConversionTargetDate, datetime2(7),>
			--   ,<ConversionTargetDateSpecified, bit,>
			--   ,<ConversionChangeNamePlanned, bit,>
			--   ,<ConfirmPaySupportGrantToSchool, bit,>
			--   ,<SupportGrantFundsPaidTo, int,>
			--   ,<DioceseFolderIdentifier, nvarchar(max),>
			--   ,<DioceseName, nvarchar(max),>
			--   ,<FoundationConsentFolderIdentifier, nvarchar(max),>
			--   ,<FoundationTrustOrBodyName, nvarchar(max),>
			--   ,<FurtherInformation, nvarchar(max),>
			--   ,<LocalAuthorityClosurePlanDetails, nvarchar(max),>
			--   ,<LocalAuthorityReoganisationDetails, nvarchar(max),>
			--   ,<OfstedInspectionDetails, nvarchar(max),>
			--   ,<ResolutionConsentFolderIdentifier, nvarchar(max),>
			--   ,<SafeguardingDetails, nvarchar(max),>
			--   ,<TrustBenefitDetails, nvarchar(max),>
			--   ,<CurrentFinancialYearCapitalCarryForward, decimal(18,2),>
			--   ,<CurrentFinancialYearCapitalCarryForwardExplained, nvarchar(max),>
			--   ,<CurrentFinancialYearCapitalCarryForwardFileLink, nvarchar(max),>
			--   ,<CurrentFinancialYearCapitalCarryForwardStatus, int,>
			--   ,<CurrentFinancialYearEndDate, datetime2(7),>
			--   ,<CurrentFinancialYearRevenue, decimal(18,2),>
			--   ,<CurrentFinancialYearRevenueStatus, int,>
			--   ,<CurrentFinancialYearRevenueStatusExplained, nvarchar(max),>
			--   ,<CurrentFinancialYearRevenueStatusFileLink, nvarchar(max),>
			--   ,<NextFinancialYearCapitalCarryForward, decimal(18,2),>
			--   ,<NextFinancialYearCapitalCarryForwardExplained, nvarchar(max),>
			--   ,<NextFinancialYearCapitalCarryForwardFileLink, nvarchar(max),>
			--   ,<NextFinancialYearCapitalCarryForwardStatus, int,>
			--   ,<NextFinancialYearEndDate, datetime2(7),>
			--   ,<NextFinancialYearRevenue, decimal(18,2),>
			--   ,<NextFinancialYearRevenueStatus, int,>
			--   ,<NextFinancialYearRevenueStatusExplained, nvarchar(max),>
			--   ,<NextFinancialYearRevenueStatusFileLink, nvarchar(max),>
			--   ,<PreviousFinancialYearCapitalCarryForward, decimal(18,2),>
			--   ,<PreviousFinancialYearCapitalCarryForwardExplained, nvarchar(max),>
			--   ,<PreviousFinancialYearCapitalCarryForwardFileLink, nvarchar(max),>
			--   ,<PreviousFinancialYearCapitalCarryForwardStatus, int,>
			--   ,<PreviousFinancialYearEndDate, datetime2(7),>
			--   ,<PreviousFinancialYearRevenue, decimal(18,2),>
			--   ,<PreviousFinancialYearRevenueStatus, int,>
			--   ,<PreviousFinancialYearRevenueStatusExplained, nvarchar(max),>
			--   ,<PreviousFinancialYearRevenueStatusFileLink, nvarchar(max),>
			--   ,<SchoolHasConsultedStakeholders, bit,>
			--   ,<SchoolPlanToConsultStakeholders, nvarchar(max),>
			--   ,<FinanceOngoingInvestigations, bit,>
			--   ,<FinancialInvestigationsExplain, nvarchar(max),>
			--   ,<FinancialInvestigationsTrustAware, bit,>
			--   ,<DeclarationBodyAgree, bit,>
			--   ,<DeclarationIAmTheChairOrHeadteacher, bit,>
			--   ,<DeclarationSignedByName, nvarchar(max),>
			--   ,<SchoolConversionReasonsForJoining, nvarchar(max),>
			--   ,<ExemptionEndDate, datetimeoffset(7),>
			--   ,<MainFeederSchools, nvarchar(max),>
			--   ,<PartOfFederation, bit,>
			--   ,<ProtectedCharacteristics, int,>
			--   ,<HasLeases, bit,>
			--   ,<HasLoans, bit,>
			--   ,<DynamicsApplyingSchoolId, uniqueidentifier,>)

	SELECT ASS.[Urn],
			APP.Id,
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			-- TODO:- the rest !!!!
			NULL as HasLeases,
			NULL as HasLoans,
			ASS.[DynamicsApplyingSchoolId]
	FROM [sdd].[A2BApplicationApplyingSchool] as ASS
	INNER JOIN [academisation].[ConversionApplication] As APP ON APP.DynamicsApplicationId = ASS.DynamicsApplicationId

	-- TODO MR:- add [DynamicsApplicationId] to [sdd].[A2BApplicationApplyingSchool]

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
