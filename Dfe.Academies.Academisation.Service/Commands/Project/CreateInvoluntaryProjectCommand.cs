using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Project
{
	public record CreateInvoluntaryProjectCommand(int SchoolId, int TrustId) : IRequest<CommandOrCreateResult>;
}
