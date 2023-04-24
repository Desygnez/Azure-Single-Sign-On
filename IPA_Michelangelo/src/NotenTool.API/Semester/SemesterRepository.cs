using System.Drawing;
using Dapper;
using NotenTool.API.Context;
using NotenTool.API.Subject;

namespace NotenTool.API.Semester;

public class SemesterRepository : ISemesterRepository
{
    private readonly DapperContext _context;

    public SemesterRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<DbSemester> GetSemesterById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var response =
            await connection.QueryFirstOrDefaultAsync<DbSemester>("SELECT * FROM Semester WHERE Id = @id ORDER BY Semester",
                new { id });
        return response;
    }

    public async Task<IEnumerable<DbSemester>> GetAllSemester()
    {
        using var connection = _context.CreateConnection();

        var response = await connection.QueryAsync<DbSemester>("SELECT * FROM Semester ORDER BY Semester");
        return response.ToList();
    }

    public async Task CreateSemester(DbSemester dbSemester)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("INSERT INTO Semester (Id, Semester) VALUES (@id, @semester)",
            new { id = dbSemester.Id, semester = dbSemester.Semester });
    }

    public async Task UpdateSemester(DbSemester dbSemester)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("UPDATE Semester SET Semester = @semester WHERE Id = @id",
            new { id = dbSemester.Id, semester = dbSemester.Semester });

    }

    public async Task DeleteSemester(Guid id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Semester WHERE id = @id", new { id });
    }
}