namespace NotenTool.API.UserInformation;

public interface IUserInformationRepository
{
    Task<DbUserInformation> GetUserById(Guid id);
    Task<IEnumerable<DbUserInformation>> GetAllUsers();
    Task<DbUserInformation> GetUserByUsername(string username);
    void UpdateUser(DbUserInformation dbUser);
    void CreateUser(DbUserInformation dbUser);
    Task CreateTrainerApprentice(DbTrainerApprentice trainerApprentice);
    void DeleteUser(Guid id);
    Task<List<DbUserInformation>> GetUserByIdList(List<Guid> loggedUserApprentices);
}