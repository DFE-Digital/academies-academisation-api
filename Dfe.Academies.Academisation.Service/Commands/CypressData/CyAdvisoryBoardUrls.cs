using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	/// The cy advisory board urls.
	/// </summary>
	public class CyAdvisoryBoardUrls : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		/// xes the.
		/// </summary>
		private void x()
		{

		}

		/// <summary>
		/// Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new (string, object[])[]
		{
			(@"update academisation.Project 
			set LocalAuthorityInformationTemplateReturnedDate = '2023-01-01',
			LocalAuthorityInformationTemplateSentDate = '2023-01-01',
			HeadTeacherBoardDate = '2024-01-01'
			where Id = {0}", new object[] { Id! })
		};


		/// <summary>
		/// Gets a value indicating whether command has valid arguments.
		/// </summary>
		public override bool HasValidArguments => !string.IsNullOrWhiteSpace(Id);

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		public string? Id { get; set; }
	}
}
