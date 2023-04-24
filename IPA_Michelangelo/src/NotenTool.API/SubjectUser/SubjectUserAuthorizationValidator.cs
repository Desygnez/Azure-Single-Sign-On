using NotenTool.API.Subject;
using NotenTool.API.UserInformation;

namespace NotenTool.API.SubjectUser;

public interface ISubjectUserAuthorizationValidator
{
    bool CanUserGetSubjectUser(Guid id);
    bool CanUserGetWithUserId(Guid userId);
    bool CanUserModifySubjectUserByUserId(Guid userId);
    bool CanUserModifySubjectUser(Guid id);
}

public class SubjectUserAuthorizationValidator : ISubjectUserAuthorizationValidator
{
    private readonly ISubjectUserRepository _subjectUserRepository;
    private readonly IUserContext _userContext;

    public SubjectUserAuthorizationValidator(ISubjectUserRepository subjectUserRepository, IUserContext userContext)
    {
        _subjectUserRepository = subjectUserRepository;
        _userContext = userContext;
    }

    bool ISubjectUserAuthorizationValidator.CanUserGetSubjectUser(Guid id)
    {
        var subjectUser = _subjectUserRepository.GetSubjectById(id).Result;
        var user = _userContext.GetLoggedUser().Result;

        if (user != null && (user.IsVocationalTrainer || user.Id == subjectUser.UserInfo_id ||
                             user.Apprentices.Contains(subjectUser.UserInfo_id)))
            return true;

        return false;
    }

    bool ISubjectUserAuthorizationValidator.CanUserGetWithUserId(Guid userId)
    {
        var user = _userContext.GetLoggedUser().Result;

        if (user != null && (user.IsVocationalTrainer || user.Id == userId || user.Apprentices.Contains(userId)))
            return true;
        return false;
    }

    bool ISubjectUserAuthorizationValidator.CanUserModifySubjectUserByUserId(Guid userId)
    {
        var user = _userContext.GetLoggedUser().Result;

        if (user != null && (user.Id == userId || user.Apprentices.Contains(userId) || user.IsVocationalTrainer))
            return true;
        return false;
    }

    bool ISubjectUserAuthorizationValidator.CanUserModifySubjectUser(Guid id)
    {
        var subjectUser = _subjectUserRepository.GetSubjectById(id).Result;
        var user = _userContext.GetLoggedUser().Result;

        if (user != null && (user.Id == subjectUser.UserInfo_id || user.Apprentices.Contains(subjectUser.UserInfo_id) || user.IsVocationalTrainer))
            return true;
        return false;
    }
}