﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.String;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy add form a mat project command handler.
	/// </summary>
	public class CyAddFormAMatProjectCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyAddFormAMatProjectCommand, CommandResult>
	{

		/// <summary>
		///     Initializes a new instance of the <see cref="CyAddFormAMatProjectCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAddFormAMatProjectCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public async Task<CommandResult> Handle(CyAddFormAMatProjectCommand request,
			CancellationToken cancellationToken)
		{
			// Define the project name
			const string projectOneName = "Cypress Project One";
			const string projectTwoName = "Cypress Project Two";
			const string applicationFirstName = "FormAMAT";

			// Find the project
			var existingFirstProject = await DbContext.Projects
				.FirstOrDefaultAsync(p => p.Details.SchoolName == projectOneName, cancellationToken);
			var existingSecondProject = await DbContext.Projects
				.FirstOrDefaultAsync(p => p.Details.SchoolName == projectTwoName, cancellationToken);

			// If the project exists, remove it
			if (existingFirstProject != null) DbContext.Projects.Remove(existingFirstProject);
			if (existingSecondProject != null) DbContext.Projects.Remove(existingSecondProject);

			// Find the Application
			var existingApplication = await DbContext.Applications
				.FirstOrDefaultAsync(p => p.Contributors.Single().Details.FirstName == applicationFirstName, cancellationToken);

			// If the Application exists, remove it
			if (existingApplication != null)
			{
				DbContext.Applications.Remove(existingApplication);
			}


			// Create Application
			var contributor = new ContributorDetails(applicationFirstName, "Project", "Cypress@Project.com",
				ContributorRole.ChairOfGovernors, "N/A");
			var createResult = new Domain.ApplicationAggregate.ApplicationFactory().Create(
				ApplicationType.FormAMat, contributor
			);

			if (createResult is CreateSuccessResult<Domain.ApplicationAggregate.Application> successResult)
			{
				var newApplication = successResult.Payload;
				// Set Join Trust Details
				newApplication.SetFormTrustDetails(new FormTrustDetails(DateTime.UtcNow, "Cypress Trust",
					"Cypress Person", "Cy@press.ui", false, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty,
					false, Empty, Empty, Empty));
				// Set Additional Details
				newApplication.SetAdditionalDetails(113537, "Benefits", null, false, null, null, null, "N/A", false,
					null, "N/A", null, Empty, Empty, null, null);
				// Create School List
				var newSchoolList = new List<UpdateSchoolParameter>
				{
					new(0, Empty, Empty, false, Empty, Empty, Empty, Empty, false, Empty, Empty, null, Empty, Empty, null, Empty,
						new SchoolDetails(100608, projectOneName, null, null, null, null),
						new List<KeyValuePair<int, LoanDetails>>(),
						new List<KeyValuePair<int, LeaseDetails>>(), false, false),
					new(0, Empty, Empty, false, Empty, Empty, Empty, Empty, false, Empty, Empty, null, Empty, Empty, null, Empty,
						new SchoolDetails(113537, projectTwoName, null, null, null, null),
						new List<KeyValuePair<int, LoanDetails>>(),
						new List<KeyValuePair<int, LeaseDetails>>(), false, false)
				};
				// Use general update to add schools
				newApplication.Update(ApplicationType.FormAMat, ApplicationStatus.InProgress,
					new List<KeyValuePair<int, ContributorDetails>>() { new(0, contributor) },
					newSchoolList);

				DbContext.Applications.Add(newApplication);
			}
			else
			{
				throw new Exception("Unable to Add application to DbContext");
			}

			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// Add the new project to the Projects DbSet
			var createdApplication = await DbContext.Applications
				.FirstOrDefaultAsync(p => p.Contributors.Single().Details.FirstName == applicationFirstName, cancellationToken);


			// Create two new projects
			var newProjectOneDetails = new ProjectDetails
			{

				SchoolName = projectOneName,
				Urn = 113537,
				ProjectStatus = "Approved with conditions",
				ApplicationReferenceNumber = $"A2B_{createdApplication!.ApplicationId}",
				HeadTeacherBoardDate = new DateTime(2023, 1, 1),
				LocalAuthorityInformationTemplateSentDate = new DateTime(2019, 3, 21),
				LocalAuthorityInformationTemplateReturnedDate = new DateTime(2020, 2, 20),
				RecommendationForProject = "Approve",
				AcademyTypeAndRoute = "Form a Mat",
				ProposedConversionDate = new DateTime(2025, 2, 20),
				ConversionSupportGrantAmount = 25000,
				PublishedAdmissionNumber = "60673",
				ViabilityIssues = "No",
				FinancialDeficit = "No",
				DistanceFromSchoolToTrustHeadquarters = 10,
				RevenueCarryForwardAtEndMarchCurrentYear = -10,
				ProjectedRevenueBalanceAtEndMarchNextYear = -10,
				CapitalCarryForwardAtEndMarchCurrentYear = -10,
				CapitalCarryForwardAtEndMarchNextYear = -10,
				IfdPipelineId = 0,
				YearOneProjectedPupilNumbers = 104,
				YearTwoProjectedPupilNumbers = 239,
				YearThreeProjectedPupilNumbers = 370,
				Region = "West Midlands",
				LocalAuthority = "Coventry"


			};


			// Create two new projects
			var newProjectTwoDetails = new ProjectDetails
			{


				SchoolName = projectTwoName,
				Urn = 100608,
				ProjectStatus = "Approved with conditions",
				ApplicationReferenceNumber = $"A2B_{createdApplication!.ApplicationId}",
				HeadTeacherBoardDate = new DateTime(2023, 1, 1),
				LocalAuthorityInformationTemplateSentDate = new DateTime(2019, 3, 21),
				LocalAuthorityInformationTemplateReturnedDate = new DateTime(2020, 2, 20),
				RecommendationForProject = "Approve",
				AcademyTypeAndRoute = "Form a Mat",
				ProposedConversionDate = new DateTime(2025, 2, 20),
				ConversionSupportGrantAmount = 25000,
				PublishedAdmissionNumber = "60673",
				ViabilityIssues = "No",
				FinancialDeficit = "No",
				DistanceFromSchoolToTrustHeadquarters = 10,
				RevenueCarryForwardAtEndMarchCurrentYear = -10,
				ProjectedRevenueBalanceAtEndMarchNextYear = -10,
				CapitalCarryForwardAtEndMarchCurrentYear = -10,
				CapitalCarryForwardAtEndMarchNextYear = -10,
				IfdPipelineId = 0,
				YearOneProjectedPupilNumbers = 104,
				YearTwoProjectedPupilNumbers = 239,
				YearThreeProjectedPupilNumbers = 370,
				Region = "West Midlands",
				LocalAuthority = "Coventry"
			};

			DbContext.Projects.Add(new Project(3, newProjectTwoDetails));
			DbContext.Projects.Add(new Project(4, newProjectTwoDetails));

			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// If we reach this point without exceptions, the command was successful
			return new CommandSuccessResult();
		}
	}
}
