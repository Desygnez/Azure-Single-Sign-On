using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using NotenTool.API.MiddlewareAzure;
using Swashbuckle.AspNetCore.Annotations;

namespace NotenTool.API.UserInformation;

[ApiController]
[Route("/[controller]")]
[ApiVersion("1")]
[Authorize]
public class UserInformationController : ControllerBase
{
    private readonly IUserInformationRepository _userInformationRepository;
    private readonly IUserInformationValidator _userInformationValidator;
    private readonly IUserInformationAuthorizationValidator _authorizationValidator;
    private readonly IUserContext _userContext;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly IUserRepository _userRepository;

    public UserInformationController(IUserInformationRepository userInformationRepository,
        IUserInformationValidator userInformationValidator,
        IUserInformationAuthorizationValidator authorizationValidator, IUserContext userContext,
        ITokenAcquisition tokenAcquisition, IUserRepository userRepository)
    {
        _userInformationRepository = userInformationRepository;
        _userInformationValidator = userInformationValidator;
        _authorizationValidator = authorizationValidator;
        _userContext = userContext;
        _tokenAcquisition = tokenAcquisition;
        _userRepository = userRepository;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all UserInformation from the database")]
    [SwaggerResponse(200, "Returns all UserInformation")]
    [SwaggerResponse(404, "When no UserInformation were found in the database")]
    public async Task<ActionResult<List<DbUserInformation>>> GetAllUser()
    {
        if (!_authorizationValidator.CanUserGetAllInformation())
            return Unauthorized();

        var resp = await _userInformationRepository.GetAllUsers();
        if (resp == null) return NotFound("There are no Users in the database");

        return Ok(resp);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Returns a UserInformation from the database with the given Id")]
    [SwaggerResponse(200, "Returns the UserInformation item with the given Id")]
    [SwaggerResponse(404, "When the UserInformation item was not found in the database")]
    public async Task<ActionResult<DbUserInformation>> GetUsersById(Guid id)
    {
        if (!_authorizationValidator.CanUserGetInformation(id))
            return Unauthorized();

        if (!_userInformationValidator.IsUserInformationValid(id))
            return new NotFoundResult();

        var resp = _userInformationRepository.GetUserById(id);

        return Ok(resp);
    }

    [HttpGet("GetByUsername")]
    [SwaggerOperation(Summary = "Returns a UserInformation from the database with the username")]
    [SwaggerResponse(200, "Returns the UserInformation item with the username")]
    [SwaggerResponse(404, "When the UserInformation item was not found in the database")]
    public async Task<ActionResult<DbUserInformation>> GetByUsername(string username)
    {
        if (!_authorizationValidator.CanUserGetInformationByUsername(username))
            return Unauthorized();
        if (!_userInformationValidator.IsUserInformationValidByUsername(username))
            return new NotFoundResult();

        var resp = _userInformationRepository.GetUserByUsername(username);
        return Ok(resp);
    }


    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates a UserInformation item in the database")]
    [SwaggerResponse(200, "Returns the newly created UserInformation item")]
    public async Task<ActionResult> CreateUser(DbUserInformation dbUserInformation)
    {
        if (!_authorizationValidator.IsVocational())
            return Unauthorized();
        var userInformationId = dbUserInformation;
        userInformationId.Id = Guid.NewGuid();
        _userInformationRepository.CreateUser(userInformationId);
        return Ok(userInformationId);
    }

    [HttpPost("trainerlist")]
    [SwaggerOperation(Summary = "Creates new TrainerApprentice Entry")]
    [SwaggerResponse(200, "Returns the newly created TrainerApprentice Entry was successfull")]
    [SwaggerResponse(401, "Returns when the user was not authorized correctly")]
    public async Task<ActionResult> CreateTrainerApprentice(DbTrainerApprentice dbTrainerApprentice)
    {
        if (!_userInformationValidator.IsUserInformationValid(dbTrainerApprentice.ApprenticeId) ||
            !_userInformationValidator.IsUserInformationValid(dbTrainerApprentice.TrainerId))
            return NotFound();

        if (!_authorizationValidator.IsVocational())
            return Unauthorized();

        var trainerApprentice = dbTrainerApprentice with { Id = Guid.NewGuid() };

        await _userInformationRepository.CreateTrainerApprentice(trainerApprentice);
        return Ok(trainerApprentice);
    }

    [HttpPut("")]
    [SwaggerOperation(Summary = "Updates a UserInformation item in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult> UpdateUser(DbUserInformation dbUserInformation)
    {
        if (!_authorizationValidator.IsVocational())
            return Unauthorized();
        if (!_userInformationValidator.IsUserInformationValid(dbUserInformation.Id))
            return new NotFoundResult();

        _userInformationRepository.UpdateUser(dbUserInformation);
        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a UserInformation item in the database")]
    [SwaggerResponse(204, "Returns no content")]
    public async Task<ActionResult> DeleteUser(Guid guid)
    {
        if (!_authorizationValidator.IsVocational())
            return Unauthorized();

        if (!_userInformationValidator.IsUserInformationValid(guid))
            return new NotFoundResult();

        _userInformationRepository.DeleteUser(guid);
        return new NoContentResult();
    }

    [HttpGet("currentUser")]
    public async Task<ActionResult<User>> CurrentUser()
    {
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "User.Read" });
        var identity = await Middleware.GetIdentity(token, "https://graph.microsoft.com/beta");
        Console.WriteLine(identity);

        var user = new User
        {
            Firstname = identity.DisplayName,
            Id = _userRepository.GetUser(identity.UserPrincipalName).Result!.Id
        };

        return Ok(user);
    }

    [HttpGet("apprentices")]
    public async Task<ActionResult<User>> GetApprentices()
    {
        var loggedUser = await _userContext.GetLoggedUser();
        if (_authorizationValidator.IsVocational())
            return Ok(await _userInformationRepository.GetAllUsers());
        if (_authorizationValidator.IsTrainer())
            return Ok(await _userInformationRepository.GetUserByIdList(loggedUser.Apprentices));

        return Unauthorized();
    }
}