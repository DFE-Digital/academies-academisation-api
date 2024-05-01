using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using MediatR;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class ConversionProjectCreateCommandHandler : IRequestHandler<ConversionProjectCreateCommand, CreateResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IMapper _mapper;

		public ConversionProjectCreateCommandHandler(IConversionProjectRepository conversionProjectRepository, IMapper mapper)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_mapper = mapper;
		}

		public async Task<CreateResult> Handle(ConversionProjectCreateCommand request, CancellationToken cancellationToken)
		{
			var project = _mapper.Map<NewProject>(request.Conversion);

			var domainServiceResult = Project.CreateNewProject(project);

			switch (domainServiceResult)
			{
				case CreateValidationErrorResult createValidationErrorResult:
					return new CreateValidationErrorResult(createValidationErrorResult.ValidationErrors);
				case CreateSuccessResult<IProject> createSuccessResult:
					await ExecuteDataCommand(createSuccessResult.Payload, cancellationToken);
					return createSuccessResult;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
		}

		private async Task ExecuteDataCommand(IProject project, CancellationToken cancellationToken)
		{
			_conversionProjectRepository.Insert(project as Project);

			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
		}
	}
}

