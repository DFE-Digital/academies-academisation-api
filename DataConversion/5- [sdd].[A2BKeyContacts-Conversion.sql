Use [<database_name, sysname, sip>]
/***
from c# for data conversion:-
public enum KeyPersonRole
{
	[Description("CEO / executive")]
	CEO = 1, = 'KeyPersonCeoExecutive'
	[Description("Chair of trust")]
	Chair = 2, = 'KeyPersonChairOfTrust'
	[Description("Financial director")]
	FinancialDirector = 3, = 'KeyPersonFinancialDirector'
	[Description("Trustee")]
	Trustee = 4, = 'KeyPersonTrustee'
	[Description("Other")]
	Other = 5, = 'KeyPersonOther'
	[Description("Member")]
	Member = 6 = 'KeyPersonMember'
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
	FROM [a2b].[stg_KeyPerson] AKP
	INNER JOIN [a2b].[stg_Application] as APP on APP.[DynamicsApplicationId] = AKP.DynamicsApplicationId
	INNER JOIN [academisation].[ConversionApplication] as NewApp on NewApp.[DynamicsApplicationId] = APP.[DynamicsApplicationId]
	WHERE NewApp.[FormTrustId] IS NOT NULL
	
	-- MR:- need to un-pivot role data. hard code roleId's as part of pivot?
	DECLARE @KeyPersonRoles TABLE
	([DynamicsKeyPersonId] uniqueidentifier, 
	 KeyPersonRole nvarchar(50),
	 TrueFalse BIT,
	 NewRoleId INT
	)	
	
	INSERT INTO @KeyPersonRoles
	([DynamicsKeyPersonId],KeyPersonRole, TrueFalse, NewRoleId)

	SELECT [DynamicsKeyPersonId], Roles, TrueFalse, 
	CASE Roles 
		WHEN 'KeyPersonCeoExecutive' THEN 1
		WHEN 'KeyPersonChairOfTrust' THEN 2
		WHEN 'KeyPersonFinancialDirector' THEN 3
		WHEN 'KeyPersonTrustee' THEN 4
		WHEN 'KeyPersonOther' THEN 5
		WHEN 'KeyPersonMember' THEN 6
	END as NewRoleId
	FROM   
		(SELECT [DynamicsKeyPersonId], [KeyPersonCeoExecutive],[KeyPersonChairOfTrust],
	[KeyPersonFinancialDirector], [KeyPersonMember],[KeyPersonOther],[KeyPersonTrustee]
		FROM [a2b].[stg_KeyPerson]) p  
	UNPIVOT  
		(TrueFalse FOR Roles IN   
			([KeyPersonCeoExecutive],[KeyPersonChairOfTrust],
	[KeyPersonFinancialDirector], [KeyPersonMember],[KeyPersonOther],[KeyPersonTrustee])  
	)AS unpvt; 

	--SELECT *
	--FROM @KeyPersonRoles 

	/*** STEP 2 - populate [academisation].[ApplicationFormTrustKeyPersonRole] - roles ***/
	INSERT INTO [academisation].[ApplicationFormTrustKeyPersonRole]
			   ([ApplicationFormTrustKeyPersonRoleId]
			   ,[Role]
			   ,[TimeInRole]
			   ,[CreatedOn]
			   ,[LastModifiedOn])
	 SELECT AKPNEW.[Id] as [ApplicationFormTrustKeyPersonRoleId],
			NewRoleId as 'Role',
			'' as [TimeInRole],
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn'
	 FROM [academisation].[ApplicationFormTrustKeyPerson] AKPNEW
	 INNER JOIN @KeyPersonRoles AKP ON AKP.[DynamicsKeyPersonId] = AKPNEW.[DynamicsKeyPersonId]
	 
	COMMIT TRAN PortDynamicsKeyContactsData
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
