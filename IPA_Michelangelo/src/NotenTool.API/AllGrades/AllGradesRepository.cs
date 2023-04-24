using System.Collections;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using NotenTool.API.Context;
using NotenTool.API.Grade;
using NotenTool.API.UserInformation;

namespace NotenTool.API.AllGrades;

public class AllGradesRepository : IAllGradesRepository
{
    private readonly DapperContext _context;

    public AllGradesRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<DbAllGrades> GetGradeById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql =
            @"SELECT * FROM AllGrades g LEFT JOIN UserInformation u ON u.Id = g.Userinfo_id LEFT JOIN Grade gr ON gr.Id = g.Grade_id WHERE g.Id = @id";
        var data = connection.QueryAsync<DbAllGrades, DbUserInformation, DbGrade, DbAllGrades>(sql,
            (grades, information, grade) =>
            {
                grades.Grades = grade;
                grades.UserInformations = information;
                return grades;
            }, new { id });

        return data.Result.First();
    }

    public async Task<DbAllGrades?> GetByGradeId(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql =
            @"SELECT * FROM AllGrades g LEFT JOIN UserInformation u ON u.Id = g.Userinfo_id LEFT JOIN Grade gr ON gr.Id = g.Grade_id WHERE g.Grade_id = @id";
        var data = connection.QueryAsync<DbAllGrades, DbUserInformation, DbGrade, DbAllGrades>(sql,
            (grades, information, grade) =>
            {
                grades.Grades = grade;
                grades.UserInformations = information;
                return grades;
            }, new { id });

        return data.Result.FirstOrDefault();
    }

    public async Task<IEnumerable<DbAllGrades>> GetAllGrades()
    {
        using var connection = _context.CreateConnection();

        var sql =
            @"SELECT * FROM AllGrades g LEFT JOIN UserInformation u ON u.Id = g.Userinfo_id LEFT JOIN Grade gr ON gr.Id = g.Grade_id";
        var data = connection.QueryAsync<DbAllGrades, DbUserInformation, DbGrade, DbAllGrades>(sql,
            (grades, information, grade) =>
            {
                grades.Grades = grade;
                grades.UserInformations = information;
                return grades;
            });

        return data.Result;
    }

    public async void UpdateGrade(DbAllGrades dbAllGrades)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("UPDATE AllGrades SET UserInfo_id = @user, Grade_id = @grade_id WHERE id = @id",
            new { user = dbAllGrades.UserInfo_id, grade_id = dbAllGrades.Grade_id, id = dbAllGrades.Id });
    }

    public async void CreateGradeAsync(DbAllGrades dbAllGrades)
    {
        using var connection = _context.CreateConnection();
        
        await connection.ExecuteAsync("INSERT INTO AllGrades (Id, UserInfo_id, Grade_id) VALUES (@id, @user, @grade)",
            new { id = dbAllGrades.Id, user = dbAllGrades.UserInfo_id, grade = dbAllGrades.Grade_id });
    }

    public async void DeleteGradeAsync(Guid id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM AllGrades WHERE id = @id", new { id });
    }

    public async void DeleteGradeByGradeId(Guid id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM AllGrades WHERE Grade_id = @id", new { id });

        var gradeRepository = new GradeRepository(_context);
        gradeRepository.DeleteGrade(id);
    }


    public async Task<IEnumerable<DbAllGrades>> GetByUserId(Guid id)
    {
        using var connection = _context.CreateConnection();
        var sql =
            @"SELECT * FROM AllGrades g LEFT JOIN UserInformation u ON u.Id = g.Userinfo_id LEFT JOIN Grade gr ON gr.Id = g.Grade_id WHERE g.UserInfo_id = @id";
        var data = connection.QueryAsync<DbAllGrades, DbUserInformation, DbGrade, DbAllGrades>(sql,
            (grades, information, grade) =>
            {
                grades.Grades = grade;
                grades.UserInformations = information;
                return grades;
            }, new { id });

        return data.Result;
    }

    public async Task<IEnumerable<DbAllGrades>> GetByUserIdAndSubjectId(Guid userId, Guid subjectId)
    {
        using var connection = _context.CreateConnection();

        var sql =
            @"SELECT * FROM AllGrades g LEFT JOIN UserInformation u ON u.Id = g.Userinfo_id LEFT JOIN Grade gr ON gr.Id = g.Grade_id WHERE g.UserInfo_id = @id";
        var data = connection.QueryAsync<DbAllGrades, DbUserInformation, DbGrade, DbAllGrades>(sql,
            (grades, information, grade) =>
            {
                grades.Grades = grade;
                grades.UserInformations = information;
                return grades;
            }, new { id = userId });

        return data.Result.Where(gradesheet => gradesheet.Grades.Subject_id == subjectId).ToList();
    }
}