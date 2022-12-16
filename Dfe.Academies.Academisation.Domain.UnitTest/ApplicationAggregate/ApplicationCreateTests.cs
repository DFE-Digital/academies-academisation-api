using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate;

public class ApplicationCreateTests
{
	private readonly Faker _faker = new();
	private readonly Fixture _fixture = new();

	[Fact]
	public void ContributorValid___ReturnsSuccessResult()
	{
		// Arrange
		ApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(),
			_faker.Name.LastName(),
			_faker.Internet.Email(),
			ContributorRole.ChairOfGovernors,
			null,
			null);
		var applicationType = _fixture.Create<ApplicationType>();

		// Act
		var result = target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<CreateSuccessResult<IApplication>>(result);

		var successResult = (CreateSuccessResult<IApplication>)result;
		Assert.Equal(ApplicationStatus.InProgress, successResult.Payload.ApplicationStatus);
		Assert.Single(successResult.Payload.Contributors, c => c.Details == contributor);
	}

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	[InlineData(ApplicationType.FormAMat, "")]
	public void ContributorRoleIsOther_OtherRoleNameIsNull___ReturnsValidationErrorResult(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(),
			_faker.Name.LastName(),
			_faker.Internet.Email(),
			ContributorRole.Other,
			otherRoleName, null);

		// Act
		var result = target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<CreateValidationErrorResult>(result);

		var validationErrorResult = result as CreateValidationErrorResult;
		Assert.Contains(validationErrorResult!.ValidationErrors, x => x.PropertyName == "OtherRoleName");
	}

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	[InlineData(ApplicationType.FormAMat, "")]
	public void ContributorRoleIsChair_OtherRoleNameIsNull___ReturnsWrappedApplication(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(),
			_faker.Name.LastName(),
			_faker.Internet.Email(),
			ContributorRole.ChairOfGovernors,
			otherRoleName, null);

		// Act
		var result = target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<CreateSuccessResult<IApplication>>(result);

		var successResult = result as CreateSuccessResult<IApplication>;
		Assert.IsType<Application>(successResult!.Payload);
	}

	[Fact]
	public void ContributorEmailAddressIsInvalid___ReturnsValidationErrorResult()
	{
		// Arrange
		ApplicationFactory target = new();
		ContributorDetails contributor = new(
			_faker.Name.FirstName(),
			_faker.Name.LastName(),
			_faker.Random.Chars(count: 20).ToString()!,
			ContributorRole.ChairOfGovernors,
			null, null);

		// Act
		var result = target.Create(ApplicationType.JoinAMat, contributor);

		// Assert
		Assert.IsType<CreateValidationErrorResult>(result);

		var validationErrorResult = result as CreateValidationErrorResult;
		Assert.Contains(validationErrorResult!.ValidationErrors, x => x.PropertyName == "EmailAddress");
	}

	[Theory]
	[InlineData("", "lastname", "FirstName")]
	[InlineData("firstname", "", "LastName")]
	public void ContributorNameIsInvalid___ReturnsValidationErrorResult(string firstName, string lastName, string expectedValidationError)
	{
		// Arrange
		ApplicationFactory target = new();
		ContributorDetails contributor = new(
			firstName,
			lastName,
			_faker.Random.Chars(count: 20).ToString()!,
			ContributorRole.ChairOfGovernors,
			null, null);

		// Act
		var result = target.Create(ApplicationType.JoinAMat, contributor);

		// Assert
		Assert.IsType<CreateValidationErrorResult>(result);

		var validationErrorResult = result as CreateValidationErrorResult;
		Assert.Contains(validationErrorResult!.ValidationErrors, x => x.PropertyName == expectedValidationError);
	}
}
