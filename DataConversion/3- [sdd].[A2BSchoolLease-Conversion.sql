Use [<database_name, sysname, sip>]

BEGIN TRY
BEGIN TRANSACTION PortDynamicsSchoolLeaseData

	/*** STEP 1 - populate [academisation].[ApplicationSchoolLease] ***/

	-- MR:- need to grab DB generated [ApplicationSchoolId]
	-- by joining onto [academisation].[ApplicationSchool]
	INSERT INTO [academisation].[ApplicationSchoolLease]
			   ([LeaseTerm]
			   ,[RepaymentAmount]
			   ,[InterestRate]
			   ,[PaymentsToDate]
			   ,[Purpose]
			   ,[ValueOfAssets]
			   ,[ResponsibleForAssets]
			   ,[ApplicationSchoolId]
			   ,[CreatedOn]
			   ,[LastModifiedOn]
			   ,[DynamicsSchoolLeaseId])

	 -- TODO MR:- negative amounts
	SELECT ASL.SchoolLeaseTerm,
			ASL.[SchoolLeaseRepaymentValue],
			ASL.[SchoolLeaseInterestRate],
			ASL.SchoolLeasePaymentToDate,
			ASL.SchoolLeasePurpose,
			ASL.[SchoolLeaseValueOfAssets],
			ASL.[SchoolLeaseResponsibleForAssets],
			SCH.Id as 'ApplicationSchoolId',
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			ASL.DynamicsSchoolLeaseId
			--ASS.[DynamicsApplyingSchoolId] -- other
	FROM [sdd].[A2BApplicationApplyingSchool] As ASS	
	INNER JOIN [sdd].[A2BSchoolLease] as ASL ON ASL.ApplyingSchoolId = ASS.ApplyingSchoolId
	INNER JOIN [academisation].[ApplicationSchool] as SCH on SCH.DynamicsApplyingSchoolId = ASS.DynamicsApplyingSchoolId
	   
	/* STEP 2 - backfill [academisation].[ApplicationSchool].HasLeases */
	-- MR:- below are nullable - backfill afterwards - as part of leases && loans conversion
	--,[HasLeases]
	UPDATE SCH
	SET SCH.HasLeases = 1
	FROM [academisation].[ApplicationSchool] as SCH
	INNER JOIN [academisation].[ApplicationSchoolLease] as ASL ON ASL.ApplicationSchoolId = SCH.Id

	COMMIT TRAN PortDynamicsSchoolLeaseData
	--ROLLBACK TRAN PortDynamicsSchoolLeaseData

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
