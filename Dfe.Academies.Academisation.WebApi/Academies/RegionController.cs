//using Dfe.Academies.Academisation.Data.Academies;
//using Dfe.Academies.Academisation.Domain.Academies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Dfe.Academies.Academisation.WebApi.Academies;

//[ApiController]
//[Route("academies/region")]
//public class RegionController
//{
//	private readonly AcademiesContext _dbContext;

//	public RegionController(AcademiesContext dbContext)
//	{
//		_dbContext = dbContext;
//	}

//	[HttpGet]
//	public async Task<ActionResult<List<Region>>> GetAll()
//	{
//		return await _dbContext.Regions.ToListAsync();
//	}
//}
