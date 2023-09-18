Use [<database_name, sysname, sip>]
GO

BEGIN TRY
BEGIN TRANSACTION PortTransferProjectAcademiesData

INSERT INTO [academisation].[TransferringAcademy]
           ([TransferProjectId]
           ,[OutgoingAcademyUkprn]
           ,[IncomingTrustUkprn]
           ,[PupilNumbersAdditionalInformation]
           ,[LatestOfstedReportAdditionalInformation]
           ,[KeyStage2PerformanceAdditionalInformation]
           ,[KeyStage4PerformanceAdditionalInformation]
           ,[KeyStage5PerformanceAdditionalInformation])

SELECT [fk_AcademyTransferProjectId]
      ,[OutgoingAcademyUkprn]
      ,[IncomingTrustUkprn]
      ,[PupilNumbersAdditionalInformation]
      ,[LatestOfstedReportAdditionalInformation]
      ,[KeyStage2PerformanceAdditionalInformation]
      ,[KeyStage4PerformanceAdditionalInformation]
      ,[KeyStage5PerformanceAdditionalInformation]
  FROM [sdd].[TransferringAcademies]



  --COMMIT TRAN PortTransferProjectAcademiesData
ROLLBACK TRAN PortTransferProjectAcademiesData

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


