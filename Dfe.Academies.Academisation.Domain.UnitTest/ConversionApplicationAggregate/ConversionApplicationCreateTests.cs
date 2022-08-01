using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionApplicationAggregate;

public class ConversionApplicationCreateTests
{
	private readonly Faker _faker = new();
	private readonly Fixture _fixture = new();

	[Fact]
	public void ContributorValid___ReturnsSuccessResult()
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(),
			_faker.Name.LastName(),
			_faker.Internet.Email(),
			ContributorRole.ChairOfGovernors,
			null);
		var applicationType = _fixture.Create<ApplicationType>();

		// Act
		var result = target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<CreateSuccessResult<IConversionApplication>>(result);
	}

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	[InlineData(ApplicationType.FormAMat, "")]
	public void ContributorRoleIsOther_OtherRoleNameIsNull___ReturnsValidationErrorResult(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(),
			_faker.Name.LastName(),
			_faker.Internet.Email(),
			ContributorRole.Other,
			otherRoleName);

		// Act
		var result = target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<CreateValidationErrorResult<IConversionApplication>>(result);

		var validationErrorResult = result as CreateValidationErrorResult<IConversionApplication>;
		Assert.Contains(validationErrorResult!.ValidationErrors, x => x.PropertyName == "OtherRoleName");
	}

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	[InlineData(ApplicationType.FormAMat, "")]
	public void ContributorRoleIsChair_OtherRoleNameIsNull___ReturnsWrappedConversionApplication(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(),
			_faker.Name.LastName(),
			_faker.Internet.Email(),
			ContributorRole.ChairOfGovernors, 
			otherRoleName);

		// Act
		var result = target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<CreateSuccessResult<IConversionApplication>>(result);

		var successResult = result as CreateSuccessResult<IConversionApplication>;
		Assert.IsType<ConversionApplication>(successResult!.Payload);
	}

	[Fact]
	public void ContributorEmailAddressIsInvalid___ReturnsValidationErrorResult()
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(), 
			_faker.Name.LastName(),
			_faker.Random.Chars(count: 20).ToString()!, 
			ContributorRole.ChairOfGovernors, 
			null);

		// Act
		var result = target.Create(ApplicationType.JoinAMat, contributor);

		// Assert
		Assert.IsType<CreateValidationErrorResult<IConversionApplication>>(result);

		var validationErrorResult = result as CreateValidationErrorResult<IConversionApplication>;
		Assert.Contains(validationErrorResult!.ValidationErrors, x => x.PropertyName == "EmailAddress");
	}

	[Theory]
	[InlineData("","lastname","FirstName")]
	[InlineData("firstname", "", "LastName")]
	public void ContributorNameIsInvalid___ReturnsValidationErrorResult(string firstName, string lastName, string expectedValidationError)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(
			firstName, 
			lastName,
			_faker.Random.Chars(count: 20).ToString()!, 
			ContributorRole.ChairOfGovernors, 
			null);

		// Act
		var result = target.Create(ApplicationType.JoinAMat, contributor);

		// Assert
		Assert.IsType<CreateValidationErrorResult<IConversionApplication>>(result);

		var validationErrorResult = result as CreateValidationErrorResult<IConversionApplication>;
		Assert.Contains(validationErrorResult!.ValidationErrors, x => x.PropertyName == expectedValidationError);
	}
}
