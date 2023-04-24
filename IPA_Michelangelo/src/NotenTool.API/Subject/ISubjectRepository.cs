namespace NotenTool.API.Subject;

public interface ISubjectRepository
{
    Task<DbSubject> GetSubjectById(Guid id);
    Task<IEnumerable<DbSubject>> GetAllSubjects();
    Task UpdateSubject(DbSubject dbSubjects);
    Task CreateSubject(DbSubject dbSubjects);
    Task DeleteSubject(Guid id);
}