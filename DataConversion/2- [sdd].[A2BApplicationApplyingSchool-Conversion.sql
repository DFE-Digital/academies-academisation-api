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
			ASS.[SchoolConversionTargetDateDifferent] as 'ConversionChangeNamePlanned',-- TODO:- check spreadsheet
			0 as 'ConfirmPaySupportGrantToSchool', -- TODO:- check spreadsheet
			--ASS.[SchoolSupportGrantFundsPaidTo],
			0 as 'SupportGrantFundsPaidTo', -- TODO:- check spreadsheet
			--**** additional info ****
			--ASS.[SchoolFaithSchool] - not in v1.5 schema ??
			NULL as 'DioceseFolderIdentifier',
			ASS.[SchoolFaithSchoolDioceseName],
			NULL as 'FoundationConsentFolderIdentifier',
			ASS.[SchoolSupportedFoundationBodyName], -- TODO:- check spreadsheet
			ASS.[SchoolFurtherInformation], -- TODO:- check spreadsheet
			--ASS.[SchoolLaClosurePlans], - not in v1.5 schema ??
			ASS.[SchoolLaClosurePlansExplain],
			--ASS.[SchoolLaClosurePlans], - not in v1.5 schema ??
			ASS.[SchoolLaReorganizationExplain],
			--ASS.[SchoolAdInspectedButReportNotPublished], - not in v1.5 schema ??
			ASS.[SchoolAdInspectedReportNotPublishedExplain],
			NULL AS 'ResolutionConsentFolderIdentifier',
			--ASS.[SchoolAdSafeguarding], - not in v1.5 schema ??
			ASS.[SchoolAdSafeguardingExplained],
			'' as 'TrustBenefitDetails', -- TODO:- check spreadsheet
			-- **** CFY / NFY / PFY details ****
			ASS.[SchoolCFYCapitalForward],
			ASS.[SchoolCFYCapitalForwardStatusExplained],
			NULL as 'CurrentFinancialYearCapitalCarryForwardFileLink',
			ASS.[SchoolCFYCapitalIsDeficit], -- TODO:- check spreadsheet. bit -> int v1.5
			ASS.[SchoolCFYEndDate],
			ASS.[SchoolCFYRevenue],
			ASS.[SchoolCFYRevenueIsDeficit], -- TODO:- check spreadsheet. bit -> int v1.5
			ASS.[SchoolCFYRevenueStatusExplained],
			NULL as 'CurrentFinancialYearRevenueStatusFileLink',
			ASS.[SchoolNFYCapitalForward],
			ASS.[SchoolCFYCapitalForwardStatusExplained],
			NULL as 'NextFinancialYearCapitalCarryForwardFileLink',
			ASS.[SchoolNFYRevenueIsDeficit],  -- TODO:- check spreadsheet. bit -> int v1.5
			ASS.[SchoolNFYEndDate],
			ASS.[SchoolNFYRevenue],
			ASS.[SchoolNFYRevenueIsDeficit],  -- TODO:- check spreadsheet. bit -> int v1.5
			ASS.[SchoolNFYRevenueStatusExplained],
			NULL as 'NextFinancialYearRevenueStatusFileLink',
			ASS.[SchoolPFYCapitalForward],
			ASS.[SchoolPFYCapitalForwardStatusExplained],
			NULL as 'PreviousFinancialYearCapitalCarryForwardFileLink',
			ASS.[SchoolPFYCapitalIsDeficit], -- TODO:- check spreadsheet. bit -> int v1.5
			ASS.[SchoolPFYEndDate],
			ASS.[SchoolPFYRevenue],
			ASS.[SchoolPFYRevenueIsDeficit],
			ASS.[SchoolPFYRevenueStatusExplained],
			NULL as 'PreviousFinancialYearRevenueStatusFileLink',
			-- ****
			ASS.[SchoolConsultationStakeholders],
			ASS.[SchoolConsultationStakeholdersConsult],
			ASS.[SchoolFinancialInvestigations],
			ASS.[SchoolFinancialInvestigationsExplain],
			ASS.[SchoolFinancialInvestigationsTrustAware],
			-- ****
			ASS.[SchoolDeclarationBodyAgree],
			ASS.[SchoolDeclarationTeacherChair],
			ASS.[SchoolDeclarationSignedByEmail], -- TODO:- check spreadsheet.
			-- ****
			ASS.[SchoolConversionReasonsForJoining],
			-- ****
			--ASS.[SchoolSACREExemption], - not in v1.5 schema ??
			ASS.[SchoolSACREExemptionEndDate],
			-- ****
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
