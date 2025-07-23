using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using MediatR;
using Moq;

namespace Dfe.Academies.Academisation.SubcutaneousTest.Summary
{
	public class SummaryQueryServiceTests
	{
		private readonly Fixture _fixture = new();

		private readonly SummaryQueryService _subject;
		private readonly Mock<IConversionProjectRepository> _conversionProjectRepositoryMock;
		private readonly Mock<ITransferProjectRepository> _transferProjectRepositoryMock;
		private readonly Mock<IFormAMatProjectRepository> _formAMatProjectRepositoryMock;
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


		public SummaryQueryServiceTests()
		{
			_transferProjectRepositoryMock = new Mock<ITransferProjectRepository>();
			_conversionProjectRepositoryMock = new Mock<IConversionProjectRepository>();
			_formAMatProjectRepositoryMock = new Mock<IFormAMatProjectRepository>();

			_mediator = new Mock<IMediator>().Object;
			_subject = new SummaryQueryService(_conversionProjectRepositoryMock.Object,
				_transferProjectRepositoryMock.Object,
				_formAMatProjectRepositoryMock.Object);

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

			_conversionProjectRepositoryMock
				.Setup(x => x.GetConversionProjectsByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync([newProject]);

			// act
			var result = (await _subject.GetProjectSummariesByAssignedEmail(testEmail, true, true, true, CancellationToken.None)).ToArray();

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

			_transferProjectRepositoryMock.Setup(x => x.GetByDeliveryOfficerEmail(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync([newProject]);

			// act
			var result = (await _subject.GetProjectSummariesByAssignedEmail(testEmail, true, true, true, CancellationToken.None)).ToArray();

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

			_conversionProjectRepositoryMock
				.Setup(x => x.GetConversionProjectsByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync([newProject]);
			
			var newTransferProject =
				TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, false, DateTime.Now);
			newTransferProject.AssignUser(Guid.NewGuid(), testEmail, "Test User");
			
			_transferProjectRepositoryMock.Setup(x => x.GetByDeliveryOfficerEmail(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync([newTransferProject]);

			// act
			var result = (await _subject.GetProjectSummariesByAssignedEmail(testEmail, true, true, true, CancellationToken.None)).ToArray();

			// assert
			Assert.NotNull(result);

			Assert.Equal(2, result.Length);
			Assert.Equal(newProject.Id, result[0].Id);
			Assert.Equal(testEmail, result[0].ConversionsSummary?.AssignedUserEmailAddress);

			Assert.Equal(newTransferProject.Id, result[1].Id);
			Assert.Equal(testEmail, result[1].TransfersSummary?.AssignedUserEmailAddress);
		}


		[Fact]
		public async Task ProjectFormAMatExists___GetProjectSummary()
		{
			const string testEmail = "a@b.com";
			const string proposedTrustName = "Form A Mat";

			// arrange

			var conversionProjectDetails = _fixture.Create<ProjectDetails>();

			conversionProjectDetails.AssignedUser = new User(Guid.NewGuid(), "Test User", testEmail);

			var newProject = new Project(0, conversionProjectDetails);

			_conversionProjectRepositoryMock
				.Setup(x => x.GetConversionProjectsByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync([newProject]);
			
			var newFormAMatProject =
				FormAMatProject.Create(proposedTrustName, "MAT123", DateTime.Now);
			newFormAMatProject.Id = 123;
			_formAMatProjectRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync([newFormAMatProject]);

			// act
			var result = (await _subject.GetProjectSummariesByAssignedEmail(testEmail, true, true, true, CancellationToken.None)).ToArray();

			// assert
			Assert.NotNull(result);

			Assert.Equal(2, result.Length);
			Assert.Equal(newProject.Id, result[0].Id);
			Assert.Equal(testEmail, result[0].ConversionsSummary?.AssignedUserEmailAddress);

			Assert.Equal(newFormAMatProject.Id, result[1].Id);
			Assert.Equal(proposedTrustName, result[1].FormAMatSummary?.ProposedTrustName);
		}
	}
}
