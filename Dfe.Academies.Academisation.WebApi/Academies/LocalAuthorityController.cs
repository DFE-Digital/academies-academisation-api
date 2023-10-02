//using Dfe.Academies.Academisation.Data.Academies;
//using Dfe.Academies.Academisation.Domain.Academies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Dfe.Academies.Academisation.WebApi.Academies;

//[ApiController]
//[Route("academies/local-authority")]
//public class LocalAuthorityController
//{
//	private readonly AcademiesContext _dbContext;

//	public LocalAuthorityController(AcademiesContext dbContext)
//	{
//		_dbContext = dbContext;
//	}

//	[HttpGet]
//	public async Task<ActionResult<List<LocalAuthority>>> GetAll()
//	{
//		return await _dbContext.LocalAuthorities.ToListAsync();
//	}
//}
