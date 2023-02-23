Use [<database_name, sysname, sip>]
/**
[ApplicationType] - dynamics -> as-is mapping
	100000001 => 'JoinMat',
	907660000 => 'FormMat',
	907660001 => 'FormSat'
**/

/**
[ApplicationType] - as-is -> v1.5
	'JoinMat' => 'JoinAMat'
	'FormMat' = > 'FormAMat'
**/

/**
[ApplicationStatus] - as-is -> v1.5
ALWAYS = null on live !!!
so, hard code it to => 'InProgress'
**/

/** from c# ->
public enum ApplicationTypes
{
	JoinAMat,
	FormAMat
	//[Description("Form new single academy trust")]
	//FormASat
}

public enum ApplicationStatus
{
	InProgress,
	Submitted
}

public enum TrustChange
{
	[Description("Yes")]
	Yes = 1,
	[Description("No")]
	No = 2,
	[Description("Unknown at this point")]
	Unknown = 3
}

**/

BEGIN TRY
BEGIN TRANSACTION PortDynamicsApplicationData

SET IDENTITY_INSERT [academisation].[ConversionApplication] ON;

	/*** TODO:- set [academisation].[ConversionApplication].[Id] = 10000 ***/

	/*** STEP 1 - populate [academisation].[ConversionApplication] ***/
	-- MR:- below are nullable - backfill as 4th / 5th step
	--(FormTrustId, int)
	--(JoinTrustId, int)
	INSERT INTO [academisation].[ConversionApplication]
           ([Id]
		   ,[ApplicationType]
           ,[CreatedOn]
           ,[LastModifiedOn]
           ,[ApplicationStatus]
           ,[FormTrustId]
           ,[JoinTrustId]
           ,[ApplicationSubmittedDate]
           ,[DynamicsApplicationId])
	SELECT	
	CAST(value AS int) as Id,
	CASE app.[ApplicationType] 
				WHEN 100000001 THEN 'JoinAMat'
				WHEN 907660000 THEN 'FormAMat'
			END as 'AppType',
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
	CASE app.[ApplicationSubmitted]
		WHEN 1 THEN 'Submitted'
		WHEN 0 THEN 'InProgress'
	END as 'AppStatus',
			NULL as 'FormTrustId',
			NULL as 'JoinTrustId',
	CASE app.[ApplicationSubmitted]
		WHEN 1 THEN app.ApplicationSubmittedOn -- the submitted dated is not in sdd so giving it date of now
		WHEN 0 THEN NULL
	END as 'ApplicationSubmittedDate',
			app.[DynamicsApplicationId]
	FROM [a2b].[stg_Application] app
	CROSS APPLY STRING_SPLIT([Name], '_')
	LEFT OUTER JOIN [academisation].[ConversionApplication] newApp on newApp.DynamicsApplicationId = app.DynamicsApplicationId
	WHERE app.[ApplicationType] IN (100000001,907660000) 
	AND app.[Name] LIKE 'A2B_%' 
	AND value <> 'A2B'
	AND newApp.DynamicsApplicationId is null

	SET IDENTITY_INSERT [academisation].[ConversionApplication] OFF;

	/*** reseed the table so all new applications can be identified from imported ones ***/
  DBCC CHECKIDENT ('[academisation].[ConversionApplication]', RESEED, 10000);

	/*** STEP 2 - populate [academisation].[ApplicationJoinTrust] ***/
	INSERT INTO [academisation].[ApplicationJoinTrust]
			   ([UKPRN]
			   ,[TrustReference]			   
			   ,[TrustName]
			   ,[CreatedOn]
			   ,[LastModifiedOn]
			   ,[ChangesToTrust]
			   ,[ChangesToTrustExplained]
			   ,[ChangesToLaGovernance]
			   ,[ChangesToLaGovernanceExplained]
			   ,[DynamicsApplicationId])
	SELECT 	[grp].[UKPRN],
			[TrustId], -- INT in v1.5, string in as-is !! TODO:- to confirm => Live data = 'TR00751'
			app.[TrustName], -- TODO:- to confirm - service support team !!
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			--[ChangesToTrust], -- convert bit -> enum value. god knows what happens to 'unknown in dynamics'!!
			CASE app.ChangesToTrust
				WHEN 907660001 THEN 2
				WHEN 907660000 THEN 1
			END as 'ChangesToTrust',
			app.[ChangesToTrustExplained],
			CASE app.ChangesToLaGovernance
				WHEN 907660001 THEN 0
				WHEN 907660000 THEN 1
			END as 'ChangesToLaGovernance',
			app.[ChangesToLaGovernanceExplained],
			app.[DynamicsApplicationId]
	FROM [a2b].[stg_Application] app
	INNER JOIN [gias].[Group] grp ON app.TrustId = grp.[Group ID] and grp.[UKPRN] IS NOT NULL
    LEFT OUTER JOIN [academisation].[ApplicationJoinTrust] newApp on newApp.DynamicsApplicationId = app.DynamicsApplicationId
	WHERE ApplicationType = 100000001 
	AND newApp.DynamicsApplicationId is null;
	
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
	SELECT 	app.[TrustApproverName],
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			CASE app.[FormTrustGrowthPlansYesNo]
				WHEN 907660001 THEN 0
				WHEN 907660000 THEN 1
			END as 'FormTrustGrowthPlansYesNo',
			app.[FormTrustImprovementApprovedSponsor],
			app.[FormTrustImprovementStrategy],
			app.[FormTrustImprovementSupport],
			app.[FormTrustOpeningDate],
			app.[FormTrustPlanForGrowth],
			app.[FormTrustPlansForNoGrowth],
			app.[FormTrustProposedNameOfTrust],
			CASE app.[FormTrustReasonApprovalToConvertAsSat]
				WHEN 907660001 THEN 0
				WHEN 907660000 THEN 1
			END as 'FormTrustReasonApprovalToConvertAsSat',
			app.[FormTrustReasonApprovedPerson],
			app.[FormTrustReasonForming],
			app.[FormTrustReasonFreedom],
			app.[FormTrustReasonGeoAreas],
			app.[FormTrustReasonImproveTeaching],
			app.[FormTrustReasonVision],
			app.[TrustApproverEmail],
			app.[DynamicsApplicationId]
	FROM [a2b].[stg_Application] app
	LEFT OUTER JOIN [academisation].[ApplicationFormTrust] newApp on newApp.DynamicsApplicationId = app.DynamicsApplicationId
	WHERE ApplicationType = 907660000
	AND newApp.DynamicsApplicationId is null;

	/*** STEP 4 - backfill (FormTrustId, int) from ***/
	UPDATE CA
	SET CA.FormTrustId = AFT.Id
	FROM [academisation].[ConversionApplication] As CA
	INNER JOIN [academisation].[ApplicationFormTrust] as AFT ON AFT.[DynamicsApplicationId] = CA.[DynamicsApplicationId]

	/*** STEP 5 - backfill (JoinTrustId, int) ***/
	UPDATE CA
	SET CA.JoinTrustId = AJT.Id
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
