using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest;

public class ConversionApplicationStateCreateTests
{
	private readonly Faker _faker;
	private readonly ConversionApplicationFactory _factory;

	public ConversionApplicationStateCreateTests()
	{
		_faker = new();
		_factory = new();
	}

	[Fact]
	public async Task ShouldReturnConversionApplicationState()
	{
		//act
		const ApplicationType expectedApplicationType = ApplicationType.FormAMat;

		ContributorDetails initialContributor = new(
			FirstName: _faker.Name.FirstName(),
			LastName: _faker.Name.LastName(),
			EmailAddress: _faker.Internet.Email(),
			ContributorRole.ChairOfGovernors,
			OtherRoleName: null
		);
		
		var conversionApplication = await _factory.Create(expectedApplicationType, initialContributor);

		//arrange
		var result = ConversionApplicationState.MapFromDomain(conversionApplication);

		//act
		Assert.IsType<ConversionApplicationState>(result);
	}
	
	[Fact]
	public async Task ShouldReturnExpectedConversionApplicationState()
	{
		//arrange
		const ApplicationType expectedApplicationType = ApplicationType.FormAMat;

		ContributorDetails initialContributor = new(
			FirstName: _faker.Name.FirstName(),
			LastName: _faker.Name.LastName(),
			EmailAddress: _faker.Internet.Email(),
			ContributorRole.ChairOfGovernors,
			OtherRoleName: null
		);

		var conversionApplication = await _factory.Create(expectedApplicationType, initialContributor);

		var expected = new ConversionApplicationState
		{
			ApplicationType = expectedApplicationType,
			Contributors = new() {
				new()
				{
					FirstName = initialContributor.FirstName,
					LastName = initialContributor.LastName,
					EmailAddress = initialContributor.EmailAddress,
					Role = initialContributor.Role,
					OtherRoleName = initialContributor.OtherRoleName
				}
			}
		};
		
		//arrange
		var result = ConversionApplicationState.MapFromDomain(conversionApplication);

		//act
		result.Should().BeEquivalentTo(expected);
	}
}
