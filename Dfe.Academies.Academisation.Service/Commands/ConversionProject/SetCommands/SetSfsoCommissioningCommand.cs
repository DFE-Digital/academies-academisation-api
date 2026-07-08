using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
    public class SetSfsoCommissioningCommand : IRequest<CommandResult>
    {
        public SetSfsoCommissioningCommand(int id, string? sfsoCommissioningOverview, bool? sfsoCommissioningSectionComplete)
        {
            Id = id;
            SfsoCommissioningOverview = sfsoCommissioningOverview;
            SfsoCommissioningSectionComplete = sfsoCommissioningSectionComplete;
        }

        public int Id { get; set; }
        public string? SfsoCommissioningOverview { get; set; }
        public bool? SfsoCommissioningSectionComplete { get; set; }
    }
}