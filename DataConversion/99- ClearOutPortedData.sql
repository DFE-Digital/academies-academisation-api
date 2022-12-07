BEGIN TRY
BEGIN TRANSACTION PortDynamicsClearData

	/*** STEP 1 - clear out [academisation].[ApplicationSchoolLeases] ***/
	DELETE FROM [academisation].[ApplicationSchoolLease]
	WHERE [DynamicsSchoolLeaseId] IS NOT NULL

	/*** STEP 2 - clear out [academisation].[ApplicationSchoolLoans] ***/
	DELETE FROM [academisation].[ApplicationSchoolLoan]
	WHERE [DynamicsSchoolLoanId] IS NOT NULL

	/*** STEP 3 - clear out [academisation].[ConversionApplicationContributor] ***/
	DELETE FROM [academisation].[ConversionApplicationContributor]
	WHERE [DynamicsApplicationId] IS NOT NULL -- TODO

	/*** STEP 4 - clear out [academisation].[ApplicationFormTrust] ***/
	DELETE FROM [academisation].[ApplicationFormTrust]
	WHERE [DynamicsApplicationId] IS NOT NULL -- TODO

	/*** STEP 5 - clear out [academisation].[ApplicationJoinTrust] ***/
	DELETE FROM [academisation].[ApplicationJoinTrust]
	WHERE [DynamicsApplicationId] IS NOT NULL -- TODO

	/*** STEP 6 - clear out [academisation].[ApplicationSchool] ***/
	DELETE FROM [academisation].[ApplicationSchool]
	WHERE [DynamicsApplyingSchoolId] IS NOT NULL

	/*** STEP 7 - clear out [academisation].[ConversionApplication] ***/
	DELETE FROM [academisation].[ConversionApplication]
	WHERE [DynamicsApplicationId] IS NOT NULL

	--COMMIT TRAN PortDynamicsClearData
	--ROLLBACK TRAN PortDynamicsClearData

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
