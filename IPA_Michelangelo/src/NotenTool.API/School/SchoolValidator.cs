namespace NotenTool.API.School;

public interface ISchoolValidator
{
    bool IsSchoolValid(Guid id);
}

public class SchoolValidator : ISchoolValidator
{
    private readonly ISchoolRepository _schoolRepository;

    public SchoolValidator(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }


    public bool IsSchoolValid(Guid id)
    {
        if (_schoolRepository.GetSchoolById(id).Result == null)
            return false;

        return true;
    }
}