
using AutoMapper;
using Dfe.Academies.Academisation.Domain.Summary;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ProjectSummaryQuery(string emailAddress, bool includeConversions, bool includeTransfers, bool includeFormAMat, string? searchTerm) : IRequest<IEnumerable<ProjectSummary>>
	{
		public string EmailAddress { get; set; } = emailAddress;
		public bool IncludeConversions { get; set; } = includeConversions;
		public bool IncludeTransfers { get; set; } = includeTransfers;
		public bool IncludeFormAMat { get; set; } = includeFormAMat;
		public string? SearchTerm { get; set; } = searchTerm;
	}

	public class ProjectSummaryQueryHandler : IRequestHandler<ProjectSummaryQuery, IEnumerable<ProjectSummary>>
	{
		private readonly ISummaryDataService _summaryRepository;
		
		public ProjectSummaryQueryHandler(ISummaryDataService summaryRepository, IMapper mapper)
		{
			_summaryRepository = summaryRepository;
		}

		public async Task<IEnumerable<ProjectSummary>> Handle(ProjectSummaryQuery request, CancellationToken cancellationToken)
		{
			var summaries = await _summaryRepository.GetProjectSummariesByAssignedEmail(request.EmailAddress,
																												request.IncludeConversions,
																												request.IncludeTransfers,
																												request.IncludeFormAMat,
																												request.SearchTerm);
			return summaries;
		}
	}
}
