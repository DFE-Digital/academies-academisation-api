BEGIN TRY
BEGIN TRANSACTION PortDynamicsData

	/*** populate [sdd].[A2BApplication] ***/
	INSERT INTO

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
