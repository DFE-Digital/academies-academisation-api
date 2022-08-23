using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ApplicationAggregate;

public class ApplicationUpdateDataCommandTests
{
	private readonly AcademisationContext _context;

	private readonly Fixture _fixture = new();
	private readonly Mock<IApplication> _mockApplication = new();

	public ApplicationUpdateDataCommandTests()
	{
		_context = new TestApplicationContext().CreateContext();
	}

	[Fact]
	public async Task WhenRecordDoesNotExist___ThrowsApplicationException()
	{
		const int applicationId = 4;

		ApplicationUpdateDataCommand subject = new(_context);

		_mockApplication.SetupGet(a => a.ApplicationId).Returns(applicationId);

		_mockApplication.SetupGet(d => d.Contributors)
		 	.Returns(new List<IContributor>());

		_mockApplication.SetupGet(d => d.Schools)
		 	.Returns(new List<ISchool>());

		//Act & Assert
		await Assert.ThrowsAsync<ApplicationException>(() => subject.Execute(_mockApplication.Object));
	}

	[Fact]
	public async Task WhenRecordAlreadyExists___UpdatesExistingRecord()
	{
		//Arrange
		const int applicationId = 1;

		ApplicationUpdateDataCommand subject = new(_context);

		var existingApplication = await _context.Applications
			.AsNoTracking()
			.SingleAsync(a => a.Id == applicationId);

		_mockApplication
			.SetupGet(a => a.ApplicationId)
			.Returns(applicationId);

		_mockApplication
			.SetupGet(a => a.CreatedOn)
			.Returns(existingApplication.CreatedOn);

		_mockApplication
			.SetupGet(a => a.LastModifiedOn)
			.Returns(existingApplication.LastModifiedOn);

		_mockApplication.SetupGet(d => d.Contributors)
		 	.Returns(new List<IContributor>());

		_mockApplication.SetupGet(d => d.Schools)
		 	.Returns(new List<ISchool>());

		await _context.Applications.LoadAsync();

		//Act
		await subject.Execute(_mockApplication.Object);

		_context.ChangeTracker.Clear();

		var queryResult = await _context.Applications.FindAsync(applicationId);

		//Assert
		Assert.Multiple(
			//() => Assert.NotEqual(existingApplication.ApprovedConditionsSet, result!.ApprovedConditionsSet),
			() => Assert.NotEqual(existingApplication.LastModifiedOn, queryResult!.LastModifiedOn),
			() => Assert.Equal(existingApplication.CreatedOn, queryResult!.CreatedOn)
		);
	}
}
