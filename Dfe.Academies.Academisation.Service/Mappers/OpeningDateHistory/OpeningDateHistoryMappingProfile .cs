using AutoMapper;
using Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate;
using Dfe.Academies.Academisation.Service.Queries;

namespace Dfe.Academies.Academisation.Service.Mappers.OpeningDateHistoryMapper
{
	public class OpeningDateHistoryMappingProfile : Profile
	{
		public OpeningDateHistoryMappingProfile()
		{
			CreateMap<OpeningDateHistory, OpeningDateHistoryDto>();
		}
	}
}
