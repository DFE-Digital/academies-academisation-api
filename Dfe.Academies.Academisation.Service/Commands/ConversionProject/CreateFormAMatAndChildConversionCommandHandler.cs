using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class CreateFormAMatAndChildConversionCommandHandler : IRequestHandler<CreateFormAMatAndChildConversionCommand, CommandResult>
	{

		private readonly IMapper _mapper;
		private readonly ILogger<CreateFormAMatAndChildConversionCommandHandler> _logger;
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IFormAMatProjectRepository _formAMatProjectRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		public CreateFormAMatAndChildConversionCommandHandler(IMapper mapper, IConversionProjectRepository conversionProjectRepository, ILogger<CreateFormAMatAndChildConversionCommandHandler> logger, IFormAMatProjectRepository formAMatProjectRepository, IDateTimeProvider dateTimeProvider)
		{
			_mapper = mapper;
			_conversionProjectRepository = conversionProjectRepository;
			_formAMatProjectRepository = formAMatProjectRepository;
			_logger = logger;
			_dateTimeProvider = dateTimeProvider;
		}


		public async Task<CommandResult> Handle(CreateFormAMatAndChildConversionCommand request, CancellationToken cancellationToken)
		{
			// Create Form A Mat (FAM) project and persist it
			var newFormAMat = FormAMatProject.Create(
				request.Conversion.Trust.Name,
				string.Empty,
				_dateTimeProvider.Now);

			_formAMatProjectRepository.Insert(newFormAMat);
			await _formAMatProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// Update FAM project with its reference and persist the update
			newFormAMat.SetProjectReference(newFormAMat.Id);
			_formAMatProjectRepository.Update(newFormAMat);
			await _formAMatProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			var newConversion = Project.CreateNewProject(_mapper.Map<NewProject>(request.Conversion));

			if (newConversion is CreateSuccessResult<IProject> successResult)
			{
				var project = successResult.Payload;

				project.SetFormAMatProjectId(newFormAMat.Id);
				_conversionProjectRepository.Insert((Project)project);
				await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			}

			// Assuming successful execution up to this point
			return new CommandSuccessResult();
		}
	}
}


