namespace NotenTool.API.UserSemester;

public interface IUserSemesterRepository
{
    Task<DbUserSemester> GetUserSemesterById(Guid id);
    Task<IEnumerable<DbUserSemester?>> GetUserSemesterByUserId(Guid id);
    Task UpdateUserSemester(DbUserSemester dbUserSemester);
    Task CreateUserSemester(DbUserSemester dbUserSemester);
    Task DeleteUserSemester(Guid id);
}