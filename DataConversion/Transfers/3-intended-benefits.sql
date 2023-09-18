Use [<database_name, sysname, sip>]
GO

BEGIN TRY
BEGIN TRANSACTION PortIntendedBenefitsData

INSERT INTO [academisation].[IntendedTransferBenefit]
           ([TransferProjectId]
           ,[SelectedBenefit])

SELECT [fk_AcademyTransferProjectId]
      ,[SelectedBenefit]
  FROM [sdd].[AcademyTransferProjectIntendedTransferBenefits]

  --COMMIT TRAN PortIntendedBenefitsData
ROLLBACK TRAN PortIntendedBenefitsData

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




