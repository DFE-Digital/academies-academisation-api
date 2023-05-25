using AutoMapper;
using Dfe.Academies.Academisation.WebApi.AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Dfe.Academies.Academisation.WebApi.Contracts.Tests
{
	public class EnumMappingTests
	{
		private readonly IMapper _mapper;

		public EnumMappingTests()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<AutoMapperProfile>();
			});
			_mapper = config.CreateMapper();
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.AdvisoryBoardDecision))]
		public void AdvisoryBoardDecision_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.AdvisoryBoardDecision)enumValue;
			ContractEnum_Should_MapTo_DomainEnum_AndReverse<FromDomain.AdvisoryBoardDecision, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision>(contract, enumValue);
		}


		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.AdvisoryBoardDeclinedReason))]
		public void AdvisoryBoardDeclinedReason_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.AdvisoryBoardDeclinedReason)enumValue;
			ContractEnum_Should_MapTo_DomainEnum_AndReverse<FromDomain.AdvisoryBoardDeclinedReason, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeclinedReason>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.AdvisoryBoardDeferredReason))]
		public void AdvisoryBoardDeferredReason_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.AdvisoryBoardDeferredReason)enumValue;
			ContractEnum_Should_MapTo_DomainEnum_AndReverse<FromDomain.AdvisoryBoardDeferredReason, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeferredReason>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.ApplicationType))]
		public void ApplicationType_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.ApplicationType)enumValue;
			ContractEnum_Should_MapTo_DomainEnum_AndReverse<FromDomain.ApplicationType, Domain.Core.ApplicationAggregate.ApplicationType>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.ContributorRole))]
		public void ContributorRole_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.ContributorRole)enumValue;
			ContractEnum_Should_MapTo_DomainEnum_AndReverse<FromDomain.ContributorRole, Domain.Core.ApplicationAggregate.ContributorRole>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.DecisionMadeBy))]
		public void DecisionMadeBy_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.DecisionMadeBy)enumValue;
			ContractEnum_Should_MapTo_DomainEnum_AndReverse<FromDomain.DecisionMadeBy, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.DecisionMadeBy>(contract, enumValue);
		}

		public static IEnumerable<object[]> YieldEnumValues(Type enumType)
		{
			foreach (object? number in Enum.GetValues(enumType))
			{
				yield return new object[] { number };
			}
		}

		private void ContractEnum_Should_MapTo_DomainEnum_AndReverse<TSourceEnum, TTargetEnum>(TSourceEnum sourceEnumVal, int enumValue)
			where TSourceEnum : Enum
			where TTargetEnum : Enum
		{
			using var scope = new AssertionScope();
			scope.AddReportable("inputs", $"Source Enum type: {typeof(TSourceEnum).Name}, Target Enum type: {typeof(TTargetEnum).Name}, int value: {enumValue}");

			Enum.IsDefined(typeof(TSourceEnum), enumValue).Should().BeTrue();
			Enum.IsDefined(typeof(TTargetEnum), enumValue).Should().BeTrue();

			var targetEnumVal = _mapper.Map<TTargetEnum>(sourceEnumVal);
			targetEnumVal.ToString().Should().Be(sourceEnumVal.ToString());

			// assert reverse map
			var reversedVal = _mapper.Map<TSourceEnum>(targetEnumVal);
			reversedVal.ToString().Should().Be(targetEnumVal.ToString());

		}
	}
}
