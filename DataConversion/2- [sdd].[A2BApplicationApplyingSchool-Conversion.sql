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
			--   (,<ConversionTargetDate, datetime2(7),>
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
			--   )

	SELECT ASS.[Urn],
			APP.Id,
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			ASS.[ProjectedPupilNumbersYear1],
			ASS.[ProjectedPupilNumbersYear2],
			ASS.[ProjectedPupilNumbersYear3],
			ASS.[SchoolConversionChangeNameValue], -- TODO:- check spreadsheet
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
			ASS.[SchoolConversionContactRole],
			--[SchoolConversionTargetDateDifferent] -- not captured v1.5
			ASS.[SchoolConversionTargetDateExplained],
			ASS.[SchoolConversionReasonsForJoining], --JoinTrustReason  -- TODO:- check spreadsheet
			ASS.[SchoolConversionMainContactOtherEmail],
			ASS.[SchoolConversionMainContactOtherName],
			ASS.[SchoolConversionMainContactOtherRole],
			ASS.[Name],  -- TODO:- check spreadsheet
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

			-- TODO:- the rest !!!!
			ASS.[SchoolAdFeederSchools],
			ASS.[SchoolPartOfFederation],
			'' as 'ProtectedCharacteristics', -- TODO:- check spreadsheet
			NULL as HasLeases, --populated in later script
			NULL as HasLoans, --populated in later script
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
