using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public class GetSignificantChangeFilterParametersQueryHandler(
		ISignificantChangeProjectRepository significantChangeProjectRepository)
		: IRequestHandler<GetSignificantChangeFilterParametersQuery, SignificantChangeFilterParameters>
	{
		private readonly ISignificantChangeProjectRepository _significantChangeProjectRepository = significantChangeProjectRepository;

		public async Task<SignificantChangeFilterParameters> Handle(
			GetSignificantChangeFilterParametersQuery query,
			CancellationToken cancellationToken)
		{
			return await _significantChangeProjectRepository.GetFilterParameters(cancellationToken);
		}
	}
}