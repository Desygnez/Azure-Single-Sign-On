using NotenTool.API.UserInformation;

namespace NotenTool.API.School;

public interface ISchoolAuthorizationValidator
{
    bool CanUserModifySchool();
}

public class SchoolAuthorizationValidator : ISchoolAuthorizationValidator
{
    private readonly IUserContext _userContext;

    public SchoolAuthorizationValidator(IUserContext userContext)
    {
        _userContext = userContext;
    }

    bool ISchoolAuthorizationValidator.CanUserModifySchool()
    {
        var user = _userContext.GetLoggedUser().Result;

        if (user != null && user.IsVocationalTrainer)
            return true;

        return false;
    }
}