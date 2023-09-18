Use [<database_name, sysname, sip>]
GO

BEGIN TRY
BEGIN TRANSACTION PortTransferProjectData

SET IDENTITY_INSERT [academisation].[TransferProject] ON;

INSERT INTO [academisation].[TransferProject]
           ([Id]
		   ,[Urn]
           ,[ProjectReference]
           ,[OutgoingTrustUkprn]
           ,[WhoInitiatedTheTransfer]
           ,[RddOrEsfaIntervention]
           ,[RddOrEsfaInterventionDetail]
           ,[TypeOfTransfer]
           ,[OtherTransferTypeDescription]
           ,[TransferFirstDiscussed]
           ,[TargetDateForTransfer]
           ,[HtbDate]
           ,[HasTransferFirstDiscussedDate]
           ,[HasTargetDateForTransfer]
           ,[HasHtbDate]
           ,[ProjectRationale]
           ,[TrustSponsorRationale]
           ,[State]
           ,[Status]
           ,[AnyRisks]
           ,[HighProfileShouldBeConsidered]
           ,[HighProfileFurtherSpecification]
           ,[ComplexLandAndBuildingShouldBeConsidered]
           ,[ComplexLandAndBuildingFurtherSpecification]
           ,[FinanceAndDebtShouldBeConsidered]
           ,[FinanceAndDebtFurtherSpecification]
           ,[OtherRisksShouldBeConsidered]
           ,[EqualitiesImpactAssessmentConsidered]
           ,[OtherRisksFurtherSpecification]
           ,[OtherBenefitValue]
           ,[Author]
           ,[Recommendation]
           ,[IncomingTrustAgreement]
           ,[DiocesanConsent]
           ,[OutgoingTrustConsent]
           ,[LegalRequirementsSectionIsCompleted]
           ,[FeatureSectionIsCompleted]
           ,[BenefitsSectionIsCompleted]
           ,[RationaleSectionIsCompleted]
           ,[AssignedUserFullName]
           ,[AssignedUserEmailAddress]
           ,[AssignedUserId]
           ,[CreatedOn])
SELECT     [Id]
           ,[Urn]
           ,[ProjectReference]
           ,[OutgoingTrustUkprn]
           ,[WhoInitiatedTheTransfer]
           ,[RddOrEsfaIntervention]
           ,[RddOrEsfaInterventionDetail]
           ,[TypeOfTransfer]
           ,[OtherTransferTypeDescription]
           ,[TransferFirstDiscussed]
           ,[TargetDateForTransfer]
           ,[HtbDate]
           ,[HasTransferFirstDiscussedDate]
           ,[HasTargetDateForTransfer]
           ,[HasHtbDate]
           ,[ProjectRationale]
           ,[TrustSponsorRationale]
           ,[State]
           ,[Status]
           ,[AnyRisks]
           ,[HighProfileShouldBeConsidered]
           ,[HighProfileFurtherSpecification]
           ,[ComplexLandAndBuildingShouldBeConsidered]
           ,[ComplexLandAndBuildingFurtherSpecification]
           ,[FinanceAndDebtShouldBeConsidered]
           ,[FinanceAndDebtFurtherSpecification]
           ,[OtherRisksShouldBeConsidered]
           ,[EqualitiesImpactAssessmentConsidered]
           ,[OtherRisksFurtherSpecification]
           ,[OtherBenefitValue]
           ,[Author]
           ,[Recommendation]
           ,[IncomingTrustAgreement]
           ,[DiocesanConsent]
           ,[OutgoingTrustConsent]
           ,[LegalRequirementsSectionIsCompleted]
           ,[FeatureSectionIsCompleted]
           ,[BenefitsSectionIsCompleted]
           ,[RationaleSectionIsCompleted]
           ,[AssignedUserFullName]
           ,[AssignedUserEmailAddress]
           ,[AssignedUserId]
           ,[CreatedOn]
  FROM [sdd].[AcademyTransferProjects]

SET IDENTITY_INSERT [academisation].[TransferProject] OFF;

	/*** reseed the table so all new applications can be identified from imported ones ***/
DECLARE @idval int;
SELECT @idval = (SELECT MAX(URN) FROM sdd.[AcademyTransferProjects]) + 1;
SELECT @idval as 'max urn'

DBCC CHECKIDENT ('[academisation].[TransferProject]', RESEED, @idval);

--COMMIT TRAN PortTransferProjectData
ROLLBACK TRAN PortTransferProjectData

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


