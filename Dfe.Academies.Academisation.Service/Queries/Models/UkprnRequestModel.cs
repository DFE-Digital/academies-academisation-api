using System.Collections.Generic;

namespace Dfe.Academies.Academisation.Service.Queries.Models
{
    public class UkprnRequestModel
    {
        public required IEnumerable<string> Ukprns { get; set; }
    }
}
