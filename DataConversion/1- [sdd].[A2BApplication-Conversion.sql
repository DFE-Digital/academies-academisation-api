BEGIN TRY
BEGIN TRANSACTION PortDynamicsData

	/*** populate [academisation].[ConversionApplication] ***/
	-- MR:- below are nullable - backfill afterwards?
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
--     VALUES
--           (<ApplicationType, nvarchar(max),>
--           ,<CreatedOn, datetime2(7),>
--           ,<LastModifiedOn, datetime2(7),>
--           ,<ApplicationStatus, nvarchar(max),>
--           ,<FormTrustId, int,>
--           ,<JoinTrustId, int,>
--           ,<ApplicationSubmittedDate, datetime2(7),>
--           ,<ApplicationReference, nvarchar(max),>
--           ,<DynamicsApplicationId, uniqueidentifier,>)

	SELECT
	FROM [sdd].[A2BApplication]

	COMMIT TRAN PortDynamicsData
	--ROLLBACK TRAN PortDynamicsData

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
