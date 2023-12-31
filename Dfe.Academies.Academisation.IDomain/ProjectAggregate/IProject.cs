namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate;

public interface IProject
{
    public int Id { get; }

    DateTime CreatedOn { get; }
    DateTime LastModifiedOn { get; }

    public IReadOnlyCollection<IProjectNote> Notes { get; }

    public ProjectDetails Details { get; }

    public CommandResult Update(ProjectDetails detailsToUpdate);

    public void SetExternalApplicationForm(bool ExternalApplicationFormSaved, string ExternalApplicationFormUrl);
    public void SetSchoolOverview(string publishedAdmissionNumber, string viabilityIssues, string partOfPfiScheme,
            string financialDeficit, decimal? numberOfPlacesFundedFor, string pfiSchemeDetails,
            decimal? distanceFromSchoolToTrustHeadquarters,
            string distanceFromSchoolToTrustHeadquartersAdditionalInformation,
            string memberOfParliamentNameAndParty);
}
