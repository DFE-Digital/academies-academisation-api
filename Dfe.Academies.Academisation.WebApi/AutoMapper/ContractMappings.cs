using Ardalis.GuardClauses;
using AutoMapper;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.WebApi.AutoMapper
{
	/// <summary>
	/// exclusively the new mappings for the contracts that have been introduced
	/// </summary>
	public static class ContractMappings
	{
		public static void AddMappings(AutoMapperProfile profile)
		{
			Guard.Against.NullOrEmpty(nameof(profile));

			profile.CreateMap<WebApi.Contracts.FromDomain.AdvisoryBoardDecision, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision>();
			profile.CreateMap<WebApi.Contracts.FromDomain.AdvisoryBoardDeclinedReason, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeclinedReason>();
			profile.CreateMap<WebApi.Contracts.FromDomain.AdvisoryBoardDeclinedReasonDetails, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeclinedReasonDetails>();
			profile.CreateMap<WebApi.Contracts.FromDomain.AdvisoryBoardDeferredReason, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeferredReason>();
			profile.CreateMap<WebApi.Contracts.FromDomain.AdvisoryBoardDeferredReasonDetails, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeferredReasonDetails>();
			profile.CreateMap<WebApi.Contracts.FromDomain.DecisionMadeBy, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.DecisionMadeBy>();
			
			profile.CreateMap<WebApi.Contracts.FromDomain.ApplicationType, Domain.Core.ApplicationAggregate.ApplicationType>();
			profile.CreateMap<WebApi.Contracts.FromDomain.ContributorRole, Domain.Core.ApplicationAggregate.ContributorRole>();
			
			
			profile.CreateMap<WebApi.Contracts.RequestModels.AdvisoryBoardDecisionCreateRequestModel, Dfe.Academies.Academisation.IService.RequestModels.AdvisoryBoardDecisionCreateRequestModel>();
			profile.CreateMap<WebApi.Contracts.RequestModels.ApplicationCreateRequestModel, Dfe.Academies.Academisation.IService.RequestModels.ApplicationCreateRequestModel>();
			profile.CreateMap<WebApi.Contracts.RequestModels.ContributorRequestModel, Dfe.Academies.Academisation.IService.RequestModels.ContributorRequestModel>();
		}
	}
}
