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
		 --VALUES
		 --      (<FirstName, nvarchar(max),>
		 --      ,<LastName, nvarchar(max),>
		 --      ,<EmailAddress, nvarchar(max),>
		 --      ,<Role, int,> = going to need to hard code to something !
		 --      ,<OtherRoleName, nvarchar(max),>
		 --      ,<ConversionApplicationId, int,>
		 --      ,<CreatedOn, datetime2(7),>
		 --      ,<LastModifiedOn, datetime2(7),>)
		 		 
	SELECT	substring(APP.[ApplicationLeadAuthorName], 1 , CHARINDEX(' ', APP.[ApplicationLeadAuthorName])-1) as 'FirstName',
			SUBSTRING([ApplicationLeadAuthorName], CHARINDEX(' ', [ApplicationLeadAuthorName])+1, len([ApplicationLeadAuthorName])) as 'LastName',
			APP.ApplicationLeadEmail,
		  --[ApplicationRole],
	  		CASE APP.[ApplicationRole]
				WHEN 'HeadTeacher' THEN 2
				WHEN 'ChairGovernor' THEN 1
				WHEN 'Other' THEN 2
			END as 'ContributorRole',
			--[ApplicationRoleOtherDescription]
			CASE APP.[ApplicationRole]
				WHEN 'HeadTeacher' THEN 'Head Teacher'
				WHEN 'ChairGovernor' THEN ''
				WHEN 'Other' THEN APP.[ApplicationRoleOtherDescription]
			END as 'ContributorDescription',
			NEWAPP.[Id] as 'ConversionApplicationId',
			GETDATE() as 'CreatedOn',
			GETDATE() as 'LastModifiedOn',
			APP.[DynamicsApplicationId]
	FROM [sdd].[A2BApplication] as APP
	INNER JOIN [academisation].[ConversionApplication] as NEWAPP on NEWAPP.[DynamicsApplicationId] = APP.[DynamicsApplicationId]
	WHERE APP.[ApplicationType] IN ('JoinMat','FormMat')

	--COMMIT TRAN CreateContributorsData
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
