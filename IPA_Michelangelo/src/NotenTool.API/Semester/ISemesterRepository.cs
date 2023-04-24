using Microsoft.AspNetCore.Mvc;

namespace NotenTool.API.Semester;

public interface ISemesterRepository
{
    Task<DbSemester> GetSemesterById(Guid id);
    Task<IEnumerable<DbSemester>> GetAllSemester();
    Task CreateSemester(DbSemester dbSemester);
    Task UpdateSemester(DbSemester dbSemester);
    Task DeleteSemester(Guid id);
}