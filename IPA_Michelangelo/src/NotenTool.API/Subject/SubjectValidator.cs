namespace NotenTool.API.Subject;

public interface ISubjectValidator
{
    bool IsSubjectValid(Guid id);
}

public class SubjectValidator : ISubjectValidator
{
    private readonly ISubjectRepository _subjectRepository;

    public SubjectValidator(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }


    public bool IsSubjectValid(Guid id)
    {
        var subject = _subjectRepository.GetSubjectById(id).Result;
        if (subject == null)
            return false;

        return true;
    }
}