using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	public class CyCommentsUpdatedCorrectly : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		public override IEnumerable<(string, object[])> SqlStatements => new (string, object[])[]
		{
			(@"update academisation.Project set 
			LocalAuthorityInformationTemplateReturnedDate = '2023-01-01', 
			LocalAuthorityInformationTemplateSentDate = '2023-01-01'                             
			where Id = {0}", new object[]{ this.Id})
		};

		public override bool HasValidArguments => !string.IsNullOrWhiteSpace(this.Id);
		public string Id { get; set; }
	}
}
