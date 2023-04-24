using NotenTool.API.UserInformation;

namespace NotenTool.API.Subject;

public interface ISubjectAuthorzationValidator
{
    bool CanUserModifySubject();
}

public class SubjectAuthorzationValidator : ISubjectAuthorzationValidator
{
    private readonly IUserContext _userContext;

    public SubjectAuthorzationValidator(IUserContext userContext)
    {
        _userContext = userContext;
    }

    bool ISubjectAuthorzationValidator.CanUserModifySubject()
    {
        var user = _userContext.GetLoggedUser().Result;

        if (user != null && (user.IsVocationalTrainer || user.Apprentices.Count != 0))
            return true;

        return false;
    }
}