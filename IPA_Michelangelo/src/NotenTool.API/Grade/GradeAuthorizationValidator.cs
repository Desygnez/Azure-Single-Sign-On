using NotenTool.API.AllGrades;
using NotenTool.API.UserInformation;

namespace NotenTool.API.Grade;

public interface IGradeAuthorizationValidator
{
    bool CanUserUseGrade(Guid gradeId);
    bool CanUserCreateGrade(Guid userId);
    bool CanUserGetAllGrades();
}

public class GradeAuthorizationValidator : IGradeAuthorizationValidator
{
    private readonly IAllGradesRepository _gradesRepository;
    private readonly IUserContext _context;

    public GradeAuthorizationValidator(IAllGradesRepository gradesRepository, IUserContext context)
    {
        _gradesRepository = gradesRepository;
        _context = context;
    }

    bool IGradeAuthorizationValidator.CanUserUseGrade(Guid gradeId)
    {
        var grade = _gradesRepository.GetByGradeId(gradeId).Result;
        var user = _context.GetLoggedUser().Result;
        // System.IO.File.AppendAllText("log.txt", "grade: " + grade?.UserInfo_id);
        // System.IO.File.AppendAllText("log.txt", "user: " + user?.Id);
        if (user == null | grade == null)
            return false;
        if (grade.UserInfo_id == user.Id || user.IsVocationalTrainer || user.Apprentices.Contains(grade.UserInfo_id))
        {
            return true;
        }

        return false;
    }

    bool IGradeAuthorizationValidator.CanUserCreateGrade(Guid userId)
    {
        var user = _context.GetLoggedUser().Result;
        if (user != null && (user.Id == userId || user.IsVocationalTrainer || user.Apprentices.Contains(userId)))
            return true;
        return false;
    }

    bool IGradeAuthorizationValidator.CanUserGetAllGrades()
    {
        var user = _context.GetLoggedUser().Result;
        if (user == null)
            return false;
        if (user.IsVocationalTrainer)
            return true;

        return false;
    }
}