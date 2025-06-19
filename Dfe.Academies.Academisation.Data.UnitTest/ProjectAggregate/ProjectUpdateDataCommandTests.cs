using System;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ProjectAggregate
{
	public class ProjectUpdateDataCommandTests
	{
		private readonly AcademisationContext _context;
		private readonly Fixture _fixture = new();
		private readonly IMediator _mediator;
		private readonly ProjectUpdateDataCommand _subject;

		public ProjectUpdateDataCommandTests()
		{
			_context = new TestProjectContext(_mediator).CreateContext();
			_subject = new ProjectUpdateDataCommand(_context);
		}

		[Fact]
		public async Task ProjectExists___ProjectUpdated()
		{
			_context.ChangeTracker.AutoDetectChangesEnabled = false;
			// arrange
			Project? existingProject = _fixture.Build<Project>()
				.Create();

			await _context.Projects.AddAsync(existingProject);

			await _context.SaveChangesAsync();

			IProject updatedProject = await _context.Projects.SingleAsync(p => p.Id == existingProject.Id);

			ProjectDetails projectDetails = _fixture.Build<ProjectDetails>()
				.With(p => p.Urn, updatedProject.Details.Urn)
				.With(x => x.ExternalApplicationFormSaved, true)
				.With(x => x.ExternalApplicationFormUrl, "test//url")
				.With(x => x.ApplicationReceivedDate, new DateTime(2024, 12, 20, 23, 59, 58, DateTimeKind.Utc)) // before support grant deadline
				.Create();


			updatedProject.Update(projectDetails);


			await _context.Projects.LoadAsync();

			// act
			await _subject.Execute(updatedProject);

			// assert
			Assert.True(projectDetails.Equals(updatedProject.Details));
		}

		[Fact]
		public async Task UpdateCommand_When_Application_Received_After_SupportGrant_Deadline_Should_Have_No_SuportGrant_Data_Recorded()
		{
			// arrange
			_context.ChangeTracker.AutoDetectChangesEnabled = false;
			
			Project? existingProject = _fixture.Build<Project>()
				.Create();

			await _context.Projects.AddAsync(existingProject);

			await _context.SaveChangesAsync();

			IProject updatedProject = await _context.Projects.SingleAsync(p => p.Id == existingProject.Id);

			ProjectDetails projectDetails = _fixture.Build<ProjectDetails>()
				.With(p => p.Urn, updatedProject.Details.Urn)
				.With(x => x.ExternalApplicationFormSaved, true)
				.With(x => x.ExternalApplicationFormUrl, "test//url")
				.With(x => x.ApplicationReceivedDate, new DateTime(2024, 12, 21, 0, 0, 0, DateTimeKind.Utc)) // after support grant deadline
				.Create();

			updatedProject.Update(projectDetails);

			await _context.Projects.LoadAsync();

			// act
			await _subject.Execute(updatedProject);

			// assert
			Assert.Null(updatedProject.Details.ConversionSupportGrantAmount);
			Assert.Null(updatedProject.Details.ConversionSupportGrantChangeReason);
			Assert.Null(updatedProject.Details.ConversionSupportGrantType);
			Assert.Null(updatedProject.Details.ConversionSupportGrantEnvironmentalImprovementGrant);
			Assert.Null(updatedProject.Details.ConversionSupportGrantAmountChanged);
			Assert.Null(updatedProject.Details.ConversionSupportGrantNumberOfSites);
		}
	}
}
