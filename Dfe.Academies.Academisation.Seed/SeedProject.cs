using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using FakerDotNet;

namespace Dfe.Academies.Academisation.Seed;

public static class SeedProject
{
	public static async Task CreateProject(AcademisationContext academisationContext, int numberOfProjects)
	{
		var fixture = new Fixture();
		var newProjects = new List<Project>();
		var projectNotes = new List<ProjectNoteState>();
		for (int i = 1; i <= numberOfProjects; i++)
		{
			var newAcademyConversionProject = NewAcademyConversionProject();
			using var dbContextTransaction = await academisationContext.Database.BeginTransactionAsync();
			try
			{
				// Create and add project
				var newProject = new Project(default, new ProjectDetails());
				newProjects.Add(newProject);

				// Batch save for every 80 project (If over 80)
				if (newProjects.Count % 80 == 0 || numberOfProjects < 80)
				{
					academisationContext.Projects.AddRange(newProjects);
					await academisationContext.SaveChangesAsync();

					foreach (var projectState in newProjects)
					{
						var newProjectNote = fixture.Create<ProjectNoteState>();
						newProjectNote.Id = default;
						newProjectNote.ProjectId = projectState.Id;
						newProjectNote.Date = DateTime.Now;
						projectNotes.Add(newProjectNote);
					}

					academisationContext.ProjectNotes.AddRange(projectNotes);
					await academisationContext.SaveChangesAsync();
					newProjects.Clear();
					projectNotes.Clear();
				}

				await dbContextTransaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await dbContextTransaction.RollbackAsync();
				// better logging and error handling strategy
				Console.WriteLine(ex.ToString());
				throw;
			}
		}
	}

	static Project NewAcademyConversionProject()
	{
		// Initialing details, would use AutoFixture but these details are init only
		// So we could do it on init but the School name for example would be unintelligible 
		var projectDetails = new ProjectDetails()
		{
			Urn = (int)Faker.Number.Between(1, 100000),
			SchoolName = Faker.Ancient.God() + " " + Faker.Company.Profession(),
			ApplicationReferenceNumber = $"A2B_{Faker.Number.Between(1, 100000)}",
			ProjectStatus = ProjectConsts.Statuses[(int)Faker.Number.Between(0, 7)],
			ApplicationReceivedDate = DateTime.Now.AddMonths(-2),
			HeadTeacherBoardDate = DateTime.Now.AddMonths(-2),
			LocalAuthorityInformationTemplateSentDate = DateTime.Now.AddYears(-2),
			LocalAuthorityInformationTemplateReturnedDate = DateTime.Now.AddYears(-1),
			ProposedAcademyOpeningDate = DateTime.Now.AddMonths(3),
			Region = ProjectConsts.Regions[(int)Faker.Number.Between(0, 9)],
			LocalAuthority = Faker.Address.State(),
			NameOfTrust = Faker.Company.Name()
		};

		var newProject = new Project(Convert.ToInt32(Faker.Number.Number(7)), projectDetails);
		return newProject;
	}
}


