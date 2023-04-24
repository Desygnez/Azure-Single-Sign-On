namespace NotenTool.API.SubjectUser;

public interface ISubjectUserValidator
{
    bool IsSubjectUserValid(Guid id);
    bool IsSubjectUserValidByUserId(Guid id);
    bool DoesNotAlreadyExistByUserId(Guid userId, Guid subjectId, Guid semesterId);
}

public class SubjectUserValidator : ISubjectUserValidator
{
    private readonly ISubjectUserRepository _subjectUserRepository;

    public SubjectUserValidator(ISubjectUserRepository subjectUserRepository)
    {
        _subjectUserRepository = subjectUserRepository;
    }


    bool ISubjectUserValidator.IsSubjectUserValid(Guid id)
    {
        var subjectUser = _subjectUserRepository.GetSubjectById(id).Result;
        if (subjectUser == null)
            return false;

        return true;
    }

    bool ISubjectUserValidator.IsSubjectUserValidByUserId(Guid id)
    {
        var subjectUser = _subjectUserRepository.GetSubjectsByUserId(id);
        if (subjectUser == null)
            return false;
        return true;
    }

    public bool DoesNotAlreadyExistByUserId(Guid userId, Guid subjectId, Guid semesterId)
    {
        var subjectUsers = _subjectUserRepository.GetSubjectsByUserId(userId).Result;
        if (subjectUsers.Any(
                subjectUser => subjectUser.Subject_id == subjectId && subjectUser.Semester_id == semesterId))
            return false;

        return true;
    }
}