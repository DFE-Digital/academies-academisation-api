using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
    public class SetSfsoCommissioningCommand : IRequest<CommandResult>
    {
        public SetSfsoCommissioningCommand(int id, string? sfsoCommissioningOverview)
        {
            Id = id;
            SfsoCommissioningOverview = sfsoCommissioningOverview;
        }

        public int Id { get; set; }
        public string? SfsoCommissioningOverview { get; set; }
    }
}