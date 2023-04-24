namespace NotenTool.API.AllGrades;

public interface IAllGradesRepository
{
    Task<DbAllGrades> GetGradeById(Guid id);
    Task<IEnumerable<DbAllGrades>> GetAllGrades();
    void UpdateGrade(DbAllGrades dbAllGrades);
    void CreateGradeAsync(DbAllGrades dbAllGrades);
    void DeleteGradeAsync(Guid id);
    void DeleteGradeByGradeId(Guid id);
    Task<IEnumerable<DbAllGrades>> GetByUserId(Guid id);
    Task<DbAllGrades?> GetByGradeId(Guid id);
    Task<IEnumerable<DbAllGrades>> GetByUserIdAndSubjectId(Guid userId, Guid subjectId);
}