namespace NotenTool.API.Grade;

public interface IGradeRepository
{
    Task<DbGrade?> GetGradeById(Guid id);
    void UpdateGrade(DbGrade dbGrade);
    void CreateGrade(DbGrade dbGrade, Guid userId);
    void DeleteGrade(Guid id);
    void DeleteGradeBySemesterId(Guid id);
}