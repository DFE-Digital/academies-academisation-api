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
	CASE [ApplicationType] 
				WHEN 'JoinMat' THEN 'JoinAMat'
				WHEN 'FormMat' THEN 'FormAMat'
			END as 'AppType',
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
	CASE [ApplicationSubmitted]
		WHEN 1 THEN 'Submitted'
		WHEN 0 THEN 'InProgress'
	END as 'AppStatus',
			NULL as 'FormTrustId',
			NULL as 'JoinTrustId',
	CASE [ApplicationSubmitted]
		WHEN 1 THEN GETDATE() -- the submitted dated is not in sdd so giving it date of now
		WHEN 0 THEN NULL
	END as 'ApplicationSubmittedDate',
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication] 
	CROSS APPLY STRING_SPLIT([Name], '_')
	WHERE [ApplicationType] IN ('JoinMat','FormMat') 
	AND [Name] LIKE 'A2B_%' 
	AND value <> 'A2B'

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
			[TrustName], -- TODO:- to confirm - service support team !!
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			--[ChangesToTrust], -- convert bit -> enum value. god knows what happens to 'unknown in dynamics'!!
			CASE ChangesToTrust
				WHEN 0 THEN 2
				WHEN 1 THEN 1
			END as 'ChangesToTrust',
			[ChangesToTrustExplained],
			[ChangesToLaGovernance],
			[ChangesToLaGovernanceExplained],
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication] app
	INNER JOIN [gias].[Group] grp ON app.TrustId = grp.[Group ID]
	WHERE ApplicationType = 'JoinMat';
	
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
	SELECT 	[TrustApproverName],
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			[FormTrustGrowthPlansYesNo],
			[FormTrustImprovementApprovedSponsor],
			[FormTrustImprovementStrategy],
			[FormTrustImprovementSupport],
			[FormTrustOpeningDate],
			[FormTrustPlanForGrowth],
			[FormTrustPlansForNoGrowth],
			[FormTrustProposedNameOfTrust],
			[FormTrustReasonApprovalToConvertAsSat],
			[FormTrustReasonApprovedPerson],
			[FormTrustReasonForming],
			[FormTrustReasonFreedom],
			[FormTrustReasonGeoAreas],
			[FormTrustReasonImproveTeaching],
			[FormTrustReasonVision],
			[TrustApproverEmail],
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication]
	WHERE ApplicationType = 'FormMat';

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

	--COMMIT TRAN PortDynamicsApplicationData
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
