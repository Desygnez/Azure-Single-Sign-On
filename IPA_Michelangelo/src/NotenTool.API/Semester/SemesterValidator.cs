namespace NotenTool.API.Semester;

public interface ISemesterValidator
{
    bool IsSemesterValid(Guid id);
}

public class SemesterValidator : ISemesterValidator
{
    private readonly ISemesterRepository _semesterRepository;

    public SemesterValidator(ISemesterRepository semesterRepository)
    {
        _semesterRepository = semesterRepository;
    }
    public bool IsSemesterValid(Guid id)
    {
        var semester = _semesterRepository.GetSemesterById(id).Result;
        
        if (semester == null)
        {
            return false;
        }

        return true;
    }
}