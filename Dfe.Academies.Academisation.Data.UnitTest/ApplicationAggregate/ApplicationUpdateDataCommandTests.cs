using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ApplicationAggregate;

public class ApplicationUpdateDataCommandTests
{
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();
	private readonly ApplicationGetDataQuery _query;
	private readonly ApplicationUpdateDataCommand _subject;
	private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

	public ApplicationUpdateDataCommandTests()
	{
		_context = new TestApplicationContext().CreateContext();
		_query = new ApplicationGetDataQuery(_context, _mapper.Object);
		_subject = new ApplicationUpdateDataCommand(_context, _mapper.Object);

		_fixture.Customize<LoanState>(composer =>
			composer
				.With(s => s.Id, 0));
	}

	[Fact]
	public async Task WhenRecordDoesNotExist___ThrowsApplicationException()
	{
		// Arrange
		const int applicationId = 4;

		Mock<IApplication> mockApplication = new();

		mockApplication.SetupGet(a => a.ApplicationId).Returns(applicationId);

		mockApplication.SetupGet(d => d.Contributors)
		 	.Returns(new List<IContributor>());

		mockApplication.SetupGet(d => d.Schools)
		 	.Returns(new List<ISchool>());

		//Act & Assert
		await Assert.ThrowsAsync<ApplicationException>(() => _subject.Execute(mockApplication.Object));
	}

	[Fact]
	public async Task RecordAlreadyExists_NoChange___LastModifiedOnUpdated()
	{
		//Arrange
		const int applicationId = 1;

		var existingApplication = await _query.Execute(applicationId);
		Assert.NotNull(existingApplication);

		Mock<IApplication> mockApplication = CloneAsMock(existingApplication);

		//Act
		await _subject.Execute(mockApplication.Object);
		_context.ChangeTracker.Clear();
		var updatedApplication = await _query.Execute(applicationId);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.Multiple(
			() => Assert.Equal(existingApplication.CreatedOn, updatedApplication.CreatedOn),
			() => Assert.NotEqual(existingApplication.LastModifiedOn, updatedApplication.LastModifiedOn)
		);
	}

	[Fact]
	public async Task RecordAlreadyExists_FieldsChanged___FieldChangesPersisted()
	{
		//Arrange
		const int applicationId = 1;

		var existingApplication = await _query.Execute(applicationId);
		Assert.NotNull(existingApplication);

		Mock<IApplication> mockApplication = CloneAsMock(existingApplication);

		mockApplication.SetupGet(a => a.ApplicationStatus).Returns(_fixture.Create<ApplicationStatus>());
		mockApplication.SetupGet(a => a.ApplicationType).Returns(_fixture.Create<ApplicationType>());

		//Act
		await _subject.Execute(mockApplication.Object);
		_context.ChangeTracker.Clear();
		var updatedApplication = await _query.Execute(applicationId);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.Multiple(
			() => Assert.Equal(existingApplication.ApplicationStatus, updatedApplication.ApplicationStatus),
			() => Assert.Equal(existingApplication.ApplicationType, updatedApplication.ApplicationType)
		);
	}

	[Fact]
	public async Task RecordAlreadyExists_ContributorAdded___AddedContributorPersisted()
	{
		//Arrange
		const int applicationId = 1;

		var existingApplication = await _query.Execute(applicationId);
		Assert.NotNull(existingApplication);

		Mock<IApplication> mockApplication = CloneAsMock(existingApplication);

		var addedContributorDetails = _fixture.Create<ContributorDetails>();
		var addedContributor = new Mock<IContributor>();
		addedContributor.SetupGet(c => c.Details).Returns(addedContributorDetails);

		mockApplication.SetupGet(d => d.Contributors)
		 	.Returns(new List<IContributor>(existingApplication.Contributors)
			{
				addedContributor.Object
			});

		//Act
		await _subject.Execute(mockApplication.Object);
		_context.ChangeTracker.Clear();
		var updatedApplication = await _query.Execute(applicationId);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.Contains(addedContributorDetails, updatedApplication.Contributors.Select(c => c.Details));
	}

