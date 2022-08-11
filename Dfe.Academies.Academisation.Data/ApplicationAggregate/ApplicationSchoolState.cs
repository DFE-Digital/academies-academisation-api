﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

[Table(name: "ApplicationSchool")]
public class ApplicationSchoolState : BaseEntity
{
	public int Urn { get; set; }
	public string SchoolName { get; set; } = null!;

	// contact details
	public string? ContactHeadName { get; set; }
	public string? ContactHeadEmail { get; set; }
	public string? ContactHeadTel { get; set; }
	public string? ContactChairName { get; set; }
	public string? ContactChairEmail { get; set; }
	public string? ContactChairTel { get; set; }
	public string? ContactRole { get; set; } // "headteacher", "chair of governing body", "someone else"
	public string? MainContactOtherName { get; set; }
	public string? MainContactOtherEmail { get; set; }
	public string? MainContactOtherTelephone { get; set; }
	public string? MainContactOtherRole { get; set; }
	public string? ApproverContactName { get; set; }
	public string? ApproverContactEmail { get; set; }

	// conversion details
	DateTime? ConversionTargetDate { get; set; }
	public string? ConversionTargetDateExplained { get; set; }
	public string? ProposedNewSchoolName { get; set; }
	public string? JoinTrustReason { get; set; }

	// future pupil numbers
	public int? ProjectedPupilNumbersYear1 { get; set; }
	public int? ProjectedPupilNumbersYear2 { get; set; }
	public int? ProjectedPupilNumbersYear3 { get; set; }
	public string? CapacityAssumptions { get; set; }
	public int? CapacityPublishedAdmissionsNumber { get; set; }

	public static ApplicationSchoolState MapFromDomain(ISchool applyingSchool)
	{
		return new()
		{
			Id = applyingSchool.Id,
			Urn = applyingSchool.Details.Urn,
			SchoolName = applyingSchool.Details.SchoolName,
			ContactRole = applyingSchool.Details.ContactRole,
			ApproverContactEmail = applyingSchool.Details.ApproverContactEmail,
			ApproverContactName = applyingSchool.Details.ApproverContactName,
			ContactChairEmail = applyingSchool.Details.ContactChairEmail,
			ContactChairName = applyingSchool.Details.ContactChairName,
			ContactChairTel = applyingSchool.Details.ContactChairTel,
			ContactHeadEmail = applyingSchool.Details.ContactHeadEmail,
			ContactHeadName = applyingSchool.Details.ContactHeadName,
			ContactHeadTel = applyingSchool.Details.ContactHeadTel,
			MainContactOtherEmail = applyingSchool.Details.MainContactOtherEmail,
			MainContactOtherName = applyingSchool.Details.MainContactOtherName,
			MainContactOtherRole = applyingSchool.Details.MainContactOtherRole,
			MainContactOtherTelephone = applyingSchool.Details.MainContactOtherTelephone,
			JoinTrustReason = applyingSchool.Details.ApplicationJoinTrustReason,
			ConversionTargetDate = applyingSchool.Details.ConversionTargetDate,
			ConversionTargetDateExplained = applyingSchool.Details.ConversionTargetDateExplained,
			ProposedNewSchoolName = applyingSchool.Details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = applyingSchool.Details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = applyingSchool.Details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = applyingSchool.Details.ProjectedPupilNumbersYear3,
			CapacityAssumptions = applyingSchool.Details.CapacityAssumptions,
			CapacityPublishedAdmissionsNumber = applyingSchool.Details.CapacityPublishedAdmissionsNumber
		};
	}

	public SchoolDetails MapToDomain()
	{
		return new SchoolDetails(Urn, SchoolName)
		{			
			ContactRole = ContactRole,
			ApproverContactEmail = ApproverContactEmail,
			ApproverContactName = ApproverContactName,
			ContactChairEmail = ContactChairEmail,
			ContactChairName = ContactChairName,
			ContactChairTel = ContactChairTel,
			ContactHeadEmail = ContactHeadEmail,
			ContactHeadName = ContactHeadName,
			ContactHeadTel = ContactHeadTel,
			MainContactOtherEmail = MainContactOtherEmail,
			MainContactOtherName = MainContactOtherName,
			MainContactOtherRole = MainContactOtherRole,
			MainContactOtherTelephone = MainContactOtherTelephone,
			ApplicationJoinTrustReason = JoinTrustReason,
			ConversionTargetDate = ConversionTargetDate,
			ConversionTargetDateExplained = ConversionTargetDateExplained,
			ProposedNewSchoolName = ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = ProjectedPupilNumbersYear3,
			CapacityAssumptions = CapacityAssumptions,
			CapacityPublishedAdmissionsNumber = CapacityPublishedAdmissionsNumber
		};
	}
}
