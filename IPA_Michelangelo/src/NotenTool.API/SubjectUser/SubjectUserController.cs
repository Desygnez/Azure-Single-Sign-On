using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace NotenTool.API.SubjectUser;

[ApiController]
[Route("/[controller]")]
[ApiVersion("1")]
[Authorize]
public class SubjectUserController : ControllerBase
{
    private readonly ISubjectUserRepository _subjectRepository;
    private readonly ISubjectUserValidator _subjectUserValidator;
    private readonly ISubjectUserAuthorizationValidator _subjectUserAuthorizationValidator;

    public SubjectUserController(ISubjectUserRepository subjectUserRepository,
        ISubjectUserValidator subjectUserValidator,
        ISubjectUserAuthorizationValidator subjectUserAuthorizationValidator)
    {
        _subjectRepository = subjectUserRepository;
        _subjectUserValidator = subjectUserValidator;
        _subjectUserAuthorizationValidator = subjectUserAuthorizationValidator;
    }


    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Returns a SubjectUser in the database with the given Id")]
    [SwaggerResponse(200, "Returns the SubjectUser with the given Id")]
    [SwaggerResponse(404, "When there is no Dataset with the given Id")]
    public async Task<ActionResult<DbSubjectUser>> GetById(Guid id)
    {
        if (!_subjectUserValidator.IsSubjectUserValid(id))
            return NotFound($"There was no Subject user with the Id = {id}");

        if (!_subjectUserAuthorizationValidator.CanUserGetSubjectUser(id))
            return Unauthorized();

        var resp = await _subjectRepository.GetSubjectById(id);
        return Ok(resp);
    }

    [HttpGet("GetByUserId/{id}")]
    [SwaggerOperation(Summary = "Returns a SubjectUser in the database with the given UserId")]
    [SwaggerResponse(200, "Returns the SubjectUser with the given UserId")]
    [SwaggerResponse(404, "When there is no Dataset with the given UserId")]
    public async Task<ActionResult<IEnumerable<DbSubjectUser>>> GetByUserId(Guid id)
    {
        if (!_subjectUserValidator.IsSubjectUserValidByUserId(id))
        {
            return NotFound();
        }

        if (!_subjectUserAuthorizationValidator.CanUserGetWithUserId(id))
            return Unauthorized();

        var resp = await _subjectRepository.GetSubjectsByUserId(id);

        if (!resp.Any()) return NotFound($"There was no Subject user with the Id = {id}");

        return Ok(resp);
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates a SubjectUser in the database")]
    [SwaggerResponse(200, "Returns the newly created SubjectUser")]
    public async Task<ActionResult> CreateSubjectUser(DbSubjectUser dbSubjectUser)
    {
        if (!_subjectUserAuthorizationValidator.CanUserModifySubjectUserByUserId(dbSubjectUser.UserInfo_id))
            return Unauthorized();
        if (!_subjectUserValidator.DoesNotAlreadyExistByUserId(dbSubjectUser.UserInfo_id, dbSubjectUser.Subject_id, dbSubjectUser.Semester_id))
            return Conflict();

        var subjectUserId = dbSubjectUser;
        subjectUserId.Id = Guid.NewGuid();

        _subjectRepository.CreateSubjectUser(subjectUserId);
        return Ok(subjectUserId);
    }

    [HttpPut("")]
    [SwaggerOperation(Summary = "Updates a SubjectUser in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult> UpdateSubjectUser(DbSubjectUser dbSubjectUser)
    {
        if (!_subjectUserAuthorizationValidator.CanUserModifySubjectUserByUserId(dbSubjectUser.UserInfo_id))
            return Unauthorized();

        if (!_subjectUserValidator.IsSubjectUserValid(dbSubjectUser.Id))
            return new NotFoundResult();

        _subjectRepository.UpdateSubjectUser(dbSubjectUser);
        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a SubjectUser in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult> DeleteSubjectUser(Guid id)
    {
        if (!_subjectUserValidator.IsSubjectUserValid(id))
            return new NotFoundResult();

        if (!_subjectUserAuthorizationValidator.CanUserModifySubjectUser(id))
            return Unauthorized();

        _subjectRepository.DeleteSubjectUser(id);
        return new NoContentResult();
    }
}