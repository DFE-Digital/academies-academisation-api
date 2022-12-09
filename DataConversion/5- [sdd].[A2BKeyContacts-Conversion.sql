BEGIN TRY
BEGIN TRANSACTION PortDynamicsSchoolKeyContactsData

	/*** STEP 1 - populate [academisation].[??] ***/

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
	FROM [sdd].[A2BApplicationApplyingSchool] As ASS	
	INNER JOIN [sdd].[A2BSchoolLoan] as ASL ON ASL.ApplyingSchoolId = ASS.ApplyingSchoolId
	INNER JOIN [academisation].[ApplicationSchool] as SCH on SCH.DynamicsApplyingSchoolId = ASS.DynamicsApplyingSchoolId

	/*** STEP 2 - populate [academisation].[??] - roles ***/

	--COMMIT TRAN PortDynamicsSchoolKeyContactsData
	--ROLLBACK TRAN PortDynamicsSchoolKeyContactsData

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
