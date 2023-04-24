using System.Data;
using Dapper;
using NotenTool.API.Context;

namespace NotenTool.API.UserInformation;

public class UserInformationRepository : IUserInformationRepository
{
    private readonly DapperContext _context;

    public UserInformationRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<DbUserInformation> GetUserById(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql = @"SELECT * FROM UserInformation u WHERE u.Id = @id";

        var data = connection.QueryAsync<DbUserInformation>(sql, new { id });

        return data.Result.FirstOrDefault();
    }

    public async Task<IEnumerable<DbUserInformation>> GetAllUsers()
    {
        using var connection = _context.CreateConnection();

        var sql = @"SELECT * FROM UserInformation u";

        var data = connection.QueryAsync<DbUserInformation>(sql);

        return data.Result;
    }

    public async Task<DbUserInformation> GetUserByUsername(string username)
    {
        using var connection = _context.CreateConnection();

        var sql = @"SELECT * FROM UserInformation u WHERE u.Username = @username";

        var data = connection.QueryAsync<DbUserInformation>(sql, new { username });

        return data.Result.FirstOrDefault();
    }

    public async void UpdateUser(DbUserInformation dbUser)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "UPDATE UserInformation SET Firstname = @firstname, Lastname = @lastname, Email = @email, Username = @username WHERE id = @id",
            new
            {
                firstname = dbUser.Firstname,
                lastname = dbUser.Lastname,
                email = dbUser.Email,
                id = dbUser.Id,
                username = dbUser.Username,
            });
    }

    public async void CreateUser(DbUserInformation dbUser)
    {
        using var connection = _context.CreateConnection();

        try
        {
            await connection.ExecuteAsync(
                "INSERT INTO UserInformation (Id, Firstname, Lastname, Email, Username) VALUES (@id, @firstname, @lastname, @email, @username)",
                new
                {
                    id = dbUser.Id,
                    firstname = dbUser.Firstname,
                    lastname = dbUser.Lastname,
                    email = dbUser.Email,
                    username = dbUser.Username,
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task CreateTrainerApprentice(DbTrainerApprentice trainerApprentice)
    {
        using var connection = _context.CreateConnection();
        try
        {
            await connection.ExecuteAsync(
                "INSERT INTO TrainerApprentice (Id, ApprenticeId, TrainerId) VALUES (@id, @apprentice, @trainer)",
                new
                {
                    id = trainerApprentice.Id,
                    apprentice = trainerApprentice.ApprenticeId,
                    trainer = trainerApprentice.TrainerId
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void DeleteUser(Guid id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync("DELETE FROM UserInformation WHERE id = @id", new { id });
    }

    public Task<List<DbUserInformation>> GetUserByIdList(List<Guid> loggedUserApprentices)
    {
        using var connection = _context.CreateConnection();
        var list = new List<DbUserInformation>();

        foreach (var guid in loggedUserApprentices)
        {
            var data = connection.QueryAsync<DbUserInformation>(
                "SELECT * FROM UserInformation u WHERE u.Id = @id", new { id = guid }).Result.First();
            list.Add(data);
        }

        return Task.FromResult(list);
    }
}