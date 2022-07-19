using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionApplicationAggregate;

public class ConversionApplicationCreateTests
{
	private readonly Faker _faker = new();

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	[InlineData(ApplicationType.FormAMat, "")]
	public async Task RoleIsOther_OtherRoleNameIsNull___ThrowsException(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Internet.Email(),
			ContributorRole.Other, otherRoleName);

		// Act & Assert
		await Assert.ThrowsAsync<FluentValidation.ValidationException>(
			() => target.Create(applicationType, contributor));
	}

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	[InlineData(ApplicationType.FormAMat, "")]
	public async Task RoleIsChair_OtherRoleNameIsNull___ReturnsConversionApplication(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Internet.Email(),
			ContributorRole.ChairOfGovernors, otherRoleName);

		// Act
		var result = await target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<ConversionApplication>(result);
	}

	[Fact]
	public async Task EmailAddressIsInvalid___ThrowsException()
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(_faker.Name.FirstName(), _faker.Name.LastName(),
			_faker.Random.Chars(count: 20).ToString()!, ContributorRole.ChairOfGovernors, null);

		// Act and Assert
		await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
			target.Create(ApplicationType.FormAMat, contributor));
	}
}
