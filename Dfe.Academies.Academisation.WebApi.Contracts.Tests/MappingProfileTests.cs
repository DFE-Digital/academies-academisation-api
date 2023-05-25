using AutoFixture;
using AutoMapper;
using AutoMapper.Execution;
using Dfe.Academies.Academisation.WebApi.AutoMapper;
using FluentAssertions;

namespace Dfe.Academies.Academisation.WebApi.Contracts.Tests
{
	public class MappingProfileTests
	{
		[Fact]
		public void Contract_Mapping_Profile_Should_Be_Valid()
		{
			CreateMapperConfiguration().AssertConfigurationIsValid();
		}

		[Theory]
		[MemberData(nameof(AdvisoryBoardDecisionEnumValues))]
		public void AdvisoryBoardDecision_Should_Map_To_DomainCore(int enumValue)
		{
			var mapper = CreateMapper();
			
			var contract = (Contracts.FromDomain.AdvisoryBoardDecision)enumValue;
			var domain = mapper.Map<Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision>(contract);

			((int)domain).Should().Be((int)contract);
		}
		
		[Theory]
		[MemberData(nameof(AdvisoryBoardDeclinedReasonEnumValues))]
		public void AdvisoryBoardDeclinedReason_Should_Map_To_DomainCore(int enumValue)
		{
			var mapper = CreateMapper();
			
			var contract = (Contracts.FromDomain.AdvisoryBoardDecision)enumValue;
			var domain = mapper.Map<Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeclinedReason>(contract);

			((int)domain).Should().Be((int)contract);
		}	
		
		[Theory]
		[MemberData(nameof(AdvisoryBoardDeclinedReasonEnumValues))]
		public void AdvisoryBoardDeferredReason_Should_Map_To_DomainCore(int enumValue)
		{
			var mapper = CreateMapper();
			
			var contract = (Contracts.FromDomain.AdvisoryBoardDeferredReason)enumValue;
			var domain = mapper.Map<Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeferredReason>(contract);

			((int)domain).Should().Be((int)contract);
		}
		
		[Theory]
		[MemberData(nameof(ApplicationTypeEnumValues))]
		public void ApplicationType_Should_Map_To_DomainCore(int enumValue)
		{
			var mapper = CreateMapper();
			
			var contract = (Contracts.FromDomain.ApplicationType)enumValue;
			var domain = mapper.Map<Domain.Core.ApplicationAggregate.ApplicationType>(contract);

			((int)domain).Should().Be((int)contract);
		}
				
		[Theory]
		[MemberData(nameof(ContributorRoleEnumValues))]
		public void ContributorRole_Should_Map_To_DomainCore(int enumValue)
		{
			var mapper = CreateMapper();
			
			var contract = (Contracts.FromDomain.ContributorRole)enumValue;
			var domain = mapper.Map<Domain.Core.ApplicationAggregate.ContributorRole>(contract);

			((int)domain).Should().Be((int)contract);
		}	
		
		[Theory]
		[MemberData(nameof(DecisionMadeByEnumValues))]
		public void DecisionMadeBy_Should_Map_To_DomainCore(int enumValue)
		{
			var mapper = CreateMapper();
			
			var contract = (Contracts.FromDomain.ContributorRole)enumValue;
			var domain = mapper.Map<Domain.Core.ApplicationAggregate.ContributorRole>(contract);

			((int)domain).Should().Be((int)contract);
		}

		public static IEnumerable<object[]> AdvisoryBoardDecisionEnumValues()
		{
			foreach (var number in Enum.GetValues(typeof(Contracts.FromDomain.AdvisoryBoardDecision)))
			{
				yield return new object[] { number };
			}
		}		
		
		public static IEnumerable<object[]> AdvisoryBoardDeclinedReasonEnumValues()
		{
			foreach (var number in Enum.GetValues(typeof(Contracts.FromDomain.AdvisoryBoardDeclinedReason)))
			{
				yield return new object[] { number };
			}
		}		
		
		public static IEnumerable<object[]> DecisionMadeByEnumValues()
		{
			foreach (var number in Enum.GetValues(typeof(Contracts.FromDomain.DecisionMadeBy)))
			{
				yield return new object[] { number };
			}
		}
		
		public static IEnumerable<object[]> AdvisoryBoardDeferredReasonEnumValues()
		{
			foreach (var number in Enum.GetValues(typeof(Contracts.FromDomain.AdvisoryBoardDeferredReason)))
			{
				yield return new object[] { number };
			}
		}
		
		public static IEnumerable<object[]> ApplicationTypeEnumValues()
		{
			foreach (var number in Enum.GetValues(typeof(Contracts.FromDomain.ApplicationType)))
			{
				yield return new object[] { number };
			}
		}
		
		public static IEnumerable<object[]> ContributorRoleEnumValues()
		{
			foreach (var number in Enum.GetValues(typeof(Contracts.FromDomain.ContributorRole)))
			{
				yield return new object[] { number };
			}
		}

        private IMapper CreateMapper()
		{
			return CreateMapperConfiguration().CreateMapper();
		}

		private MapperConfiguration CreateMapperConfiguration()
		{
			return new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<SubProfile>();
			});
		}

		internal class SubProfile : Profile
		{
			public SubProfile()
			{
				ContractMappings.AddMappings(this);
			} 
		}
	}
}
