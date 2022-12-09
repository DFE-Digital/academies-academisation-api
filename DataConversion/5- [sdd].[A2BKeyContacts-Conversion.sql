/***
from c# for data conversion:-
	public enum KeyPersonRole
	{
		[Description("CEO")]
		CEO = 1,
		[Description("The chair of the trust")]
		Chair = 2,
		[Description("Financial director")]
		FinancialDirector = 3,
		[Description("Trustee")]
		Trustee = 4,
		[Description("Other")]
		Other = 5
	}

***/

BEGIN TRY
BEGIN TRANSACTION PortDynamicsKeyContactsData

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
	SELECT AKP.[Name],
			AKP.[KeyPersonDateOfBirth],
			AKP.[KeyPersonBiography],
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
	
	-- TODO MR:- need to pivot role data. hard code roleId's as part of pivot?
	-- Unpivot the table.  
	SELECT [KeyPersonId],[DynamicsKeyPersonId], Roles, TrueFalse  
	FROM   
	   (SELECT [KeyPersonId],[DynamicsKeyPersonId], [KeyPersonCeoExecutive],[KeyPersonChairOfTrust],
	[KeyPersonFinancialDirector], [KeyPersonMember],[KeyPersonOther],[KeyPersonTrustee]
	   FROM [sdd].[A2BApplicationKeyPersons]) p  
	UNPIVOT  
	   (TrueFalse FOR Roles IN   
		  ([KeyPersonCeoExecutive],[KeyPersonChairOfTrust],
	[KeyPersonFinancialDirector], [KeyPersonMember],[KeyPersonOther],[KeyPersonTrustee])  
	)AS unpvt;  

	/*** STEP 2 - populate [academisation].[ApplicationFormTrustKeyPersonRole] - roles ***/
	INSERT INTO [academisation].[ApplicationFormTrustKeyPersonRole]
			   ([ApplicationFormTrustKeyPersonRoleId]
			   ,[Role]
			   ,[TimeInRole]
			   ,[CreatedOn]
			   ,[LastModifiedOn])
     --VALUES      --      (,<ApplicationFormTrustKeyPersonRoleId, int,>
	 --<Role, int,> = see enum values above !!!
     --      ,<TimeInRole, nvarchar(max),>
     --      ,<CreatedOn, datetime2(7),>
     --      ,<LastModifiedOn, datetime2(7),>)
	 SELECT AKPNEW.[Id] as [ApplicationFormTrustKeyPersonRoleId],
			1 as 'Role',
			'' as [TimeInRole],
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn'
	 FROM [academisation].[ApplicationFormTrustKeyPerson] AKPNEW
	 INNER JOIN [sdd].[A2BApplicationKeyPersons] AKP ON AKP.[DynamicsKeyPersonId] = AKPNEW.[DynamicsKeyPersonId]
	 
	--COMMIT TRAN PortDynamicsKeyContactsData
	--ROLLBACK TRAN PortDynamicsKeyContactsData

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
