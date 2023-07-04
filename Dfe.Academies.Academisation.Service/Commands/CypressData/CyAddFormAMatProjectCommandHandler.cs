using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
		public async Task<CommandResult> Handle(CyAddFormAMatProjectCommand request, CancellationToken cancellationToken)
		{
			// Define the project name
			const string projectOneName = "Cypress Project One";
			const string projectTwoName = "Cypress Project Two";

			// Find the project
			var existingFirstProject = await DbContext.Projects
				.FirstOrDefaultAsync(p => p.SchoolName == projectOneName, cancellationToken)
			var existingSecondProject = await DbContext.Projects
				.FirstOrDefaultAsync(p => p.SchoolName == projectTwoName, cancellationToken);

			// If the project exists, remove it
			if (existingFirstProject != null) DbContext.Projects.Remove(existingFirstProject);
			if (existingSecondProject != null) DbContext.Projects.Remove(existingSecondProject);
			

			// Create two new projects
			var newProjectOne = new ProjectState
			{
				SchoolName = projectOneName,
				Urn = 139292,
				ProjectStatus = "Approved with conditions",
				HeadTeacherBoardDate = new DateTime(2023, 1, 1),
				LocalAuthorityInformationTemplateSentDate = new DateTime(2019, 3, 21),
				LocalAuthorityInformationTemplateReturnedDate = new DateTime(2020, 2, 20),
				RecommendationForProject = "Approve",
				AcademyOrderRequired = "Yes",
				AcademyTypeAndRoute = "FormAMat",
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
				CreatedOn = DateTime.Now,
				Region = "West Midlands",
				LocalAuthority = "Coventry"
			};
			// Create two new projects
			var newProjectTwo = new ProjectState
			{
				SchoolName = projectTwoName,
				Urn = 139292,
				ProjectStatus = "Approved with conditions",
				HeadTeacherBoardDate = new DateTime(2023, 1, 1),
				LocalAuthorityInformationTemplateSentDate = new DateTime(2019, 3, 21),
				LocalAuthorityInformationTemplateReturnedDate = new DateTime(2020, 2, 20),
				RecommendationForProject = "Approve",
				AcademyOrderRequired = "Yes",
				AcademyTypeAndRoute = "FormAMat",
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
				CreatedOn = DateTime.Now,
				Region = "West Midlands",
				LocalAuthority = "Coventry"
			};

			// Add the new project to the Projects DbSet
			DbContext.Projects.Add(newProjectOne);
			DbContext.Projects.Add(newProjectTwo);

			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// If we reach this point without exceptions, the command was successful
			return new CommandSuccessResult();
		}
	}
}
