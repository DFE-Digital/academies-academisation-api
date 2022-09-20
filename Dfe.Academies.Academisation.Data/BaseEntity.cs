using System.ComponentModel.DataAnnotations;

namespace Dfe.Academies.Academisation.Data;

public class BaseEntity
{
	[Key]
	public int Id { get; set; }
	//public DateTime CreatedOn { get; set; }
	//public DateTime LastModifiedOn { get; set; }
}
