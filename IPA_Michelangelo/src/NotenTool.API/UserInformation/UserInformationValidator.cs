namespace NotenTool.API.UserInformation;

public interface IUserInformationValidator
{
    bool IsUserInformationValid(Guid id);
    bool IsUserInformationValidByUsername(string username);
}

public class UserInformationValidator : IUserInformationValidator
{
    private readonly IUserInformationRepository _userInformationRepository;

    public UserInformationValidator(IUserInformationRepository userInformationRepository)
    {
        _userInformationRepository = userInformationRepository;
    }

    bool IUserInformationValidator.IsUserInformationValid(Guid id)
    {
        var user = _userInformationRepository.GetUserById(id);
        if (user != null)
            return true;
        return false;
    }

    bool IUserInformationValidator.IsUserInformationValidByUsername(string username)
    {
        var user = _userInformationRepository.GetUserByUsername(username);
        if (user != null)
            return true;
        return false;
    }
}