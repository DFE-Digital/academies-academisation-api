using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy sponsored project command handler.
	/// </summary>
	public class CyAddSponsoredProjectCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyAddSponsoredProjectCommand, CommandResult>
	{

		/// <summary>
		///     Initializes a new instance of the <see cref="CyAddSponsoredProjectCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAddSponsoredProjectCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public async Task<CommandResult> Handle(CyAddSponsoredProjectCommand request, CancellationToken cancellationToken)
		{
			// Define the project name
			const string projectName = "Sponsored Cypress Project";

			// Find the project
			var existingProject = await DbContext.Projects
				.FirstOrDefaultAsync(p => p.SchoolName == projectName, cancellationToken);

			// If the project exists, remove it
			if (existingProject != null)
			{
				DbContext.Projects.Remove(existingProject);
			}

			// Create a new project
			var newProject = new ProjectState
			{
				SchoolName = projectName,
				Urn = 139292,
				ProjectStatus = "Approved with conditions",
				HeadTeacherBoardDate = new DateTime(2023, 1, 1),
				LocalAuthorityInformationTemplateSentDate = new DateTime(2019, 3, 21),
				LocalAuthorityInformationTemplateReturnedDate = new DateTime(2020, 2, 20),
				RecommendationForProject = "Approve",
				AcademyOrderRequired = "Yes",
				AcademyTypeAndRoute = "Sponsored",
				ProposedAcademyOpeningDate = new DateTime(2025, 2, 20),
				ConversionSupportGrantAmount = 25000,
				PublishedAdmissionNumber = "60673",
				PartOfPfiScheme = "No",
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
			DbContext.Projects.Add(newProject);

			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// If we reach this point without exceptions, the command was successful
			return new CommandSuccessResult();
		}
	}
}
