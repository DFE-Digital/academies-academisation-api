using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using MediatR;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTransferDatesCommand : SetTransferProjectCommand
	{
		public DateTime? HtbDate { get; set; }
		public DateTime? TargetDateForTransfer { get; set; }
	}
}
