using System.Data;
using System.Data.SqlClient;
using Dapper;
using NotenTool.API.AllGrades;
using NotenTool.API.Context;
using NotenTool.API.Semester;
using NotenTool.API.Subject;

namespace NotenTool.API.Grade;

public class GradeRepository : IGradeRepository
{
    private readonly DapperContext _context;

    public GradeRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<DbGrade?> GetGradeById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql =
            @"SELECT * FROM Grade g LEFT JOIN Subject s ON s.Id = g.Subject_id LEFT JOIN Semester sem ON sem.Id = g.Semester_id WHERE g.id = @id";

        var data = connection.QueryAsync<DbGrade, DbSubject, DbSemester, DbGrade>(sql,
            (grade, subject,  semester) =>
            {
                grade.Subjects = subject;
                grade.Semester = semester;
                return grade;
            }, new { id });

        return data.Result.First();
    }

    public async void UpdateGrade(DbGrade dbGrade)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "UPDATE Grade SET Grade = @grade, Date = @date, Subject_id = @subjectId, Semester_id = @semesterId, Comment = @comment, Weight = @weight WHERE id = @id",
            new
            {
                grade = dbGrade.Grade,
                date = dbGrade.Date,
                subjectId = dbGrade.Subject_id,
                semesterId = dbGrade.Semester_id,
                weight = dbGrade.Weight,
                comment = dbGrade.Comment,
                id = dbGrade.Id
            });
    }


    public async void CreateGrade(DbGrade dbGrade, Guid UserId)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "INSERT INTO dbo.Grade (Id, Grade, Date, Subject_id,  Semester_id, Comment, Weight) VALUES (@id, @grade, @date, @subjectId, @semesterId, @comment, @weight)",
            new
            {
                id = dbGrade.Id,
                grade = dbGrade.Grade,
                date = dbGrade.Date,
                subjectId = dbGrade.Subject_id,
                semesterId = dbGrade.Semester_id,
                weight = dbGrade.Weight,
                comment = dbGrade.Comment
            });
        var allGrades = new AllGradesRepository(_context);
        allGrades.CreateGradeAsync(new DbAllGrades
            { Id = Guid.NewGuid(), UserInfo_id = UserId, Grade_id = dbGrade.Id });
    }

    public async void DeleteGrade(Guid id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("DELETE FROM Grade WHERE id = @id", new { id });
        var allGrades = new AllGradesRepository(_context);
        allGrades.DeleteGradeByGradeId(id);
    }

    public async void DeleteGradeBySemesterId(Guid id)
    {
        using var connection = _context.CreateConnection();
        var grades = connection.QueryAsync<Guid>("SELECT Id FROM Grade WHERE Semester_id = @Id", new { id }).Result;

        await connection.ExecuteAsync("DELETE FROM Grade WHERE Semester_id = @id", new { id });

        var allGrades = new AllGradesRepository(_context);
        foreach (var grade in grades)
        {
            allGrades.DeleteGradeByGradeId(grade);
        }
    }
}