namespace NotenTool.API.UserSemester;

public interface IUserSemesterValidator
{
    bool IsUserSemesterValid(Guid id);
    bool IsUserSemesterValidByUserId(Guid userId);
    bool DoesNotAlreadyExistByUserId(Guid userId, Guid semesterId);
}

public class UserSemesterValidator : IUserSemesterValidator
{
    private readonly IUserSemesterRepository _userSemesterRepository;

    public UserSemesterValidator(IUserSemesterRepository userSemesterRepository)
    {
        _userSemesterRepository = userSemesterRepository;
    }


    bool IUserSemesterValidator.IsUserSemesterValid(Guid id)
    {
        var userSchool = _userSemesterRepository.GetUserSemesterById(id).Result;
        if (userSchool == null)
            return false;

        return true;
    }

    bool IUserSemesterValidator.IsUserSemesterValidByUserId(Guid userId)
    {
        var userSchool = _userSemesterRepository.GetUserSemesterByUserId(userId).Result;
        if (!userSchool.Any() || userSchool == null)
            return false;

        return true;
    }

    public bool DoesNotAlreadyExistByUserId(Guid userId, Guid semesterId)
    {
        var userSemesters = _userSemesterRepository.GetUserSemesterByUserId(userId).Result;
        if (userSemesters.Any(userSemester => userSemester.Semester_id == semesterId))
            return false;

        return true;
    }
}