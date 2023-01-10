using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Seed;
using FakerDotNet;

public static class SeedProject
{
	public static void CreateProject(AcademisationContext? academisationContext)
	{
		var fixture = new Fixture();
		using (var dbContextTransaction = academisationContext.Database.BeginTransaction())
		{
			try
			{
				Project newAcademyConversionProject = NewAcademyConversionProject();
				var newProjectState = ProjectState.MapFromDomain(newAcademyConversionProject);
				newProjectState.Id = default;

				academisationContext.Projects.Add(newProjectState);
				academisationContext.SaveChanges();

				// Get created project as the Db will assign an Id
				var project = academisationContext.Projects.First(x => x.Urn == newProjectState.Urn);
				// Add project notes
				var projectNote = fixture.Create<ProjectNoteState>();
				
				newProjectState.Notes.Add(projectNote);
				academisationContext.SaveChanges();

				dbContextTransaction.Commit();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
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
				ProjectStatus = ProjectConsts.Statuses[(int)Faker.Number.Between(0, 5)],
				ApplicationReceivedDate = DateTime.Now.AddMonths(-2),
				HeadTeacherBoardDate = DateTime.Now.AddMonths(-2),
				LocalAuthorityInformationTemplateSentDate = DateTime.Now.AddYears(-2),
				LocalAuthorityInformationTemplateReturnedDate = DateTime.Now.AddYears(-1),
				ProposedAcademyOpeningDate = DateTime.Now.AddMonths(3),
				Region = ProjectConsts.Regions[(int)Faker.Number.Between(0, 9)],
			};

			var newProject = new Project(Convert.ToInt32(Faker.Number.Number(7)), projectDetails);
			return newProject;
		}
	}
}
