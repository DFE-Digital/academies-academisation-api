﻿using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

[Table(name: "ConversionApplication")]
public class ApplicationState : BaseEntity
{
	public ApplicationType ApplicationType { get; set; }
	public ApplicationStatus ApplicationStatus { get; set; }
	[ForeignKey("ConversionApplicationId")]
	public HashSet<ContributorState> Contributors { get; set; } = new();
	[ForeignKey("ConversionApplicationId")]
	public HashSet<ApplicationSchoolState> Schools { get; set; } = new();

	public JoinTrustState? JoinTrust { get; set; }
	public FormTrustState? FormTrust { get; set; }
	public DateTime? ApplicationSubmittedDate { get; set; }

	// MR:- below mods for Dynamics -> SQL server A2B external app conversion
	public Guid? DynamicsApplicationId { get; set; }

	/// <summary>
	/// This is in the format $"A2B_{ApplicationId}"
	/// Currently calculated by new UI but we need somewhere to store existing data from dynamics
	/// </summary>
	public string? ApplicationReference { get; set; }

	public static ApplicationState MapFromDomain(IApplication application, IMapper mapper)
	{
		return new()
		{
			Id = application.ApplicationId,
			CreatedOn = application.CreatedOn,
			LastModifiedOn = application.LastModifiedOn,
			ApplicationStatus = application.ApplicationStatus,
			ApplicationType = application.ApplicationType,
			Contributors = application.Contributors
				.Select(ContributorState.MapFromDomain)
				.ToHashSet(),
			Schools = application.Schools
				.Select(ApplicationSchoolState.MapFromDomain)
				.ToHashSet(),
			FormTrust = mapper.Map<FormTrustState>(application.FormTrust),
			JoinTrust = mapper.Map<JoinTrustState>(application.JoinTrust),
			ApplicationSubmittedDate = application.ApplicationSubmittedDate,
			ApplicationReference = application.ApplicationReference
		};
	}

	public Application MapToDomain(IMapper mapper)
	{
		var contributorsDictionary = Contributors.ToDictionary(
			c => c.Id,
			c => new ContributorDetails(c.FirstName, c.LastName, c.EmailAddress, c.Role, c.OtherRoleName, c.DynamicsApplicationId));

		var schoolsList = Schools.Select(n => n.MapToDomain());

		return new Application(Id, CreatedOn, LastModifiedOn, ApplicationType, ApplicationStatus,
		contributorsDictionary, schoolsList,
								mapper.Map<JoinTrust>(JoinTrust),
								mapper.Map<FormTrust>(FormTrust),
								ApplicationSubmittedDate, ApplicationReference);
	}
}
