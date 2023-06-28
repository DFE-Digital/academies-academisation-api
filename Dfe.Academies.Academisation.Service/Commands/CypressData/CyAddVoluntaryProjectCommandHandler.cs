using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
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
		private readonly IProjectFactory _projectFactory;
		/// <summary>
		///     Initializes a new instance of the <see cref="CyAddVoluntaryProjectCommandHandler" /> class.
		/// </summary>
		/// <param name="dbContext">The db context.</param>
		public CyAddVoluntaryProjectCommandHandler(AcademisationContext dbContext, IProjectFactory projectFactory) : base(dbContext)
		{
			_projectFactory = projectFactory;
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
			var newProject = new ProjectState { SchoolName = projectName };

			// Add the new project to the Projects DbSet
			DbContext.Projects.Add(newProject);

			// Save changes to the database
			await DbContext.SaveChangesAsync(cancellationToken);

			// If we reach this point without exceptions, the command was successful
			return new CommandSuccessResult();
		}
	}
}
