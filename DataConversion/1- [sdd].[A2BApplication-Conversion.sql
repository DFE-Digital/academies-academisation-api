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

**/

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
	SELECT	CASE [ApplicationType] 
				WHEN 'JoinMat' THEN 'JoinAMat'
				WHEN 'FormMat' THEN 'FormAMat'
			END as 'AppType',
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			--[ApplicationStatusId], -- MR:- ALWAYS = null on live !!!
			'InProgress' as AppStatus,
			NULL as 'FormTrustId',
			NULL as 'JoinTrustId',
			--[ApplicationSubmitted] - ???
			NULL as 'ApplicationSubmittedDate',
			[Name] as 'ApplicationReference',
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication]
	WHERE [ApplicationType] IN ('JoinMat','FormMat')

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
	SELECT 	[TrustId], -- TODO:- to confirm => Live data = 'TR00751'
			[TrustName], -- TODO:- to confirm
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			[ChangesToTrust],
			[ChangesToTrustExplained],
			[ChangesToLaGovernance],
			[ChangesToLaGovernanceExplained],
			[DynamicsApplicationId]
	FROM [sdd].[A2BApplication]
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
			[TrustName], -- TODO:- to confirm
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

	/*** STEP 4 - backfill <FormTrustId, int,> from ***/
	UPDATE CA
	SET CA.FormTrustId = AFT.Id
	FROM [academisation].[ConversionApplication] As CA
	INNER JOIN [academisation].[ApplicationFormTrust] as AFT ON AFT.[DynamicsApplicationId] = CA.[DynamicsApplicationId]

	/*** STEP 5 - backfill <JoinTrustId, int,> ***/
	UPDATE CA
	SET CA.FormTrustId = AJT.Id
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
