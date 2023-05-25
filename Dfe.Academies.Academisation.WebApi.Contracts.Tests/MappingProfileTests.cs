using AutoMapper;
using Dfe.Academies.Academisation.WebApi.AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Dfe.Academies.Academisation.WebApi.Contracts.Tests
{
	public class MappingProfileTests
	{
		[Fact]
		public void Contract_Mapping_Profile_Should_Be_Valid()
		{
			CreateMapperConfiguration().AssertConfigurationIsValid();
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
