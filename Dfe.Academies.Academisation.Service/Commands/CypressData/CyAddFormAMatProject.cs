using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	/// The cy add form a mat project.
	/// </summary>
	public class CyAddFormAMatProject : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		public override IEnumerable<(string, object[])> SqlStatements =>
			new (string, object[])[]
			{
				(@"UPDATE [academisation].[ConversionApplication]
				SET [academisation].[ConversionApplication].ApplicationStatus = 'InProgress'
				SELECT TOP (1000) [Id]
					  ,[ApplicationType]
					  ,[CreatedOn]
					  ,[LastModifiedOn]
					  ,[ApplicationStatus]
					  ,[FormTrustId]
					  ,[JoinTrustId]
					  ,[ApplicationSubmittedDate]
					  ,[DynamicsApplicationId]
					  ,[ApplicationReference]
				  FROM [academisation].[ConversionApplication]
				  WHERE [academisation].[ConversionApplication].ApplicationReference = 'A2B_31'", Array.Empty<object>())
			};

		public override bool HasValidArguments => true;
	}

	public class CyAddFormAMatProjectCommandHandler : CypressDataBaseCommandHandlerAbstractBase, IRequestHandler<CyAddFormAMatProject, CommandResult>
	{
		public Task<CommandResult> Handle(CyAddFormAMatProject request, CancellationToken cancellationToken) => Handle(request as CypressDataCommandAbstractBase, cancellationToken);

		public CyAddFormAMatProjectCommandHandler(AcademisationContext dbContext) : base(dbContext)
		{
		}
	}
}
