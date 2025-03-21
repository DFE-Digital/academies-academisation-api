﻿using System.Collections.Generic;

namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
    public class AcademyTransferProjectSummaryResponse
    {
        public string ProjectUrn { get; set; }
        public string ProjectReference { get; set; }
        public string OutgoingTrustUkprn { get; set; }
        public string OutgoingTrustName { get; set; }
        public string Status { get; set; }
		public List<TransferringAcademyDto> TransferringAcademies { get; set; }
        public AssignedUserResponse AssignedUser { get; set; }
		public bool? IsFormAMat { get; set; }
		public bool IsReadOnly { get; set; }
		public DateTime? ProjectSentToCompleteDate { get; set; }
	}
}
