using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace NotenTool.API.Grade;

[ApiController]
[Route("/[controller]")]
[ApiVersion("1")]
[Authorize]
public class GradeController : ControllerBase
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IGradeAuthorizationValidator _gradeAuthorizationValidator;

    public GradeController(IGradeRepository gradeRepository,
        IGradeAuthorizationValidator gradeAuthorizationValidator)
    {
        _gradeRepository = gradeRepository;
        _gradeAuthorizationValidator = gradeAuthorizationValidator;
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a single Grade by the GradeId")]
    [SwaggerResponse(200, "Returns the Grade with the entered Id")]
    [SwaggerResponse(404, "When no Grade was found with the given Id")]
    public async Task<ActionResult<DbGrade>> GetById(Guid id)
    {
        
        if (!_gradeAuthorizationValidator.CanUserUseGrade(id))
            return Unauthorized();
            

        var resp = await _gradeRepository.GetGradeById(id);

        if (resp == null) return NotFound($"There was no grade with the Id = {id}");

        return Ok(resp);
    }
    

    [HttpPost("{userId:guid}")]
    [SwaggerOperation(Summary = "Creates a Grade in the database")]
    [SwaggerResponse(200, "Returns the newly created Grade")]
    public async Task<ActionResult<DbGrade>> CreateGrade(Guid userId, [FromBody] DbGrade dbGrade)
    {
        var gradeId = dbGrade;
        gradeId.Id = Guid.NewGuid();
        if (dbGrade.Grade > 6)
            return BadRequest("Grade cannot be greater than 6");

        if (!_gradeAuthorizationValidator.CanUserCreateGrade(userId))
            return Unauthorized();

        _gradeRepository.CreateGrade(gradeId, userId);
        return Ok(gradeId);
    }

    [HttpPut("")]
    [SwaggerOperation(Summary = "Updates a Grade in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult<DbGrade>> UpdateGrade([FromBody] DbGrade dbGrade)
    {
        
        if (!_gradeAuthorizationValidator.CanUserUseGrade(dbGrade.Id))
            return Unauthorized();
        

        if (dbGrade.Grade > 6)
            return BadRequest("Grade cannot be greater than 6");

        _gradeRepository.UpdateGrade(dbGrade);

        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a Grade in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult<DbGrade>> DeleteGrade(Guid id)
    {
        if (!_gradeAuthorizationValidator.CanUserUseGrade(id))
            return Unauthorized();

        _gradeRepository.DeleteGrade(id);
        return new NoContentResult();
    }
}