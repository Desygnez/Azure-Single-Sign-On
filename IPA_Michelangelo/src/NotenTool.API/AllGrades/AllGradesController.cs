using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotenTool.API.Classes;
using Swashbuckle.AspNetCore.Annotations;

namespace NotenTool.API.AllGrades;

[ApiController]
[Route("/[controller]")]
[ApiVersion("1")]
[Authorize]
public class AllGradesController : ControllerBase
{
    private readonly IAllGradesRepository _allGradeRepo;
    private readonly IAllGradesAuthoriationValidator _allGradesAuthoriationValidator;

    public AllGradesController(IAllGradesRepository allGradesRepository,
        IAllGradesAuthoriationValidator allGradesAuthoriationValidator)
    {
        _allGradeRepo = allGradesRepository;
        _allGradesAuthoriationValidator = allGradesAuthoriationValidator;
    }


    [HttpGet("")]
    [SwaggerOperation(Summary = "Return all grades from the Gradesheets")]
    [SwaggerResponse(200, "Returns all grades in the Gradesheet")]
    [SwaggerResponse(404, "When the given Id does not exist in the database")]
    public async Task<ActionResult<List<DbAllGrades>>> GetAllGrades()
    {
        if (!_allGradesAuthoriationValidator.IsVocational())
            return Unauthorized();

        var resp = await _allGradeRepo.GetAllGrades();
        if (resp == null) return NotFound("There are no Grades in the database");

        return Ok(resp);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Returns a single Grade by the Gradesheet item ID")]
    [SwaggerResponse(200, "Returns the Gradesheet item")]
    [SwaggerResponse(404, "The given Id does not exist in the database")]
    public async Task<ActionResult<DbAllGrades>> GetById(Guid id)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeById(id))
            return Unauthorized();

        var resp = await _allGradeRepo.GetGradeById(id);

        if (resp == null) return NotFound($"There was no grade with the Id = {id}");

        return Ok(resp);
    }

    [HttpGet("GetByUserId/{id}")]
    [SwaggerOperation(Summary = "Returns a single Grade by the UserId")]
    [SwaggerResponse(200, "Returns the Gradesheet item")]
    [SwaggerResponse(404, "The given UserId does not exist in the database")]
    public async Task<ActionResult<DbAllGrades>> GetByUserId(Guid id)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeByUserId(id))
            return Unauthorized();
        var resp = await _allGradeRepo.GetByUserId(id);

        if (resp == null) return NotFound($"There was no grade by the user with the Id = {id}");

        return Ok(resp);
    }

    [HttpGet("GetByGradeId/{id}")]
    [SwaggerOperation(Summary = "Returns a single Grade by the GradeId")]
    [SwaggerResponse(200, "Returns the Gradesheet item")]
    [SwaggerResponse(404, "The given GradeId does not exist in the database")]
    public async Task<ActionResult<DbAllGrades>> GetByGradeId(Guid id)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeByGradeId(id))
            return Unauthorized();
        var resp = await _allGradeRepo.GetByGradeId(id);

        if (resp == null) return NotFound($"There was no grade with the Grade_id = {id}");

        return Ok(resp);
    }

    [HttpPost("GetByUserAndSubjectId")]
    [SwaggerOperation(Summary = "Returns all Grades, with the given UserId and SubjectId")]
    [SwaggerResponse(200, "Returns the Gradesheet items")]
    [SwaggerResponse(404, "The combination of Id's given could not be found")]
    public async Task<ActionResult<DbAllGrades>> GetByUserAndSubjectId([FromBody] UserAndSubjectIDModel model)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeByUserId(model.userId))
            return Unauthorized();

        var resp = await _allGradeRepo.GetByUserIdAndSubjectId(model.userId, model.subjectId);

        if (resp == null)
            return NotFound(
                $"There was no grade by the user with the userId = {model.userId} and subjectId = {model.subjectId}");

        return Ok(resp);
    }


    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates a GradeSheet item")]
    [SwaggerResponse(200, "Returns the newly provided GradeSheet item")]
    public async Task<ActionResult> CreateGrade(DbAllGrades dbAllGrades)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeByUserId(dbAllGrades.UserInfo_id))
            return Unauthorized();

        var gradeId = dbAllGrades;
        gradeId.Id = Guid.NewGuid();

        _allGradeRepo.CreateGradeAsync(gradeId);
        return Ok(gradeId);
    }

    [HttpPut("")]
    [SwaggerOperation(Summary = "Updates a GradeSheet item")]
    [SwaggerResponse(204, "Returns no Content")]
    public async Task<ActionResult> UpdateGrade(DbAllGrades dbGrade)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeByUserId(dbGrade.UserInfo_id))
            return Unauthorized();

        _allGradeRepo.UpdateGrade(dbGrade);
        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a GradeSheet item")]
    [SwaggerResponse(204, "Returns no Content")]
    public async Task<ActionResult> DeleteGrade(Guid id)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeById(id))
            return Unauthorized();

        _allGradeRepo.DeleteGradeAsync(id);
        return new NoContentResult();
    }

    [HttpDelete("ByGrade/{id}")]
    [SwaggerOperation(Summary = "Deletes a GradeSheet item")]
    [SwaggerResponse(204, "Returns no Content")]
    public async Task<ActionResult> DeleteGradeByGradeId(Guid id)
    {
        if (!_allGradesAuthoriationValidator.CanUserUseGradeByGradeId(id))
            return Unauthorized();

        _allGradeRepo.DeleteGradeByGradeId(id);
        return new NoContentResult();
    }
}