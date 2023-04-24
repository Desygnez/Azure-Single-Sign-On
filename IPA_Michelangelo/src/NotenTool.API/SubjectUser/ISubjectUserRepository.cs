namespace NotenTool.API.SubjectUser;

public interface ISubjectUserRepository
{
    Task<DbSubjectUser> GetSubjectById(Guid id);
    Task<IEnumerable<DbSubjectUser>> GetSubjectsByUserId(Guid userId);
    void UpdateSubjectUser(DbSubjectUser dbSubjectUser);
    void CreateSubjectUser(DbSubjectUser dbSubjectUser);
    void DeleteSubjectUser(Guid id);
    void DeleteSubjectUserBySemesterId(Guid id);
}