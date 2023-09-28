using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate
{
	public interface IProjectNote
	{

		 int ProjectId { get; }
		 int Id { get;  }
		 string? Subject { get;   }
		 string? Note { get;   }
		 string? Author { get;   }
		 DateTime? Date { get;   }
	}
}
