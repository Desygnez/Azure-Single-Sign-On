using Dapper;
using NotenTool.API.Context;
using NotenTool.API.Grade;
using NotenTool.API.Semester;
using NotenTool.API.SubjectUser;
using NotenTool.API.UserInformation;

namespace NotenTool.API.UserSemester;

public class UserSemesterRepository : IUserSemesterRepository
{
    private readonly DapperContext _context;
    private readonly ISubjectUserRepository _subjectUserRepository;
    private readonly IGradeRepository _gradeRepository;

    public UserSemesterRepository(DapperContext context, ISubjectUserRepository subjectUserRepository,
        IGradeRepository gradeRepository)
    {
        _context = context;
        _subjectUserRepository = subjectUserRepository;
        _gradeRepository = gradeRepository;
    }

    public Task<DbUserSemester> GetUserSemesterById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var resp = connection.QueryAsync<DbUserSemester, DbUserInformation, DbSemester, DbUserSemester>(
            "SELECT * FROM User_has_Semester uhs LEFT JOIN UserInformation ui ON ui.Id = uhs.UserInfo_id LEFT JOIN Semester s ON s.Id = uhs.Semester_id WHERE uhs.Id = @id ORDER BY s.Semester",
            (userSemester, user, semester) =>
            {
                userSemester.UserInformation = user;
                userSemester.Semester = semester;
                return userSemester;
            }, new { id });
        return Task.FromResult(resp.Result.FirstOrDefault())!;
    }

    public async Task<IEnumerable<DbUserSemester?>> GetUserSemesterByUserId(Guid id)
    {
        using var connection = _context.CreateConnection();

        var resp = connection.QueryAsync<DbUserSemester, DbUserInformation, DbSemester, DbUserSemester>(
            "SELECT * FROM User_has_Semester uhs LEFT JOIN UserInformation ui ON ui.Id = uhs.UserInfo_id LEFT JOIN Semester s ON s.Id = uhs.Semester_id WHERE uhs.UserInfo_id = @id ORDER BY s.Semester",
            (userSemester, user, semester) =>
            {
                userSemester.UserInformation = user;
                userSemester.Semester = semester;
                return userSemester;
            }, new { id });
        return await resp;
    }

    public async Task UpdateUserSemester(DbUserSemester dbUserSemester)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "UPDATE User_has_Semester SET UserInfo_id = @userInfoId, Semester_id  = @semesterId  WHERE Id = @id",
            new
            {
                userInfoId = dbUserSemester.UserInfo_id, semesterId = dbUserSemester.Semester_id, id = dbUserSemester.Id
            });
    }

    public async Task CreateUserSemester(DbUserSemester dbUserSemester)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "INSERT INTO User_has_Semester (Id, UserInfo_id, Semester_id) VALUES (@id, @userInfoId, @semesterId)",
            new
            {
                id = dbUserSemester.Id, userInfoId = dbUserSemester.UserInfo_id, semesterId = dbUserSemester.Semester_id
            });
    }

    public async Task DeleteUserSemester(Guid id)
    {
        using var connection = _context.CreateConnection();
        var userSemester = connection.QueryAsync<DbUserSemester>(
            "SELECT * FROM User_has_Semester WHERE Id = @id", new { id }).Result.First();

        await connection.ExecuteAsync("DELETE FROM User_has_Semester WHERE Id = @id", new { id });
        _subjectUserRepository.DeleteSubjectUserBySemesterId(userSemester.Semester_id);
        _gradeRepository.DeleteGradeBySemesterId(userSemester.Semester_id);
    }
}