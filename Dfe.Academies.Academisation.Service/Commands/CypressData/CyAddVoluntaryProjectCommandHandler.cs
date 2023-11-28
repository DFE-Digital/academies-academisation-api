using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.String;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy voluntary project command handler.
	/// </summary>
	public class CyAddVoluntaryProjectCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyAddVoluntaryProjectCommand, CommandResult>
	{

		/// <summary>
		///     Initializes a new instance of the <see cref="CyAddVoluntaryProjectCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAddVoluntaryProjectCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public async Task<CommandResult> Handle(CyAddVoluntaryProjectCommand request, CancellationToken cancellationToken)
		{
			// Define the project name
			const string projectName = "Voluntary Cypress Project";
			const string applicationFirstName = "Voluntary";

			// Find the project
			var existingProject = await DbContext.Projects
				.FirstOrDefaultAsync(p => p.Details.SchoolName == projectName, cancellationToken);

			// If the project exists, remove it
			if (existingProject != null)
			{
				DbContext.Projects.Remove(existingProject);
			}

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
				ApplicationType.JoinAMat, contributor
				);

			if (createResult is CreateSuccessResult<Domain.ApplicationAggregate.Application> successResult)
			{
				var newApplication = successResult.Payload;
				// Set Join Trust Details
				newApplication.SetJoinTrustDetails(10059766, "Cypress Trust", Empty, ChangesToTrust.No, null,
					false, null);
				// Set Additional Details
				newApplication.SetAdditionalDetails(113537, "Benefits", null, false, null, null, null, "N/A", false,
					null, "N/A", null, Empty, Empty, null, null);
				// Create School List
				var newSchoolList = new List<UpdateSchoolParameter>
				{
					new(0, Empty, Empty, false, Empty, Empty, Empty, Empty, false, Empty, Empty, null, Empty, Empty, null, Empty,
						new SchoolDetails(113537, "Plymstock School", null, null, null, null),
						new List<KeyValuePair<int, LoanDetails>>(),
						new List<KeyValuePair<int, LeaseDetails>>(), false, false)
				};
				// Use general update to add schools
				newApplication.Update(newApplication.ApplicationType, ApplicationStatus.InProgress,
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


			// Add the new project to the Projects DbSet with applicationId
			var createdApplication = await DbContext.Applications
				.FirstOrDefaultAsync(p => p.Contributors.Single().Details.FirstName == applicationFirstName, cancellationToken);


			// Create a new project
			var newProjectDetails = new ProjectDetails
			{
				SchoolName = projectName,
				Urn = 139292,
				ApplicationReferenceNumber = $"A2B_{createdApplication!.ApplicationId}",
				ProjectStatus = "Approved with conditions",
				ApplicationReceivedDate = new DateTime(2022, 3, 17),
				HeadTeacherBoardDate = new DateTime(2023, 1, 1),
				LocalAuthorityInformationTemplateSentDate = new DateTime(2019, 3, 21),
				LocalAuthorityInformationTemplateReturnedDate = new DateTime(2020, 2, 20),
				RecommendationForProject = "Approve",
				AcademyOrderRequired = "Yes",
				AcademyTypeAndRoute = "Converter",
				ProposedAcademyOpeningDate = new DateTime(2025, 2, 20),
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
				LocalAuthority = "Coventry",
			};

			
			DbContext.Projects.Add(new Project(2,newProjectDetails));

			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// If we reach this point without exceptions, the command was successful
			return new CommandSuccessResult();
		}
	}
}
