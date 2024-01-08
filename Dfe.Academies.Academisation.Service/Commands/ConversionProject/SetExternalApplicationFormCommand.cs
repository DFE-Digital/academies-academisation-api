using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class SetExternalApplicationFormCommand : IRequest<CommandResult>
	{
		public SetExternalApplicationFormCommand(int id, bool ExternalApplicationFormSaved,
			string ExternalApplicationFormUrl)
		{
			Id = id;
			this.ExternalApplicationFormSaved = ExternalApplicationFormSaved;
			this.ExternalApplicationFormUrl = ExternalApplicationFormUrl;
		}

		public int Id { get; set; }
		public bool ExternalApplicationFormSaved { get; set; }
		public string ExternalApplicationFormUrl { get; set; }
	}
}
