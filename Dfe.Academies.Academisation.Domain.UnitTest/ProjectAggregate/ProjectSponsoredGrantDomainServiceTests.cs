﻿using System.Collections.Generic;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ProjectAggregate
{
	public class ProjectSponsoredGrantDomainServiceTests
	{
		public static IEnumerable<object[]> TypeChangedData => new List<object[]>
		{
			new object[] { "fast track", "intermediate", 70000m, 90000m },
			new object[] { "fast track", "full", 70000m, 110000m },
			new object[] { "intermediate", "full", 90000m, 110000m }
		};

		[Theory]
		[MemberData(nameof(TypeChangedData))]
		public void CalculateDefaultSponsoredGrant_TypeChanged_ReturnsNewDefault(string existingType, string newType, decimal? currentAmount, decimal? expected)
		{
			var result = ProjectSponsoredGrantDomainService.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount);
			result.Should().Be(expected);
		}

		public static IEnumerable<object[]> TypeUnchangedData => new List<object[]>
		{
			new object[] { "fast track", "fast track", 70000m, 70000m },
			new object[] { "intermediate", "intermediate", 90000m, 90000m },
			new object[] { "full", "full", 110000m, 110000m }
		};

		[Theory]
		[MemberData(nameof(TypeUnchangedData))]
		public void CalculateDefaultSponsoredGrant_TypeUnchanged_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected)
		{
			var result = ProjectSponsoredGrantDomainService.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount);
			result.Should().Be(expected);
		}

		public static IEnumerable<object[]> EmptyExistingTypeData => new List<object[]>
		{
			new object[] { null, "fast track", null, 70000m },
			new object[] { null, "intermediate", null, 90000m },
			new object[] { null, "full", null, 110000m }
		};

		[Theory]
		[MemberData(nameof(EmptyExistingTypeData))]
		public void CalculateDefaultSponsoredGrant_EmptyExistingType_ReturnsNewDefault(string existingType, string newType, decimal? currentAmount, decimal? expected)
		{
			var result = ProjectSponsoredGrantDomainService.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount);
			result.Should().Be(expected);
		}

		public static IEnumerable<object[]> NullNewTypeData => new List<object[]>
		{
			new object[] { "fast track", null, 70000m, 70000m },
			new object[] { "intermediate", null, 90000m, 90000m },
			new object[] { "full", null, 110000m, 110000m }
		};

		[Theory]
		[MemberData(nameof(NullNewTypeData))]
		public void CalculateDefaultSponsoredGrant_NullNewType_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected)
		{
			var result = ProjectSponsoredGrantDomainService.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount);
			result.Should().Be(expected);
		}

		public static IEnumerable<object[]> InvalidNewTypeData => new List<object[]>
		{
			new object[] { "fast track", "invalid", 70000m, 70000m }
		};

		[Theory]
		[MemberData(nameof(InvalidNewTypeData))]
		public void CalculateDefaultSponsoredGrant_InvalidNewType_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected)
		{
			var result = ProjectSponsoredGrantDomainService.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount);
			result.Should().Be(expected);
		}

		public static IEnumerable<object[]> NullExistingAndNewTypeData => new List<object[]>
		{
			new object[] { null, null, 50000m, 50000m }
		};

		[Theory]
		[MemberData(nameof(NullExistingAndNewTypeData))]
		public void CalculateDefaultSponsoredGrant_NullExistingAndNewType_ReturnsCurrentAmount(string existingType, string newType, decimal? currentAmount, decimal? expected)
		{
			var result = ProjectSponsoredGrantDomainService.CalculateDefaultSponsoredGrant(existingType, newType, currentAmount);
			result.Should().Be(expected);
		}
	}
}
