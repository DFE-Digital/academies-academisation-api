﻿using Bogus;
using Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest;

public class ConversionApplicationStateTests
{
	private readonly Faker _faker = new();

	[Fact]
	public void MapFromDomain___ConversionApplicationStateReturned()
	{
		//Arrange
		const ApplicationType expectedApplicationType = ApplicationType.FormAMat;

		ContributorDetails initialContributorDetails = new(
			FirstName: _faker.Name.FirstName(),
			LastName: _faker.Name.LastName(),
			EmailAddress: _faker.Internet.Email(),
			ContributorRole.ChairOfGovernors,
			OtherRoleName: null
		);

		var mockConversionApplication = new Mock<IApplication>();
		var mockContributor = new Mock<IContributor>();

		mockContributor.SetupGet(x => x.Details).Returns(initialContributorDetails);
		mockConversionApplication.SetupGet(x => x.ApplicationType).Returns(expectedApplicationType);
		mockConversionApplication.SetupGet(x => x.Contributors).Returns(new List<IContributor>(new[] { mockContributor.Object }));
		mockConversionApplication.SetupGet(x => x.Schools).Returns(new List<ISchool>());

		var expected = new ConversionApplicationState
		{
			ApplicationType = expectedApplicationType,
			Contributors = new() {
				new()
				{
					FirstName = initialContributorDetails.FirstName,
					LastName = initialContributorDetails.LastName,
					EmailAddress = initialContributorDetails.EmailAddress,
					Role = initialContributorDetails.Role,
					OtherRoleName = initialContributorDetails.OtherRoleName
				}
			}
		};
		
		//Act
		var result = ConversionApplicationState.MapFromDomain(mockConversionApplication.Object);

		//Assert
		Assert.Multiple(
			() => Assert.IsType<ConversionApplicationState>(result),
			() => Assert.Equivalent(expected, result)
		);
	}
}
