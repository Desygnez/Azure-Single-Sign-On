using Dapper;
using NotenTool.API.Context;

namespace NotenTool.API.School;

public class SchoolRepository : ISchoolRepository
{
    private readonly DapperContext _context;

    public SchoolRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<DbSchool> GetSchoolById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var resp = await connection.QueryFirstOrDefaultAsync<DbSchool>("SELECT * FROM School WHERE Id = @id",
            new { id });
        return resp;
    }

    public async Task<IEnumerable<DbSchool>> GetAllSchools()
    {
        using var connection = _context.CreateConnection();

        var resp = await connection.QueryAsync<DbSchool>("SELECT * FROM School");
        return resp.ToList();
    }

    public async Task UpdateSchool(DbSchool dbSchool)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("UPDATE School SET School = @school WHERE Id = @id",
            new { school = dbSchool.School, id = dbSchool.Id });

    }

    public async Task CreateSchool(DbSchool dbSchool)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("INSERT INTO School (Id, School) VALUES (@id, @school)",
            new { id = dbSchool.Id, school = dbSchool.School });

    }

    public async void DeleteSchool(Guid id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("DELETE FROM School WHERE Id = @id", new { id });
    }
}