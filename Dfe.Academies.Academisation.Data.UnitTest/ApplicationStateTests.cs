using System.Collections.Generic;
using Bogus;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest;

public class ApplicationStateTests
{
	private readonly Faker _faker = new();

	[Fact]
	public void MapFromDomain___ApplicationStateReturned()
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

		var mockApplication = new Mock<IApplication>();
		var mockContributor = new Mock<IContributor>();

		mockContributor.SetupGet(x => x.Details).Returns(initialContributorDetails);
		mockApplication.SetupGet(x => x.ApplicationType).Returns(expectedApplicationType);
		mockApplication.SetupGet(x => x.Contributors).Returns(new List<IContributor>(new[] { mockContributor.Object }));
		mockApplication.SetupGet(x => x.Schools).Returns(new List<ISchool>());

		var expected = new ApplicationState
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
		var result = ApplicationState.MapFromDomain(mockApplication.Object);

		//Assert
		Assert.Multiple(
			() => Assert.IsType<ApplicationState>(result),
			() => Assert.Equivalent(expected, result)
		);
	}
}
