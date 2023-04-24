namespace NotenTool.API.UserInformation;

public interface IUserInformationAuthorizationValidator
{
    bool CanUserGetInformation(Guid userId);
    bool CanUserGetAllInformation();
    bool CanUserGetInformationByUsername(string username);
    bool IsVocational();
    bool IsTrainer();
}

public class UserInformationAuthorizationValidator : IUserInformationAuthorizationValidator
{
    private readonly IUserContext _userContext;
    private readonly IUserInformationRepository _userInformationRepository;

    public UserInformationAuthorizationValidator(IUserContext userContext,
        IUserInformationRepository userInformationRepository)
    {
        _userContext = userContext;
        _userInformationRepository = userInformationRepository;
    }

    bool IUserInformationAuthorizationValidator.CanUserGetInformation(Guid userId)
    {
        var current = _userContext.GetLoggedUser().Result;
        var dbUserInformation = _userInformationRepository.GetUserById(userId).Result;

        if (current != null && (dbUserInformation.Id == current.Id || current.IsVocationalTrainer ||
                                current.Apprentices.Contains(current.Id)))
            return true;

        return false;
    }

    bool IUserInformationAuthorizationValidator.CanUserGetAllInformation()
    {
        var current = _userContext.GetLoggedUser().Result;

        if (current != null && current.IsVocationalTrainer)
            return true;

        return false;
    }

    public bool CanUserGetInformationByUsername(string username)
    {
        var current = _userContext.GetLoggedUser().Result;
        var dbUserInformation = _userInformationRepository.GetUserByUsername(username).Result;

        if (current != null && (current.Id == dbUserInformation.Id ||
                                current.Apprentices.Contains(dbUserInformation.Id) || current.IsVocationalTrainer))
            return true;
        return false;
    }

    public bool IsVocational()
    {
        var current = _userContext.GetLoggedUser().Result;

        if (current != null && current.IsVocationalTrainer)
            return true;
        return false;
    }

    public bool IsTrainer()
    {
        var current = _userContext.GetLoggedUser().Result;

        if (current != null && current.Apprentices.Count != 0)
            return true;
        return false;
    }
}