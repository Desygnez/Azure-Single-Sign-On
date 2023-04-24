using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace NotenTool.API.Subject;

[ApiController]
[Route("/[controller]")]
[ApiVersion("1")]
[Authorize]
public class SubjectController : ControllerBase
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ISubjectValidator _validator;
    private readonly ISubjectAuthorzationValidator _subjectAuthorzationValidator;

    public SubjectController(ISubjectRepository subjectRepository, ISubjectValidator validator, ISubjectAuthorzationValidator subjectAuthorzationValidator)
    {
        _subjectRepository = subjectRepository;
        _validator = validator;
        _subjectAuthorzationValidator = subjectAuthorzationValidator;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all Subjects from the database")]
    [SwaggerResponse(200, "Returns all Subjects")]
    [SwaggerResponse(404, "When no Subjects were found in the database")]
    public async Task<ActionResult<List<DbSubject>>> GetAllSubjects()
    {
        var resp = await _subjectRepository.GetAllSubjects();
        if (resp == null) return NotFound("There are no Subjects in the database");

        return Ok(resp);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Returns a Subject from the database with the given Id")]
    [SwaggerResponse(200, "Returns the Subject with the given Id")]
    [SwaggerResponse(404, "When no Subject with the given Id was found in the database")]
    public async Task<ActionResult<DbSubject>> GetById(Guid id)
    {
        var resp = await _subjectRepository.GetSubjectById(id);

        if (resp == null) return NotFound($"There was no subject with the Id = {id}");

        return Ok(resp);
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates a Subject in the database")]
    [SwaggerResponse(200, "Returns the newly created Subject")]
    public async Task<ActionResult> CreateSubject(DbSubject dbSubject)
    {
        if (!_subjectAuthorzationValidator.CanUserModifySubject())
            return Unauthorized();

        var subject = dbSubject with { Id = Guid.NewGuid() };

        await _subjectRepository.CreateSubject(subject);
        return Ok(subject);
    }

    [HttpPut("")]
    [SwaggerOperation(Summary = "Updates a Subject in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult> UpdateSubject(DbSubject dbSubject)
    {
        if (!_subjectAuthorzationValidator.CanUserModifySubject())
            return Unauthorized();

        if (!_validator.IsSubjectValid(dbSubject.Id))
            return new NotFoundResult();

        await _subjectRepository.UpdateSubject(dbSubject);
        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a Subject in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult> DeleteSubject(Guid id)
    {
        if (!_subjectAuthorzationValidator.CanUserModifySubject())
            return Unauthorized();

        if (!_validator.IsSubjectValid(id))
            return new NotFoundResult();
        await _subjectRepository.DeleteSubject(id);
        return new NoContentResult();
    }
}