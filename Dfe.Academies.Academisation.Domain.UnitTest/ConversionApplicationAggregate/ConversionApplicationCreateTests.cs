﻿using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionApplicationAggregate;

public class ConversionApplicationCreateTests
{
	private readonly Faker _faker = new();

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	[InlineData(ApplicationType.FormAMat, "")]
	public async Task RoleIsOther_OtherRoleNameIsNull___ReturnsValidationErrorResult(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Internet.Email(),
			ContributorRole.Other, otherRoleName);

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
	public async Task RoleIsChair_OtherRoleNameIsNull___ReturnsWrappedConversionApplication(ApplicationType applicationType,
		string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Internet.Email(),
			ContributorRole.ChairOfGovernors, otherRoleName);

		// Act
		var result = target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<CreateSuccessResult<IConversionApplication>>(result);

		var successResult = result as CreateSuccessResult<IConversionApplication>;
		Assert.IsType<ConversionApplication>(successResult!.Payload);
	}

	[Fact]
	public async Task EmailAddressIsInvalid___ReturnsValidationErrorResult()
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(_faker.Name.FirstName(), _faker.Name.LastName(),
			_faker.Random.Chars(count: 20).ToString()!, ContributorRole.ChairOfGovernors, null);

		// Act
		var result = target.Create(ApplicationType.JoinAMat, contributor);

		// Assert
		Assert.IsType<CreateValidationErrorResult<IConversionApplication>>(result);

		var validationErrorResult = result as CreateValidationErrorResult<IConversionApplication>;
		Assert.Contains(validationErrorResult!.ValidationErrors, x => x.PropertyName == "EmailAddress");
	}
}
