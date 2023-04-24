using NotenTool.API.UserInformation;

namespace NotenTool.API.Semester;

public interface ISemesterAuthorizationValidator
{
    bool CanUserModifySemester();
}

public class SemesterAuthorizationValidator : ISemesterAuthorizationValidator
{
    private readonly IUserContext _userContext;

    public SemesterAuthorizationValidator(IUserContext userContext)
    {
        _userContext = userContext;
    }

    bool ISemesterAuthorizationValidator.CanUserModifySemester()
    {
        var user = _userContext.GetLoggedUser().Result;
        if (user != null && (user.IsVocationalTrainer || user.Apprentices.Count != 0))
            return true;

        return false;
    }
}