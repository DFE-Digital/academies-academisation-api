using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using FakerDotNet;

namespace Dfe.Academies.Academisation.Seed;

public static class SeedProject
{
	public static void CreateProject(AcademisationContext? academisationContext)
	{
		var fixture = new Fixture();
		using var dbContextTransaction = academisationContext?.Database.BeginTransaction();
		try
		{
			// Create and add project
			Project newAcademyConversionProject = NewAcademyConversionProject();
			var newProjectState = ProjectState.MapFromDomain(newAcademyConversionProject);

			// Clear Id as EF will throw an exception otherwise
			newProjectState.Id = default;

			// Save project
			academisationContext?.Projects.Add(newProjectState);
			academisationContext?.SaveChanges();

			// Create and add project note
			var projectNote = fixture.Create<ProjectNoteState>();
			projectNote.ProjectId = newProjectState.Id;

			// Clear Id as EF will throw an exception otherwise
			projectNote.Id = default;
			academisationContext?.ProjectNotes.Add(projectNote);
			academisationContext?.SaveChanges();

			dbContextTransaction?.Commit();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
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
			};

			var newProject = new Project(Convert.ToInt32(Faker.Number.Number(7)), projectDetails);
			return newProject;
		}
	}
}
