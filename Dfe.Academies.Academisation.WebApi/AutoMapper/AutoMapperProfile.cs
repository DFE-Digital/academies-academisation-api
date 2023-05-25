using AutoMapper;
using Dfe.Academies.Academisation.Service.AutoMapper;

namespace Dfe.Academies.Academisation.WebApi.AutoMapper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			AutoMapperSetup.AddMappings(this);
			ContractMappings.AddMappings(this);
		}
	}
}
