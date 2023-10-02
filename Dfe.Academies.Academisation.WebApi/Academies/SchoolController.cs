//using Dfe.Academies.Academisation.Data.Academies;
//using Dfe.Academies.Academisation.Domain.Academies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Dfe.Academies.Academisation.WebApi.Academies;

//[ApiController]
//[Route("academies/school")]
//public class SchoolController
//{
//	private readonly AcademiesContext _dbContext;

//	public SchoolController(AcademiesContext dbContext)
//	{
//		_dbContext = dbContext;
//	}

//	[HttpGet("{id}")]
//	public async Task<List<School>> GetSchoolByLocalAuthorityId(int id)
//	{
//		return await _dbContext.Schools.Where(x => x.LocalAuthorityId == id).ToListAsync();
//	}
//}
