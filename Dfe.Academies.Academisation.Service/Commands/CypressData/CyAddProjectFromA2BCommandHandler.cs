using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Migrations;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Dfe.Academies.Academisation.IService.ServiceModels.Academies.Establishment;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy project from A2B command handler.
	/// </summary>
	public class CyAddProjectFromA2BCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyAddProjectFromA2BCommand, CommandResult>
	{

		/// <summary>
		///     Initializes a new instance of the <see cref="CyAddProjectFromA2BCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAddProjectFromA2BCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public async Task<CommandResult> Handle(CyAddProjectFromA2BCommand request, CancellationToken cancellationToken)
		{
			// Define the project name
			string projectName =  "Fahads Cypress test";

			// Find the project
			var existingProject = await DbContext.Projects
				.FirstOrDefaultAsync(p => p.Details.SchoolName == projectName, cancellationToken);

			// If the project exists, remove it
			if (existingProject != null)
			{
				DbContext.Projects.Remove(existingProject);
			}

			var now = DateTime.Now;


			// Create a new project
			var projectDetails = new ProjectDetails
			{
				Urn = 113537,
				ApplicationReferenceNumber = "A2B_124088",
				SchoolName = projectName,
				SchoolPhase = "Secondary",
				ProjectStatus = "Converter Pre-AO (C)",
				ApplicationReceivedDate = now,
				HeadTeacherBoardDate = now,
				LocalAuthorityInformationTemplateSentDate = now,
				LocalAuthorityInformationTemplateReturnedDate = now,
				LocalAuthorityInformationTemplateComments = "TEST",
				LocalAuthorityInformationTemplateLink = "TEST",
				LocalAuthorityInformationTemplateSectionComplete = false,
				PreviousHeadTeacherBoardDateQuestion = "TEST",
				PreviousHeadTeacherBoardDate = now,
				TrustReferenceNumber = "TRUST_REFERENCE",
				NameOfTrust = "Fahad's Trust",
				AcademyTypeAndRoute = "Converter",
				ProposedConversionDate = now,
				SchoolAndTrustInformationSectionComplete = false,
				ConversionSupportGrantAmount = 25000,		
				ConversionSupportGrantChangeReason = "",
				PublishedAdmissionNumber = "210.00", 
				PartOfPfiScheme = "Yes",
				ViabilityIssues = "No",
				FinancialDeficit = "No",
				DistanceFromSchoolToTrustHeadquarters = 10,
				DistanceFromSchoolToTrustHeadquartersAdditionalInformation = "testing",
				MemberOfParliamentNameAndParty = "",
				SchoolOverviewSectionComplete = false,
				RationaleForTrust = "Conversion key details  Reasons for joining  Why does the school want to join this trust in particular?",
				RevenueCarryForwardAtEndMarchCurrentYear = -3000,
				ProjectedRevenueBalanceAtEndMarchNextYear = -5000,
				CapitalCarryForwardAtEndMarchCurrentYear = -4000,
				CapitalCarryForwardAtEndMarchNextYear = -4000,
				SchoolBudgetInformationSectionComplete = false,
				IfdPipelineId = 0,
				YearOneProjectedPupilNumbers = 101,
				YearTwoProjectedPupilNumbers = 102,
				YearThreeProjectedPupilNumbers = 103,
				SchoolType = "Foundation school",
				Consultation = YesNoNotApplicable.NotApplicable,
				FoundationConsent = YesNoNotApplicable.NotApplicable,
				GoverningBodyResolution = YesNoNotApplicable.NotApplicable,
				LegalRequirementsSectionComplete = false,
				EndOfCurrentFinancialYear =  now,
				EndOfNextFinancialYear = now,
				Region = "North West",
				LocalAuthority = "Stockport",
				AnnexBFormUrl = "",
				ExternalApplicationFormUrl = ""
			};

			var sql = @"
						INSERT INTO [academisation].[Project] (
							[Urn], 
							[SchoolName], 
							[ApplicationReferenceNumber], 
							[SchoolPhase], 
							[ProjectStatus], 
							[ApplicationReceivedDate], 
							[HeadTeacherBoardDate], 
							[LocalAuthorityInformationTemplateSentDate], 
							[LocalAuthorityInformationTemplateReturnedDate], 
							[LocalAuthorityInformationTemplateComments], 
							[LocalAuthorityInformationTemplateLink], 
							[LocalAuthorityInformationTemplateSectionComplete], 
							[PreviousHeadTeacherBoardDateQuestion], 
							[PreviousHeadTeacherBoardDate], 
							[TrustReferenceNumber], 
							[NameOfTrust], 
							[AcademyTypeAndRoute], 
							[ProposedAcademyOpeningDate], 
							[SchoolAndTrustInformationSectionComplete], 
							[ConversionSupportGrantAmount], 
							[ConversionSupportGrantChangeReason], 
							[PublishedAdmissionNumber], 
							[PartOfPfiScheme], 
							[ViabilityIssues], 
							[FinancialDeficit], 
							[DistanceFromSchoolToTrustHeadquarters], 
							[DistanceFromSchoolToTrustHeadquartersAdditionalInformation], 
							[MemberOfParliamentNameAndParty], 
							[SchoolOverviewSectionComplete], 
							[RationaleForTrust], 
							[RevenueCarryForwardAtEndMarchCurrentYear], 
							[ProjectedRevenueBalanceAtEndMarchNextYear], 
							[CapitalCarryForwardAtEndMarchCurrentYear], 
							[CapitalCarryForwardAtEndMarchNextYear], 
							[SchoolBudgetInformationSectionComplete], 
							[IfdPipelineId], 
							[YearOneProjectedPupilNumbers], 
							[YearTwoProjectedPupilNumbers], 
							[YearThreeProjectedPupilNumbers], 
							[SchoolType], 
							[CreatedOn], 
							[LastModifiedOn], 
							[Consultation], 
							[FoundationConsent], 
							[GoverningBodyResolution], 
							[LegalRequirementsSectionComplete], 
							[AssignedUserEmailAddress], 
							[AssignedUserFullName], 
							[AssignedUserId], 
							[EndOfCurrentFinancialYear], 
							[EndOfNextFinancialYear], 
							[Region], 
							[LocalAuthority], 
							[AnnexBFormUrl], 
							[ExternalApplicationFormUrl]
						) 
						VALUES (
							@Urn, 
							@SchoolName, 
							@ApplicationReferenceNumber, 
							@SchoolPhase, 
							@ProjectStatus, 
							@ApplicationReceivedDate, 
							@HeadTeacherBoardDate, 
							@LocalAuthorityInformationTemplateSentDate, 
							@LocalAuthorityInformationTemplateReturnedDate, 
							@LocalAuthorityInformationTemplateComments, 
							@LocalAuthorityInformationTemplateLink, 
							@LocalAuthorityInformationTemplateSectionComplete, 
							@PreviousHeadTeacherBoardDateQuestion, 
							@PreviousHeadTeacherBoardDate, 
							@TrustReferenceNumber, 
							@NameOfTrust, 
							@AcademyTypeAndRoute, 
							@ProposedAcademyOpeningDate, 
							@SchoolAndTrustInformationSectionComplete, 
							@ConversionSupportGrantAmount, 
							@ConversionSupportGrantChangeReason, 
							@PublishedAdmissionNumber, 
							@PartOfPfiScheme, 
							@ViabilityIssues, 
							@FinancialDeficit, 
							@DistanceFromSchoolToTrustHeadquarters, 
							@DistanceFromSchoolToTrustHeadquartersAdditionalInformation, 
							@MemberOfParliamentNameAndParty, 
							@SchoolOverviewSectionComplete, 
							@RationaleForTrust, 
							@RevenueCarryForwardAtEndMarchCurrentYear, 
							@ProjectedRevenueBalanceAtEndMarchNextYear, 
							@CapitalCarryForwardAtEndMarchCurrentYear, 
							@CapitalCarryForwardAtEndMarchNextYear, 
							@SchoolBudgetInformationSectionComplete, 
							@IfdPipelineId, 
							@YearOneProjectedPupilNumbers, 
							@YearTwoProjectedPupilNumbers, 
							@YearThreeProjectedPupilNumbers, 
							@SchoolType, 
							@CreatedOn, 
							@LastModifiedOn, 
							@Consultation, 
							@FoundationConsent, 
							@GoverningBodyResolution, 
							@LegalRequirementsSectionComplete, 
							@AssignedUserEmailAddress, 
							@AssignedUserFullName, 
							@AssignedUserId, 
							@EndOfCurrentFinancialYear, 
							@EndOfNextFinancialYear, 
							@Region, 
							@LocalAuthority, 
							@AnnexBFormUrl, 
							@ExternalApplicationFormUrl
						)";

			var parameters = new SqlParameter[]
			{
				new SqlParameter("@Urn", projectDetails.Urn),
				new SqlParameter("@SchoolName", projectDetails.SchoolName),
				new SqlParameter("@ApplicationReferenceNumber", projectDetails.ApplicationReferenceNumber),
				new SqlParameter("@SchoolPhase", projectDetails.SchoolPhase),
				new SqlParameter("@ProjectStatus", projectDetails.ProjectStatus),
				new SqlParameter("@ApplicationReceivedDate", projectDetails.ApplicationReceivedDate),
				new SqlParameter("@HeadTeacherBoardDate", projectDetails.HeadTeacherBoardDate),
				new SqlParameter("@LocalAuthorityInformationTemplateSentDate", projectDetails.LocalAuthorityInformationTemplateSentDate),
				new SqlParameter("@LocalAuthorityInformationTemplateReturnedDate", projectDetails.LocalAuthorityInformationTemplateReturnedDate),
				new SqlParameter("@LocalAuthorityInformationTemplateComments", projectDetails.LocalAuthorityInformationTemplateComments),
				new SqlParameter("@LocalAuthorityInformationTemplateLink", projectDetails.LocalAuthorityInformationTemplateLink),
				new SqlParameter("@LocalAuthorityInformationTemplateSectionComplete", projectDetails.LocalAuthorityInformationTemplateSectionComplete),
				new SqlParameter("@PreviousHeadTeacherBoardDateQuestion", projectDetails.PreviousHeadTeacherBoardDateQuestion),
				new SqlParameter("@PreviousHeadTeacherBoardDate", projectDetails.PreviousHeadTeacherBoardDate),
				new SqlParameter("@TrustReferenceNumber", projectDetails.TrustReferenceNumber),
				new SqlParameter("@NameOfTrust", projectDetails.NameOfTrust),
				new SqlParameter("@AcademyTypeAndRoute", projectDetails.AcademyTypeAndRoute),
				new SqlParameter("@ProposedAcademyOpeningDate", projectDetails.ProposedConversionDate),
				new SqlParameter("@SchoolAndTrustInformationSectionComplete", projectDetails.SchoolAndTrustInformationSectionComplete),
				new SqlParameter("@ConversionSupportGrantAmount", projectDetails.ConversionSupportGrantAmount),
				new SqlParameter("@ConversionSupportGrantChangeReason", projectDetails.ConversionSupportGrantChangeReason),
				new SqlParameter("@PublishedAdmissionNumber", projectDetails.PublishedAdmissionNumber),
				new SqlParameter("@PartOfPfiScheme", projectDetails.PartOfPfiScheme),
				new SqlParameter("@ViabilityIssues", projectDetails.ViabilityIssues),
				new SqlParameter("@FinancialDeficit", projectDetails.FinancialDeficit),
				new SqlParameter("@DistanceFromSchoolToTrustHeadquarters", projectDetails.DistanceFromSchoolToTrustHeadquarters),
				new SqlParameter("@DistanceFromSchoolToTrustHeadquartersAdditionalInformation", projectDetails.DistanceFromSchoolToTrustHeadquartersAdditionalInformation),
				new SqlParameter("@MemberOfParliamentNameAndParty", projectDetails.MemberOfParliamentNameAndParty),
				new SqlParameter("@SchoolOverviewSectionComplete", projectDetails.SchoolOverviewSectionComplete),
				new SqlParameter("@RationaleForTrust", projectDetails.RationaleForTrust),
				new SqlParameter("@RevenueCarryForwardAtEndMarchCurrentYear", projectDetails.RevenueCarryForwardAtEndMarchCurrentYear),
				new SqlParameter("@ProjectedRevenueBalanceAtEndMarchNextYear", projectDetails.ProjectedRevenueBalanceAtEndMarchNextYear),
				new SqlParameter("@CapitalCarryForwardAtEndMarchCurrentYear", projectDetails.CapitalCarryForwardAtEndMarchCurrentYear),
				new SqlParameter("@CapitalCarryForwardAtEndMarchNextYear", projectDetails.CapitalCarryForwardAtEndMarchNextYear),
				new SqlParameter("@SchoolBudgetInformationSectionComplete", projectDetails.SchoolBudgetInformationSectionComplete),
				new SqlParameter("@IfdPipelineId", projectDetails.IfdPipelineId),
				new SqlParameter("@YearOneProjectedPupilNumbers", projectDetails.YearOneProjectedPupilNumbers),
				new SqlParameter("@YearTwoProjectedPupilNumbers", projectDetails.YearTwoProjectedPupilNumbers),
				new SqlParameter("@YearThreeProjectedPupilNumbers", projectDetails.YearThreeProjectedPupilNumbers),
				new SqlParameter("@SchoolType", projectDetails.SchoolType),
				new SqlParameter("@CreatedOn", now),
				new SqlParameter("@LastModifiedOn", now),
				new SqlParameter("@Consultation", projectDetails.Consultation),
				new SqlParameter("@FoundationConsent", projectDetails.FoundationConsent),
				new SqlParameter("@GoverningBodyResolution", projectDetails.GoverningBodyResolution),
				new SqlParameter("@LegalRequirementsSectionComplete", projectDetails.LegalRequirementsSectionComplete),
				new SqlParameter("@AssignedUserEmailAddress", "Elijah.Aremu@education.gov.uk"),
				new SqlParameter("@AssignedUserFullName", "Elijah Aremu"),
				new SqlParameter("@AssignedUserId", Guid.NewGuid()),
				new SqlParameter("@EndOfCurrentFinancialYear", projectDetails.EndOfCurrentFinancialYear),
				new SqlParameter("@EndOfNextFinancialYear", projectDetails.EndOfNextFinancialYear),
				new SqlParameter("@Region", projectDetails.Region),
				new SqlParameter("@LocalAuthority", projectDetails.LocalAuthority),
				new SqlParameter("@AnnexBFormUrl", projectDetails.AnnexBFormUrl),
				new SqlParameter("@ExternalApplicationFormUrl", projectDetails.ExternalApplicationFormUrl)
			};

			DbContext.Database.ExecuteSqlRaw(sql, parameters);


			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// If we reach this point without exceptions, the command was successful
			return new CommandSuccessResult();
		}
	}
}
