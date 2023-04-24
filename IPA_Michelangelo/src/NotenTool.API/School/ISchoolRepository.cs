namespace NotenTool.API.School;

public interface ISchoolRepository
{
    Task<DbSchool> GetSchoolById(Guid id);
    Task<IEnumerable<DbSchool>> GetAllSchools();
    Task UpdateSchool(DbSchool dbSchool);
    Task CreateSchool(DbSchool dbSchool);
    void DeleteSchool(Guid id);
}