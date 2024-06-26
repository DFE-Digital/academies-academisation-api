using AutoMapper;
using Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class GetOpeningDateHistoryQuery : IRequest<IEnumerable<OpeningDateHistoryDto>>
	{
		public string EntityType { get; set; }
		public int EntityId { get; set; }

		public GetOpeningDateHistoryQuery(string entityType, int entityId)
		{
			EntityType = entityType;
			EntityId = entityId;
		}
	}
	public class GetOpeningDateHistoryQueryHandler : IRequestHandler<GetOpeningDateHistoryQuery, IEnumerable<OpeningDateHistoryDto>>
	{
		private readonly IOpeningDateHistoryRepository _repository;
		private readonly IMapper _mapper;

		public GetOpeningDateHistoryQueryHandler(IOpeningDateHistoryRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<OpeningDateHistoryDto>> Handle(GetOpeningDateHistoryQuery request, CancellationToken cancellationToken)
		{
			var histories = await _repository.GetByEntityTypeAndIdAsync(request.EntityType, request.EntityId);
			return _mapper.Map<IEnumerable<OpeningDateHistoryDto>>(histories);
		}
	}
	public class OpeningDateHistoryDto
	{
		public int Id { get; set; }
		public int EntityId { get; set; }
		public string EntityType { get; set; }
		public DateTime? OldDate { get; set; }
		public DateTime? NewDate { get; set; }
		public DateTime ChangedAt { get; set; }
		public string ChangedBy { get; set; }
		public string ReasonForChangeDetails { get; set; }
	}
}
