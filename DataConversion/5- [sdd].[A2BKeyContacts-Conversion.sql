BEGIN TRY
BEGIN TRANSACTION PortDynamicsSchoolKeyContactsData

	/*** STEP 1 - populate [academisation].[ApplicationFormTrustKeyPerson] ***/

	-- MR:- need to grab ApplicationFormTrustId
	-- by joining onto [academisation].[ConversionApplication]
	INSERT INTO [academisation].[ApplicationFormTrustKeyPerson]
           ([Name]
           ,[DateOfBirth]
           ,[Biography]
           ,[ApplicationFormTrustId]
           ,[CreatedOn]
           ,[LastModifiedOn]
           ,[DynamicsKeyPersonId])
	SELECT [Name],
		[KeyPersonDateOfBirth],
		[KeyPersonBiography],
		NewApp.[FormTrustId] as 'ApplicationFormTrustId',
		-- roles hived off to another table !!
		--,[KeyPersonCeoExecutive] = BIT = NULL
		--,[KeyPersonChairOfTrust] = BIT = NULL
		--,[KeyPersonFinancialDirector] = BIT = NULL
		--,[KeyPersonMember] = BIT = NULL
		--,[KeyPersonOther] = BIT = NULL
		--,[KeyPersonTrustee] = BIT = NULL
		GETDATE() as 'CreatedOn',
		GETDATE() as 'LastModifiedOn',
		[DynamicsKeyPersonId]
	FROM [sdd].[A2BApplicationKeyPersons] AKP
	INNER JOIN [sdd].[A2BApplication] as APP on APP.[ApplicationId] = AKP.ApplicationId
	INNER JOIN [academisation].[ConversionApplication] as NewApp on NewApp.[DynamicsApplicationId] = APP.[DynamicsApplicationId]
	WHERE NewApp.[FormTrustId] IS NOT NULL
	
	/*** STEP 2 - populate [academisation].[ApplicationFormTrustKeyPersonRole] - roles ***/
	INSERT INTO [academisation].[ApplicationFormTrustKeyPersonRole]
			   ([Role]
			   ,[TimeInRole]
			   ,[ApplicationFormTrustKeyPersonRoleId]
			   ,[CreatedOn]
			   ,[LastModifiedOn])
     --VALUES
     --      (<Role, int,>
     --      ,<TimeInRole, nvarchar(max),>
     --      ,<ApplicationFormTrustKeyPersonRoleId, int,>
     --      ,<CreatedOn, datetime2(7),>
     --      ,<LastModifiedOn, datetime2(7),>)



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
