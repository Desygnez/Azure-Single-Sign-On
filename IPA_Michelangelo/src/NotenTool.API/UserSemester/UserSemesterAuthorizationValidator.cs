using NotenTool.API.UserInformation;

namespace NotenTool.API.UserSemester;

public interface IUserSemesterAuthorizationValidator
{
    bool CanUserGetUserSemester(Guid id);
    bool CanUserGetUserSemesterByUserId(Guid userId);
    bool CanUserModify(Guid userId);
    bool CanUserUpdate(Guid id);
}

public class UserSemesterAuthorizationValidator : IUserSemesterAuthorizationValidator
{
    private readonly IUserContext _userContext;
    private readonly IUserSemesterRepository _userSemesterRepository;

    public UserSemesterAuthorizationValidator(IUserContext userContext, IUserSemesterRepository userSemesterRepository)
    {
        _userContext = userContext;
        _userSemesterRepository = userSemesterRepository;
    }

    bool IUserSemesterAuthorizationValidator.CanUserGetUserSemester(Guid id)
    {
        var current = _userContext.GetLoggedUser().Result;
        var userSemester = _userSemesterRepository.GetUserSemesterById(id).Result;
        if (current != null && (current.IsVocationalTrainer || current.Id == userSemester.UserInfo_id ||
                                current.Apprentices.Contains(userSemester.UserInfo_id)))
            return true;
        return false;
    }

    bool IUserSemesterAuthorizationValidator.CanUserGetUserSemesterByUserId(Guid userId)
    {
        var current = _userContext.GetLoggedUser().Result;
        var userSemester = _userSemesterRepository.GetUserSemesterByUserId(userId).Result.First();
        if (userSemester != null && current != null &&
            (current.IsVocationalTrainer || current.Id == userSemester.UserInfo_id ||
             current.Apprentices.Contains(userSemester.UserInfo_id)))
            return true;
        return false;
    }

    bool IUserSemesterAuthorizationValidator.CanUserModify(Guid userId)
    {
        var current = _userContext.GetLoggedUser().Result;
        if (current != null &&
            (current.IsVocationalTrainer || current.Id == userId || current.Apprentices.Contains(userId)))
            return true;

        return false;
    }

    public bool CanUserUpdate(Guid id)
    {
        var current = _userContext.GetLoggedUser().Result;
        var dbUserSemester = _userSemesterRepository.GetUserSemesterById(id).Result;

        if (current != null &&
            (current.IsVocationalTrainer || current.Id == dbUserSemester.UserInfo_id ||
             current.Apprentices.Contains(dbUserSemester.UserInfo_id)))
            return true;

        return false;
    }
}