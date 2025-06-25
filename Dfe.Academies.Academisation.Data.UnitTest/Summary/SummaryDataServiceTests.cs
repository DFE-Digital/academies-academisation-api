using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.Summary;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.Summary
{
	public class SummaryDataServiceTests
	{
		private readonly Fixture _fixture = new();

		private readonly SummaryDataService _subject;
		private readonly AcademisationContext _context;
		private readonly IMediator _mediator;

		private readonly string _outgoingTrustUkprn = "12345678";
		private readonly string _outgoingTrusName = "_outgoingTrusName";
		private static readonly string _incomingTrustUkprn = "23456789";
		private static readonly string _incomingTrustName = "_incomingTrustName";

		private readonly List<TransferringAcademy> _academies =
		[
			new(_incomingTrustUkprn, _incomingTrustName, "academy1", "region", "local authority"),
			new(_incomingTrustUkprn, _incomingTrustName, "academy2", "region", "local authority")
		];


		public SummaryDataServiceTests()
		{
			_mediator = new Mock<IMediator>().Object;
			_context = new TestProjectContext(_mediator).CreateContext();
			_subject = new SummaryDataService(_context);

			_fixture.Customize<ProjectDetails>(
				composer => composer.With(x => x.ProjectStatus, "Converter Pre-AO (C)")
			);
		}


		[Fact]
		public async Task ProjectConversionExists___GetProjectSummary()
		{
			const string testEmail = "a@b.com";
			
			// arrange
			var projectDetails = _fixture.Create<ProjectDetails>();

			projectDetails.AssignedUser = new User(Guid.NewGuid(), "Test User", testEmail);

			var newProject = new Project(0, projectDetails);

			_context.Projects.Add(newProject);
			await _context.SaveChangesAsync();

			// act
			var result = (await _subject.GetProjectSummariesByAssignedEmail(testEmail, true, true, true)).ToArray();

			// assert
			Assert.NotNull(result);
			
			Assert.Single(result);
			Assert.Equal(newProject.Id, result[0].Id);
			Assert.Equal(testEmail, result[0].ConversionsSummary?.AssignedUserEmailAddress);
		}


		[Fact]
		public async Task ProjectTransferExists___GetProjectSummary()
		{
			const string testEmail = "a@b.com";

			// arrange
			var newProject =
				TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, false, DateTime.Now);
			newProject.AssignUser(Guid.NewGuid(), testEmail, "Test User");

			_context.TransferProjects.Add(newProject);

			await _context.SaveChangesAsync();

			// act
			var result = (await _subject.GetProjectSummariesByAssignedEmail(testEmail, true, true, true)).ToArray();

			// assert
			Assert.NotNull(result);

			Assert.Single(result);
			Assert.Equal(newProject.Id, result[0].Id);
			Assert.Equal(testEmail, result[0].TransfersSummary?.AssignedUserEmailAddress);
		}

		[Fact]
		public async Task ProjectBothExists___GetProjectSummary()
		{
			const string testEmail = "a@b.com";

			// arrange

			var conversionProjectDetails = _fixture.Create<ProjectDetails>();

			conversionProjectDetails.AssignedUser = new User(Guid.NewGuid(), "Test User", testEmail);

			var newProject = new Project(0, conversionProjectDetails);

			_context.Projects.Add(newProject);

			var newTransferProject =
				TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, false, DateTime.Now);
			newTransferProject.AssignUser(Guid.NewGuid(), testEmail, "Test User");

			_context.TransferProjects.Add(newTransferProject);
			await _context.SaveChangesAsync();

			// act
			var result = (await _subject.GetProjectSummariesByAssignedEmail(testEmail, true, true, true)).ToArray();

			// assert
			Assert.NotNull(result);

			Assert.Equal(2, result.Length);
			Assert.Equal(newProject.Id, result[0].Id);
			Assert.Equal(testEmail, result[0].ConversionsSummary?.AssignedUserEmailAddress);

			Assert.Equal(newTransferProject.Id, result[1].Id);
			Assert.Equal(testEmail, result[1].TransfersSummary?.AssignedUserEmailAddress);
		}
	}
}
