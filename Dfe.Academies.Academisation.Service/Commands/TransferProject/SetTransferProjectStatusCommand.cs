﻿using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectStatusCommand : SetTransferProjectCommand
	{
		public string Status { get; set; }
	}
}
