using System.Data;
using Dapper;
using NotenTool.API.Context;

namespace NotenTool.API.Subject;

public class SubjectRepository : ISubjectRepository
{
    private readonly DapperContext _context;

    public SubjectRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<DbSubject> GetSubjectById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var response =
            await connection.QueryFirstOrDefaultAsync<DbSubject>("SELECT * FROM Subject WHERE Id = @subjectId",
                new { subjectId = id });
        return response;
    }

    public async Task<IEnumerable<DbSubject>> GetAllSubjects()
    {
        using var connection = _context.CreateConnection();

        var response = await connection.QueryAsync<DbSubject>("SELECT * FROM Subject");
        return response.ToList();
    }

    public async Task UpdateSubject(DbSubject dbSubjects)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("UPDATE Subject SET Subject = @subject WHERE Id = @id",
            new { id = dbSubjects.Id, subject = dbSubjects.Subject });
    }

    public async Task CreateSubject(DbSubject dbSubjects)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("INSERT INTO Subject (Id, Subject) VALUES (@id, @subject)",
            new { id = dbSubjects.Id, subject = dbSubjects.Subject });
    }

    public async Task DeleteSubject(Guid id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Subject WHERE id = @id", new { id });
    }
}