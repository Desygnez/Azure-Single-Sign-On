using Dapper;
using NotenTool.API.Context;

namespace NotenTool.API.UserInformation;

public interface IUserRepository
{
    Task<User?> GetUser(string username);
}

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUser(string username)
    {
        using var connection = _context.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM UserInformation WHERE username = @username",
            new { username });

        if (user == null)
            return null;

        var trainedApprenticeIds = await connection.QueryAsync<Guid>(
            "SELECT ApprenticeId FROM TrainerApprentice WHERE TrainerId = @id",
            new { user.Id });

        user.Apprentices = trainedApprenticeIds.ToList();

        var vocational = await connection.QuerySingleOrDefaultAsync(
            "SELECT * FROM VocationalTrainer WHERE id = @id",
            new { user.Id });
        if (vocational != null) user.IsVocationalTrainer = true;

        return user;
    }
}

public class User
{
    public User()
    {
        Apprentices = new List<Guid>();
    }

    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public bool IsVocationalTrainer { get; set; }
    public List<Guid> Apprentices { get; set; }
}
