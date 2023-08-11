using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTransferDatesCommand : IRequest<CommandResult>
	{
		public int Id { get; set; }
		public DateTime HtbDate { get; set; }
		public DateTime TargetDateForTransfer { get; set; }
	}
}
