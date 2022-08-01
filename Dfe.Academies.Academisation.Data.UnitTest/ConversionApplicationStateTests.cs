using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using FluentAssertions;
using Xunit;
using Moq;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.UnitTest;

public class ConversionApplicationStateTests
{
	private readonly Faker _faker;
	private readonly ConversionApplicationFactory _factory;

	public ConversionApplicationStateTests()
	{
		_faker = new();
		_factory = new();
	}

	[Fact]
	public void MapFromDomain___ConversionApplicationStateReturned()
	{
		//arrange
		const ApplicationType expectedApplicationType = ApplicationType.FormAMat;

		ContributorDetails initialContributorDetails = new(
			FirstName: _faker.Name.FirstName(),
			LastName: _faker.Name.LastName(),
			EmailAddress: _faker.Internet.Email(),
			ContributorRole.ChairOfGovernors,
			OtherRoleName: null
		);

		var mockConversionApplication = new Mock<IConversionApplication>();
		var mockContributor = new Mock<IContributor>();

		mockContributor.SetupGet(x => x.Details).Returns(initialContributorDetails);
		mockConversionApplication.SetupGet(x => x.ApplicationType).Returns(expectedApplicationType);
		mockConversionApplication.SetupGet(x => x.Contributors).Returns(new List<IContributor>(new[] { mockContributor.Object }));

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
		
		//arrange
		var result = ConversionApplicationState.MapFromDomain(mockConversionApplication.Object);

		//assert
		Assert.IsType<ConversionApplicationState>(result);
		result.Should().BeEquivalentTo(expected);
	}
}
