using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace NotenTool.API.UserSemester;

[ApiController]
[ApiVersion("1")]
[Route("/[controller]")]
[Authorize]
public class UserSemesterController : ControllerBase
{
    private readonly IUserSemesterRepository _userSemesterRepository;
    private readonly IUserSemesterValidator _userSemesterValidator;
    private readonly IUserSemesterAuthorizationValidator _userSemesterAuthorizationValidator;

    public UserSemesterController(IUserSemesterRepository userSemesterRepository,
        IUserSemesterValidator userSemesterValidator,
        IUserSemesterAuthorizationValidator userSemesterAuthorizationValidator)
    {
        _userSemesterRepository = userSemesterRepository;
        _userSemesterValidator = userSemesterValidator;
        _userSemesterAuthorizationValidator = userSemesterAuthorizationValidator;
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a UserSemester with its Id")]
    [SwaggerResponse(200, "Returns the UserSemester item with the given Id")]
    [SwaggerResponse(404, "When no UserSemester item was found with the given Id")]
    public async Task<ActionResult<DbUserSemester>> GetById(Guid id)
    {
        if (!_userSemesterValidator.IsUserSemesterValid(id))
            return NotFound();
        if (!_userSemesterAuthorizationValidator.CanUserGetUserSemester(id))
            return Unauthorized();

        var resp = await _userSemesterRepository.GetUserSemesterById(id);
        return Ok(resp);
    }

    [HttpGet("GetByUserId/{id}")]
    [SwaggerOperation(Summary = "Gets a UserSemester with its Id")]
    [SwaggerResponse(200, "Returns the UserSemester item with the given User Id")]
    [SwaggerResponse(404, "When no UserSemester item was found with the given User Id")]
    public async Task<ActionResult<List<DbUserSemester>>> GetByUserId(Guid id)
    {
        if (!_userSemesterValidator.IsUserSemesterValidByUserId(id))
            return NotFound();
        if (!_userSemesterAuthorizationValidator.CanUserGetUserSemesterByUserId(id))
            return Unauthorized();
        var resp = await _userSemesterRepository.GetUserSemesterByUserId(id);

        return Ok(resp);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a UserSemester Dataset in the database")]
    [SwaggerResponse(200, "Returns the created UserSemester Dataset")]
    public async Task<ActionResult<DbUserSemester>> CreateUserSemester(DbUserSemester dbUserSemester)
    {
        if (!_userSemesterAuthorizationValidator.CanUserModify(dbUserSemester.UserInfo_id))
            return Unauthorized();
        if (!_userSemesterValidator.DoesNotAlreadyExistByUserId(dbUserSemester.UserInfo_id, dbUserSemester.Semester_id))
            return Conflict();


        dbUserSemester.Id = Guid.NewGuid();
        await _userSemesterRepository.CreateUserSemester(dbUserSemester);
        return Ok(dbUserSemester);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Updates a UserSemester item")]
    [SwaggerResponse(204, "Returns the Updated UserSemester item")]
    public async Task<ActionResult<DbUserSemester>> UpdateUserSemester(DbUserSemester dbUserSemester)
    {
        if (!_userSemesterValidator.IsUserSemesterValid(dbUserSemester.Id))
            return new NotFoundResult();
        if (!_userSemesterAuthorizationValidator.CanUserModify(dbUserSemester.UserInfo_id))
            return Unauthorized();

        await _userSemesterRepository.UpdateUserSemester(dbUserSemester);

        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a UserSemester item")]
    [SwaggerResponse(204, "Returns no content")]
    public ActionResult DeleteUserSemester(Guid id)
    {
        if (!_userSemesterValidator.IsUserSemesterValid(id))
            return new NotFoundResult();
        if (!_userSemesterAuthorizationValidator.CanUserUpdate(id))
            return Unauthorized();

        _userSemesterRepository.DeleteUserSemester(id);
        return new NoContentResult();
    }
}