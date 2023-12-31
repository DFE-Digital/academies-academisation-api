using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
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
			string pfiSchemeDetails,
			decimal? distanceFromSchoolToTrustHeadquarters,
			string distanceFromSchoolToTrustHeadquartersAdditionalInformation,
			string memberOfParliamentNameAndParty)
		{
			Id = id;
			PublishedAdmissionNumber = publishedAdmissionNumber;
			ViabilityIssues = viabilityIssues;
			PartOfPfiScheme = partOfPfiScheme;
			FinancialDeficit = financialDeficit;
			NumberOfPlacesFundedFor = numberOfPlacesFundedFor;
			PfiSchemeDetails = pfiSchemeDetails;
			DistanceFromSchoolToTrustHeadquarters = distanceFromSchoolToTrustHeadquarters;
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = distanceFromSchoolToTrustHeadquartersAdditionalInformation;
			MemberOfParliamentNameAndParty = memberOfParliamentNameAndParty;
		}

		public int Id { get; set; }
		public string PublishedAdmissionNumber { get; set; }
		public string ViabilityIssues { get; set; }
		public string PartOfPfiScheme { get; set; }
		public string FinancialDeficit { get; set; }
		public decimal? NumberOfPlacesFundedFor { get; set; }
		public string PfiSchemeDetails { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		public string MemberOfParliamentNameAndParty { get; set; }
	}
}


