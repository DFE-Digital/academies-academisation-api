using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.IService.Commands.Application
{
	public record ApplicationSubmitCommand(
		int applicationId) : IRequest<CommandOrCreateResult>;
}
