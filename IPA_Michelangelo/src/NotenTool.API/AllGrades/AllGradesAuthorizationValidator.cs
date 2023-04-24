using NotenTool.API.AllGrades;
using NotenTool.API.UserInformation;

namespace NotenTool.API.Classes;

public interface IAllGradesAuthoriationValidator
{
    bool CanUserUseGradeById(Guid id);
    bool CanUserUseGradeByUserId(Guid id);
    bool CanUserUseGradeByGradeId(Guid id);
    bool IsVocational();
}

public class AllGradesAuthorizationValidator : IAllGradesAuthoriationValidator
{
    private readonly IUserContext _context;
    private readonly IAllGradesRepository _allGradesRepository;

    public AllGradesAuthorizationValidator(IUserContext context, IAllGradesRepository allGradesRepository)
    {
        _context = context;
        _allGradesRepository = allGradesRepository;
    }

    bool IAllGradesAuthoriationValidator.CanUserUseGradeById(Guid id)
    {
        var user = _context.GetLoggedUser().Result;
        var grade = _allGradesRepository.GetByGradeId(id).Result;
        if (user != null && grade != null && !user.IsVocationalTrainer &&
            !user.Apprentices.Contains(grade.UserInfo_id) && grade.UserInfo_id != user.Id)
            return false;

        return true;
    }

    bool IAllGradesAuthoriationValidator.CanUserUseGradeByUserId(Guid id)
    {
        var user = _context.GetLoggedUser().Result;
        if (!user.IsVocationalTrainer && user.Id != id && !user.Apprentices.Contains(id))
        {
            return false;
        }

        return true;
    }

    bool IAllGradesAuthoriationValidator.CanUserUseGradeByGradeId(Guid id)
    {
        var user = _context.GetLoggedUser().Result;
        var grade = _allGradesRepository.GetByGradeId(id).Result;
        
        if (!user.IsVocationalTrainer && user.Id != grade.UserInfo_id && !user.Apprentices.Contains(grade.UserInfo_id))
        {
            return false;
        }

        return true;
    }

    bool IAllGradesAuthoriationValidator.IsVocational()
    {
        var user = _context.GetLoggedUser().Result;

        if (user != null && !user.IsVocationalTrainer)
        {
            return false;
        }

        return true;
    }
}