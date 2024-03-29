Use [<database_name, sysname, sip>]


BEGIN TRY
BEGIN TRANSACTION PortDynamicsSchoolLoansData

	/*** STEP 1 - populate [academisation].[ApplicationSchoolLoan] ***/

	-- MR:- need to grab DB generated [ApplicationSchoolId]
	-- by joining onto [academisation].[ApplicationSchool]
	INSERT INTO [academisation].[ApplicationSchoolLoan]
           ([Amount]
           ,[Purpose]
           ,[Provider]
           ,[InterestRate]
           ,[Schedule]
           ,[ApplicationSchoolId]
           ,[CreatedOn]
           ,[LastModifiedOn]
           ,[DynamicsSchoolLoanId])
	 	 
	 -- TODO MR:- negative amounts
	SELECT ASL.[SchoolLoanAmount],
			ASL.[SchoolLoanPurpose],
			ASL.SchoolLoanProvider,
			ASL.SchoolLoanInterestRate,
			ASL.SchoolLoanSchedule,
			SCH.Id as 'ApplicationSchoolId',
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			ASL.DynamicsSchoolLoanId
			--ASS.[DynamicsApplyingSchoolId] -- other
	FROM [a2b].[stg_ApplyingSchool] As ASS	
	INNER JOIN [a2b].[stg_SchoolLoan] as ASL ON ASL.DynamicsApplyingSchoolId = ASS.DynamicsApplyingSchoolId
	INNER JOIN [academisation].[ApplicationSchool] as SCH on SCH.DynamicsApplyingSchoolId = ASS.DynamicsApplyingSchoolId
	LEFT OUTER JOIN [academisation].[ApplicationSchoolLoan] newLoan on newLoan.DynamicsSchoolLoanId = ASL.DynamicsSchoolLoanId
	WHERE newLoan.DynamicsSchoolLoanId is null
	
	/* STEP 2 - backfill [academisation].[ApplicationSchool].HasLoans */
	-- MR:- below are nullable - backfill afterwards - as part of leases && loans conversion
	--,[HasLoans]
	UPDATE SCH
	SET SCH.HasLoans = 1
	FROM [academisation].[ApplicationSchool] as SCH
	INNER JOIN [academisation].[ApplicationSchoolLoan] as ASL ON ASL.ApplicationSchoolId = SCH.Id

	COMMIT TRAN PortDynamicsSchoolLoansData
	--ROLLBACK TRAN PortDynamicsSchoolLoansData

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
