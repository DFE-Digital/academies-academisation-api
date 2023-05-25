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
			Assert_ContractEnum_MapsTo_DomainCoreEnum<FromDomain.AdvisoryBoardDecision, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision>(contract, enumValue);
		}


		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.AdvisoryBoardDeclinedReason))]
		public void AdvisoryBoardDeclinedReason_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.AdvisoryBoardDeclinedReason)enumValue;
			Assert_ContractEnum_MapsTo_DomainCoreEnum<FromDomain.AdvisoryBoardDeclinedReason, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeclinedReason>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.AdvisoryBoardDeferredReason))]
		public void AdvisoryBoardDeferredReason_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.AdvisoryBoardDeferredReason)enumValue;
			Assert_ContractEnum_MapsTo_DomainCoreEnum<FromDomain.AdvisoryBoardDeferredReason, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeferredReason>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.ApplicationType))]
		public void ApplicationType_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.ApplicationType)enumValue;
			Assert_ContractEnum_MapsTo_DomainCoreEnum<FromDomain.ApplicationType, Domain.Core.ApplicationAggregate.ApplicationType>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.ContributorRole))]
		public void ContributorRole_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.ContributorRole)enumValue;
			Assert_ContractEnum_MapsTo_DomainCoreEnum<FromDomain.ContributorRole, Domain.Core.ApplicationAggregate.ContributorRole>(contract, enumValue);
		}

		[Theory]
		[MemberData(nameof(YieldEnumValues), typeof(FromDomain.DecisionMadeBy))]
		public void DecisionMadeBy_Should_Map_To_DomainCore(int enumValue)
		{
			var contract = (FromDomain.DecisionMadeBy)enumValue;
			Assert_ContractEnum_MapsTo_DomainCoreEnum<FromDomain.DecisionMadeBy, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.DecisionMadeBy>(contract, enumValue);
		}

		public static IEnumerable<object[]> YieldEnumValues(Type enumType)
		{
			foreach (object? number in Enum.GetValues(enumType))
			{
				yield return new object[] { number };
			}
		}

		private void Assert_ContractEnum_MapsTo_DomainCoreEnum<TContract, TDomain>(TContract contractEnumVal, int enumValue)
			where TContract : Enum
			where TDomain : Enum
		{
			using var scope = new AssertionScope();
			scope.AddReportable("inputs", $"Contract Enum type: {typeof(TContract).Name}, Domain Enum type: {typeof(TDomain).Name}, int value: {enumValue}");

			Enum.IsDefined(typeof(TContract), enumValue).Should().BeTrue();
			Enum.IsDefined(typeof(TDomain), enumValue).Should().BeTrue();

			var domainVal = _mapper.Map<TDomain>(contractEnumVal);

			domainVal.ToString().Should().Be(contractEnumVal.ToString());
		}
	}
}
