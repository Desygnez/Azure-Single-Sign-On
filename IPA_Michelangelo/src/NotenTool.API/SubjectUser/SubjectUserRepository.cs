using System.Data;
using Dapper;
using NotenTool.API.Context;
using NotenTool.API.School;
using NotenTool.API.Semester;
using NotenTool.API.Subject;
using NotenTool.API.UserInformation;

namespace NotenTool.API.SubjectUser;

public class SubjectUserRepository : ISubjectUserRepository
{
    private readonly DapperContext _context;

    public SubjectUserRepository(DapperContext context)
    {
        _context = context;
    }


    public async Task<DbSubjectUser> GetSubjectById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql =
            @"SELECT * FROM Subject_has_user shu
            LEFT JOIN Subject s ON s.Id = shu.Subject_id 
            LEFT JOIN UserInformation u ON u.Id = shu.UserInfo_id 
            LEFT JOIN School sc ON sc.Id = shu.School_id
            LEFT JOIN Semester se ON se.Id = shu.Semester_id
            WHERE shu.Id = @id";

        var data = connection
            .QueryAsync<DbSubjectUser, DbSubject, DbUserInformation, DbSchool, DbSemester, DbSubjectUser>(sql,
                (subjectUser, subject, userinfo, school, semester) =>
                {
                    subjectUser.Subject = subject;
                    subjectUser.UserInformation = userinfo;
                    subjectUser.School = school;
                    subjectUser.Semester = semester;

                    return subjectUser;
                }, new { id });

        return data.Result.FirstOrDefault();
    }


    public async Task<IEnumerable<DbSubjectUser>> GetSubjectsByUserId(Guid userId)
    {
        using var connection = _context.CreateConnection();

        var sql =
            @"SELECT * FROM Subject_has_user shu
            LEFT JOIN Subject s ON s.Id = shu.Subject_id 
            LEFT JOIN UserInformation u ON u.Id = shu.UserInfo_id 
            LEFT JOIN School sc ON sc.Id = shu.School_id
            LEFT JOIN Semester se ON se.Id = shu.Semester_id 
            WHERE shu.UserInfo_id = @id";

        var data = connection
            .QueryAsync<DbSubjectUser, DbSubject, DbUserInformation, DbSchool, DbSemester, DbSubjectUser>(sql,
                (subjectUser, subject, userinfo, school, semester) =>
                {
                    subjectUser.Subject = subject;
                    subjectUser.UserInformation = userinfo;
                    subjectUser.School = school;
                    subjectUser.Semester = semester;

                    return subjectUser;
                }, new { id = userId });

        return data.Result;
    }

    public async void UpdateSubjectUser(DbSubjectUser dbSubjectUser)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "UPDATE Subject_has_user SET Subject_id = @subject, UserInfo_id = @userInfo, School_id = @schoolId, Semester_id = @semesterId WHERE id = @id",
            new
            {
                id = dbSubjectUser.Id, subject = dbSubjectUser.Subject_id, userInfo = dbSubjectUser.UserInfo_id,
                schoolId = dbSubjectUser.School_id, semesterId = dbSubjectUser.Semester_id
            });
    }

    public async void CreateSubjectUser(DbSubjectUser dbSubjectUser)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "INSERT INTO Subject_has_user (Id, Subject_id, UserInfo_id, School_id, Semester_id) VALUES (@id, @subject, @userInfo, @school, @semester)",
            new
            {
                id = dbSubjectUser.Id, subject = dbSubjectUser.Subject_id, userInfo = dbSubjectUser.UserInfo_id,
                school = dbSubjectUser.School_id, semester = dbSubjectUser.Semester_id
            });
    }

    public async void DeleteSubjectUser(Guid id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("DELETE FROM Subject_has_user WHERE id = @id", new { id });
    }

    public async void DeleteSubjectUserBySemesterId(Guid id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("DELETE FROM Subject_has_user WHERE Semester_id = @id",
            new { id });
    }
}