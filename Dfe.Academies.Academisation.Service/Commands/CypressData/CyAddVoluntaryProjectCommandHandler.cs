using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using ApplicationAggregate = Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using CoreApplicationAggregate = Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy voluntary project command handler.
	/// </summary>
	public class CyAddVoluntaryProjectCommandHandler : CypressDataBaseCommandHandlerAbstractBase,
		IRequestHandler<CyAddVoluntaryProjectCommand, CommandResult>
	{

		private readonly IApplicationFactory _domainFactory;

		/// <summary>
		///     Initializes a new instance of the <see cref="CyAddVoluntaryProjectCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAddVoluntaryProjectCommandHandler(IApplicationFactory domainFactory, AcademisationContext dbContext) : base(dbContext)
		{
			_domainFactory = domainFactory;
		}

		/// <summary>
		///     Handles the.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public async Task<CommandResult> Handle(CyAddVoluntaryProjectCommand request, CancellationToken cancellationToken)
		{

			 var contributor = new CoreApplicationAggregate.ContributorDetails
			(
            "FirstName",
            "LastName",
            "email@address.com", 
             CoreApplicationAggregate.ContributorRole.ChairOfGovernors, 
             null
			);

			var result = _domainFactory.Create(CoreApplicationAggregate.ApplicationType.JoinAMat, contributor);

			if (result is not CreateSuccessResult<Domain.ApplicationAggregate.Application> domainSuccessResult)
			{
			    throw new NotImplementedException("Other CreateResult types not expected when creating cypress application data");
		     }

		    // Find the project
			var existingApplication = await DbContext.Applications 
				.FirstOrDefaultAsync(a => a.EntityId == domainSuccessResult.Payload.EntityId, cancellationToken);

				// If the project exists, remove it
			if (existingApplication != null)
			{
				DbContext.Applications.Remove(existingApplication);
			}

			
			// Define the project name
			const string projectName = "Cypress Project";

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
				ApplicationReferenceNumber = "A2B_1357",
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
				CreatedOn = DateTime.Now,
				Region = "West Midlands",
				LocalAuthority = "Coventry"
			};

			// Add the new application to the applications DbSet
			DbContext.Applications.Add(domainSuccessResult.Payload);

			// Add the new project to the Projects DbSet
			DbContext.Projects.Add(newProject);

			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// If we reach this point without exceptions, the command was successful
			return new CommandSuccessResult();
		}
	}
}
