using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public record ConversionProjectAddNoteCommand(string Subject, string Note, string Author, DateTime Date, int ProjectId) : IRequest<CommandResult>;
}
