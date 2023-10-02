//using Dfe.Academies.Academisation.Data.Academies;
//using Dfe.Academies.Academisation.Domain.Academies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Dfe.Academies.Academisation.WebApi.Academies;

//[ApiController]
//[Route("academies/trust")]
//public class TrustController
//{
//	private readonly AcademiesContext _dbContext;

//	public TrustController(AcademiesContext dbContext)
//	{
//		_dbContext = dbContext;
//	}

//	[HttpGet("")]
//	public async Task<ActionResult<List<Trust>>> GetTrustsByRegion(int Id)
//	{
//		return (await _dbContext.Trusts.Where(x => x.RegionId == Id).ToListAsync());
//	}
//}
