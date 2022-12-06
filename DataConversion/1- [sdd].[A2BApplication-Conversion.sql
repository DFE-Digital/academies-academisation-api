BEGIN TRY
BEGIN TRANSACTION PortDynamicsApplicationData

	/*** TODO:- set [academisation].[ConversionApplication].[Id] = 10000 ***/

	/*** STEP 1 - populate [academisation].[ConversionApplication] ***/
	-- MR:- below are nullable - backfill as 4th / 5th step
	--,<FormTrustId, int,>
	--,<JoinTrustId, int,>

	INSERT INTO [academisation].[ConversionApplication]
           ([ApplicationType]
           ,[CreatedOn]
           ,[LastModifiedOn]
           ,[ApplicationStatus]
           ,[FormTrustId]
           ,[JoinTrustId]
           ,[ApplicationSubmittedDate]
           ,[ApplicationReference]
           ,[DynamicsApplicationId])
	SELECT	[ApplicationType], -- TODO MR:- data conversion
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			[ApplicationStatusId], -- TODO MR:- data conversion
			NULL as 'FormTrustId',
			NULL as 'JoinTrustId',
			--[ApplicationSubmitted]
			NULL as 'ApplicationSubmittedDate',
			[Name] as 'ApplicationReference',
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication]

	/*** STEP 2 - populate [academisation].[ApplicationJoinTrust] ***/
	INSERT INTO [academisation].[ApplicationJoinTrust]
			   ([UKPRN]
			   ,[TrustName]
			   ,[CreatedOn]
			   ,[LastModifiedOn]
			   ,[ChangesToTrust]
			   ,[ChangesToTrustExplained]
			   ,[ChangesToLaGovernance]
			   ,[ChangesToLaGovernanceExplained]
			   ,[DynamicsApplicationId])
	SELECT 	[TrustId],
			[TrustName],
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			[ChangesToTrust],
			[ChangesToTrustExplained],
			[ChangesToLaGovernance],
			[ChangesToLaGovernanceExplained],
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication]
	WHERE ApplicationType = 'JAM';
	
	/*** STEP 3 - populate [academisation].[ApplicationFormTrust] ***/
	INSERT INTO [academisation].[ApplicationFormTrust]
           ([TrustApproverName]
           ,[CreatedOn]
           ,[LastModifiedOn]
           ,[FormTrustGrowthPlansYesNo]
           ,[FormTrustImprovementApprovedSponsor]
           ,[FormTrustImprovementStrategy]
           ,[FormTrustImprovementSupport]
           ,[FormTrustOpeningDate]
           ,[FormTrustPlanForGrowth]
           ,[FormTrustPlansForNoGrowth]
           ,[FormTrustProposedNameOfTrust]
           ,[FormTrustReasonApprovaltoConvertasSAT]
           ,[FormTrustReasonApprovedPerson]
           ,[FormTrustReasonForming]
           ,[FormTrustReasonFreedom]
           ,[FormTrustReasonGeoAreas]
           ,[FormTrustReasonImproveTeaching]
           ,[FormTrustReasonVision]
           ,[TrustApproverEmail]
		   ,[DynamicsApplicationId])
     --VALUES
     --      (<FormTrustGrowthPlansYesNo, bit,>
     --      ,<FormTrustImprovementApprovedSponsor, nvarchar(max),>
     --      ,<FormTrustImprovementStrategy, nvarchar(max),>
     --      ,<FormTrustImprovementSupport, nvarchar(max),>
     --      ,<FormTrustOpeningDate, datetime2(7),>
     --      ,<FormTrustPlanForGrowth, nvarchar(max),>
     --      ,<FormTrustPlansForNoGrowth, nvarchar(max),>
     --      ,<FormTrustProposedNameOfTrust, nvarchar(max),>
     --      ,<FormTrustReasonApprovaltoConvertasSAT, bit,>
     --      ,<FormTrustReasonApprovedPerson, nvarchar(max),>
     --      ,<FormTrustReasonForming, nvarchar(max),>
     --      ,<FormTrustReasonFreedom, nvarchar(max),>
     --      ,<FormTrustReasonGeoAreas, nvarchar(max),>
     --      ,<FormTrustReasonImproveTeaching, nvarchar(max),>
     --      ,<FormTrustReasonVision, nvarchar(max),>
     --      ,<TrustApproverEmail, nvarchar(max),>)
	SELECT 	[TrustApproverName],
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
		-- TODO:- the rest !!!!
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication]
	WHERE ApplicationType = 'FAM';

	/*** STEP 4 - backfill <FormTrustId, int,> from ***/
	-- TODO MR:- need to add [DynamicsApplicationId] to [academisation].[ApplicationFormTrust]
	UPDATE CA
	SET CA.FormTrustId = AFT.Id
	FROM [academisation].[ConversionApplication] As CA
	INNER JOIN [academisation].[ApplicationFormTrust] as AFT ON AFT.[DynamicsApplicationId] = CA.[DynamicsApplicationId]

	/*** STEP 5 - backfill <JoinTrustId, int,> ***/
	-- TODO MR:- need to add [DynamicsApplicationId] to [academisation].[ApplicationJoinTrust]
	UPDATE CA
	SET CA.FormTrustId = AJT.Id
	FROM [academisation].[ConversionApplication] As CA
	INNER JOIN [academisation].[ApplicationJoinTrust] as AJT ON AJT.[DynamicsApplicationId] = CA.[DynamicsApplicationId]

	COMMIT TRAN PortDynamicsApplicationData
	--ROLLBACK TRAN PortDynamicsApplicationData

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
