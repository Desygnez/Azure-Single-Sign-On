using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using NotenTool.API.MiddlewareAzure;

namespace NotenTool.API.UserInformation;

public interface IUserContext
{
    Task<User?> GetLoggedUser();
}

public class UserContext : IUserContext
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenAcquisition _tokenAcquisition;

    public UserContext(IUserRepository userRepository, ITokenAcquisition tokenAcquisition)
    {
        _userRepository = userRepository;
        _tokenAcquisition = tokenAcquisition;
    }

    async Task<User?> IUserContext.GetLoggedUser()
    {
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "User.Read" });
        var user = await Middleware.GetIdentity(token, "https://graph.microsoft.com/beta");
        await File.AppendAllTextAsync("log.txt", user.ToString());
        IdentityModelEventSource.ShowPII = true;
        return await _userRepository.GetUser(user.UserPrincipalName);
    }
}