	[Fact]
	public async Task RecordAlreadyExists_ContributorRemoved___RemovedContributorPersisted()
	{
		//Arrange
		const int applicationId = 1;

		var existingApplication = await _query.Execute(applicationId);
		Assert.NotNull(existingApplication);

		Mock<IApplication> mockApplication = CloneAsMock(existingApplication);

		var mutatedContributorList = new List<IContributor>(existingApplication.Contributors);
		var contributorToRemove = PickRandomElement(mutatedContributorList);
		mutatedContributorList.Remove(contributorToRemove);

		mockApplication.SetupGet(d => d.Contributors)
		 	.Returns(mutatedContributorList);

		//Act
		await _subject.Execute(mockApplication.Object);
		_context.ChangeTracker.Clear();
		var updatedApplication = await _query.Execute(applicationId);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.DoesNotContain(contributorToRemove.Details, updatedApplication.Contributors.Select(c => c.Details));
	}

	[Fact]
	public async Task RecordAlreadyExists_SchoolAdded___AddedSchoolPersisted()
	{
		//Arrange
		const int applicationId = 1;

		var existingApplication = await _query.Execute(applicationId);
		Assert.NotNull(existingApplication);

		Mock<IApplication> mockApplication = CloneAsMock(existingApplication);

		var addedSchoolDetails = _fixture.Create<SchoolDetails>();
		var addedSchool = new Mock<ISchool>();
		addedSchool.SetupGet(c => c.Details).Returns(addedSchoolDetails);

		mockApplication.SetupGet(d => d.Schools)
		 	.Returns(new List<ISchool>(existingApplication.Schools)
			{
				addedSchool.Object
			});

		//Act
		await _subject.Execute(mockApplication.Object);
		_context.ChangeTracker.Clear();
		var updatedApplication = await _query.Execute(applicationId);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.Contains(addedSchoolDetails, updatedApplication.Schools.Select(s => s.Details));
	}

	[Fact]
	public async Task RecordAlreadyExists_SchoolRemoved___RemovedSchoolPersisted()
	{
		//Arrange
		const int applicationId = 1;

		var existingApplication = await _query.Execute(applicationId);
		Assert.NotNull(existingApplication);

		Mock<IApplication> mockApplication = CloneAsMock(existingApplication);

		var mutatedSchoolList = new List<ISchool>(existingApplication.Schools);
		var contributorToRemove = PickRandomElement(mutatedSchoolList);
		mutatedSchoolList.Remove(contributorToRemove);

		mockApplication.SetupGet(d => d.Schools)
		 	.Returns(mutatedSchoolList);

		//Act
		await _subject.Execute(mockApplication.Object);
		_context.ChangeTracker.Clear();
		var updatedApplication = await _query.Execute(applicationId);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.DoesNotContain(contributorToRemove.Details, updatedApplication.Schools.Select(s => s.Details));
	}

	private static Mock<IApplication> CloneAsMock(IApplication existingApplication)
	{
		Mock<IApplication> mockApplication = new();

		mockApplication
			.SetupGet(a => a.ApplicationId)
			.Returns(existingApplication.ApplicationId);

		mockApplication
			.SetupGet(a => a.CreatedOn)
			.Returns(existingApplication.CreatedOn);

		mockApplication
			.SetupGet(a => a.LastModifiedOn)
			.Returns(existingApplication.LastModifiedOn);

		mockApplication.SetupGet(d => d.Contributors)
			 .Returns(existingApplication.Contributors);

		mockApplication.SetupGet(d => d.Schools)
			.Returns(existingApplication.Schools);

		return mockApplication;
	}

	private static T PickRandomElement<T>(IEnumerable<T> enumerable)
	{
		Random random = new();
		var list = enumerable.ToList();
		return list.ElementAt(random.Next(1, list.Count));
	}
}
