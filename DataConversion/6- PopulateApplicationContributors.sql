Use [<database_name, sysname, sip>]
/****
Role enum from c#
public enum SchoolRoles
{
	[Description("The chair of the school's governors")]
	ChairOfGovernors = 1,
	[Description("Something else")]
	Other = 2
}

****/

BEGIN TRY
BEGIN TRANSACTION CreateContributorsData

	/*** STEP 1 - populate [academisation].[ConversionApplicationContributor] ***/

	-- MR:- need to grab DB generated [ConversionApplicationId]
	-- by joining onto [academisation].[Application]
	INSERT INTO [academisation].[ConversionApplicationContributor]
			   ([FirstName]
			   ,[LastName]
			   ,[EmailAddress]
			   ,[Role]
			   ,[OtherRoleName]
			   ,[ConversionApplicationId]
			   ,[CreatedOn]
			   ,[LastModifiedOn]
			   ,[DynamicsApplicationId])
	SELECT	substring(APP.[ApplicationLeadAuthorName], 1 , CHARINDEX(' ', APP.[ApplicationLeadAuthorName])-1) as 'FirstName',
			SUBSTRING([ApplicationLeadAuthorName], CHARINDEX(' ', [ApplicationLeadAuthorName])+1, len([ApplicationLeadAuthorName])) as 'LastName',
			APP.ApplicationLeadEmail,
		  --[ApplicationRole],
	  		CASE APP.[ApplicationRole]
				WHEN 907660000 THEN 2
				WHEN 907660001 THEN 1
				WHEN 907660002 THEN 2
			END as 'ContributorRole',
			--[ApplicationRoleOtherDescription]
			CASE APP.[ApplicationRole]
				WHEN 907660000 THEN 'Head Teacher'
				WHEN 907660001 THEN ''
				WHEN 907660002 THEN APP.[ApplicationRoleOtherDescription]
			END as 'ContributorDescription',
			NEWAPP.[Id] as 'ConversionApplicationId',
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			APP.[DynamicsApplicationId]
	FROM [a2b].[stg_Application] as APP
	INNER JOIN [academisation].[ConversionApplication] as NEWAPP on NEWAPP.[DynamicsApplicationId] = APP.[DynamicsApplicationId]
	WHERE APP.[ApplicationType] IN (100000001,907660000) 

	COMMIT TRAN CreateContributorsData
	--ROLLBACK TRAN CreateContributorsData

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
