﻿using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetSchoolOverviewCommand : IRequest<CommandResult>
	{
		public SetSchoolOverviewCommand(
			int id,
			string publishedAdmissionNumber,
			string viabilityIssues,
			string partOfPfiScheme,
			string financialDeficit,
			decimal? numberOfPlacesFundedFor,
			decimal? numberOfResidentialPlaces,
			decimal? numberOfFundedResidentialPlaces,
			string pfiSchemeDetails,
			decimal? distanceFromSchoolToTrustHeadquarters,
			string distanceFromSchoolToTrustHeadquartersAdditionalInformation,
			string memberOfParliamentNameAndParty,
			bool? pupilsAttendingGroupPermanentlyExcluded,
			bool? pupilsAttendingGroupMedicalAndHealthNeeds,
			bool? pupilsAttendingGroupTeenageMums)
		{
			Id = id;
			PublishedAdmissionNumber = publishedAdmissionNumber;
			ViabilityIssues = viabilityIssues;
			PartOfPfiScheme = partOfPfiScheme;
			FinancialDeficit = financialDeficit;
			NumberOfPlacesFundedFor = numberOfPlacesFundedFor;
			NumberOfResidentialPlaces = numberOfResidentialPlaces;
			NumberOfFundedResidentialPlaces = numberOfFundedResidentialPlaces;
			PfiSchemeDetails = pfiSchemeDetails;
			DistanceFromSchoolToTrustHeadquarters = distanceFromSchoolToTrustHeadquarters;
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = distanceFromSchoolToTrustHeadquartersAdditionalInformation;
			MemberOfParliamentNameAndParty = memberOfParliamentNameAndParty;
			PupilsAttendingGroupPermanentlyExcluded = pupilsAttendingGroupPermanentlyExcluded;
			PupilsAttendingGroupMedicalAndHealthNeeds = pupilsAttendingGroupMedicalAndHealthNeeds;
			PupilsAttendingGroupTeenageMums = pupilsAttendingGroupTeenageMums;
		}

		public int Id { get; set; }
		public string PublishedAdmissionNumber { get; set; }
		public string ViabilityIssues { get; set; }
		public string PartOfPfiScheme { get; set; }
		public string FinancialDeficit { get; set; }
		public decimal? NumberOfPlacesFundedFor { get; set; }
		public decimal? NumberOfResidentialPlaces { get; set; }
		public decimal? NumberOfFundedResidentialPlaces { get; set; }
		public string PfiSchemeDetails { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		public string MemberOfParliamentNameAndParty { get; set; }
		public bool? PupilsAttendingGroupPermanentlyExcluded { get; set; }
		public bool? PupilsAttendingGroupMedicalAndHealthNeeds { get; set; }
		public bool? PupilsAttendingGroupTeenageMums { get; set; }
	}
}


