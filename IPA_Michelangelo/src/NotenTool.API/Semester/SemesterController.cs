using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NotenTool.API.Semester;

[ApiController]
[Route("/[controller]")]
[ApiVersion("1")]
[Authorize]
public class SemesterController : ControllerBase
{
    private readonly ISemesterRepository _semesterRepository;
    private readonly ISemesterValidator _semesterValidator;
    private readonly ISemesterAuthorizationValidator _semesterAuthorizationValidator;

    public SemesterController(ISemesterRepository semesterRepository, ISemesterValidator semesterValidator,
        ISemesterAuthorizationValidator semesterAuthorizationValidator)
    {
        _semesterRepository = semesterRepository;
        _semesterValidator = semesterValidator;
        _semesterAuthorizationValidator = semesterAuthorizationValidator;
    }

    [HttpGet("")]
    public async Task<ActionResult<List<DbSemester>>> GetAllSemesters()
    {
        var resp = await _semesterRepository.GetAllSemester();
        if (resp == null) return NotFound("There are no Semesters in the database");

        return Ok(resp);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DbSemester>> GetById(Guid id)
    {
        var resp = await _semesterRepository.GetSemesterById(id);

        if (resp == null) return NotFound($"There was no semester with the Id = {id}");

        return Ok(resp);
    }

    [HttpPost("")]
    public async Task<ActionResult> CreateSemester(DbSemester dbSemester)
    {
        if (!_semesterAuthorizationValidator.CanUserModifySemester())
            return Unauthorized();

        var semester = dbSemester with { Id = Guid.NewGuid() };

        await _semesterRepository.CreateSemester(semester);

        return Ok(semester);
    }

    [HttpPut("")]
    public async Task<ActionResult> UpdateSemester(DbSemester dbSemester)
    {
        if (!_semesterAuthorizationValidator.CanUserModifySemester())
            return Unauthorized();

        if (!_semesterValidator.IsSemesterValid(dbSemester.Id))
        {
            return new NotFoundResult();
        }

        await _semesterRepository.UpdateSemester(dbSemester);
        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSemester(Guid id)
    {
        if (!_semesterAuthorizationValidator.CanUserModifySemester())
            return Unauthorized();

        if (!_semesterValidator.IsSemesterValid(id))
        {
            return new NotFoundResult();
        }

        await _semesterRepository.DeleteSemester(id);
        return new NoContentResult();
    }
}