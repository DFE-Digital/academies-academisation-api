﻿using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionApplicationAggregate;

public class ConversionApplicationCreateTests
{
	private readonly Faker faker = new();

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
//		[InlineData(ApplicationType.FormAMat, "")]
	public async Task RoleIsOther_OtherRoleNameIsNull___ThrowsException(ApplicationType applicationType, string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(faker.Name.FirstName(), faker.Name.LastName(), faker.Internet.Email(), ContributorRole.Other, otherRoleName);

		// Act & Assert
		await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => target.Create(applicationType, contributor));
	}

	[Theory]
	[InlineData(ApplicationType.FormAMat, null)]
	//		[InlineData(ApplicationType.FormAMat, "")]
	public async Task RoleIsChair_OtherRoleNameIsNull___ReturnsConversionApplication(ApplicationType applicationType, string otherRoleName)
	{
		// Arrange
		ConversionApplicationFactory target = new();
		ContributorDetails contributor = new(faker.Name.FirstName(), faker.Name.LastName(), faker.Internet.Email(), ContributorRole.ChairOfGovernors, otherRoleName);

		// Act
		var result = await target.Create(applicationType, contributor);

		// Assert
		Assert.IsType<ConversionApplication>(result);
	}
}