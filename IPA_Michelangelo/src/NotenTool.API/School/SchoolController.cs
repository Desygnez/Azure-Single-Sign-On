using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NotenTool.API.School;

[ApiController]
[Route("/[controller]")]
[ApiVersion("1")]
[Authorize]
public class SchoolController : ControllerBase
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly ISchoolValidator _schoolValidator;
    private readonly ISchoolAuthorizationValidator _authorizationValidator;

    public SchoolController(ISchoolRepository schoolRepository, ISchoolValidator schoolValidator,
        ISchoolAuthorizationValidator authorizationValidator)
    {
        _schoolRepository = schoolRepository;
        _schoolValidator = schoolValidator;
        _authorizationValidator = authorizationValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DbSchool>>> GetAllSchools()
    {
        var resp = await _schoolRepository.GetAllSchools();
        return resp == null ? NotFound("There are no Schools in the database") : Ok(resp);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DbSchool>> GetSchoolById(Guid id)
    {
        var resp = await _schoolRepository.GetSchoolById(id);

        return resp == null ? NotFound($"There is no School with the id = ${id} in the database") : Ok(resp);
    }

    [HttpPost]
    public async Task<ActionResult<DbSchool>> CreateSchool(DbSchool dbSchool)
    {
        if (!_authorizationValidator.CanUserModifySchool())
            return Unauthorized();

        var school = dbSchool with { Id = Guid.NewGuid() };

        await _schoolRepository.CreateSchool(school);

        return Ok(school);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateSchool(DbSchool dbSchool)
    {
        if (!_authorizationValidator.CanUserModifySchool())
            return Unauthorized();

        if (!_schoolValidator.IsSchoolValid(dbSchool.Id))
            return new NotFoundResult();

        await _schoolRepository.UpdateSchool(dbSchool);

        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteSchool(Guid id)
    {
        if (!_authorizationValidator.CanUserModifySchool())
            return Unauthorized();

        if (!_schoolValidator.IsSchoolValid(id))
            return new NotFoundResult();

        _schoolRepository.DeleteSchool(id);
        return new NoContentResult();
    }
}