using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NotenTool.API.Classes;
using NotenTool.API.Grade;
using NotenTool.API.IoC;
using NotenTool.API.School;
using NotenTool.API.Semester;
using NotenTool.API.Subject;
using NotenTool.API.SubjectUser;
using NotenTool.API.UserInformation;
using NotenTool.API.UserSemester;

namespace NotenTool.Api.Test.Integration;

public class TestApiModule : IApiModule
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ISemesterRepository _semesterRepository;
    private readonly ISchoolRepository _schoolRepository;
    private readonly IUserSemesterRepository _userSemesterRepository;
    private readonly ISubjectUserRepository _subjectUserRepository;
    private readonly IUserInformationRepository _userInformationRepository;

    public const string ConnectionString =
        "server=127.0.0.1\\inspiring_nighting,1433;user=sa;password=thisPasswordIsStrong1234;database=TestNotentool; Trusted_Connection=true;integrated security=False";

    public TestApiModule(ISubjectRepository subjectRepository,
        ISemesterRepository semesterRepository, ISchoolRepository schoolRepository, IUserSemesterRepository userSemesterRepository,
        ISubjectUserRepository subjectUserRepository, IUserInformationRepository userInformationRepository,
        IGradeRepository gradeRepository)
    {
        _subjectRepository = subjectRepository;
        _semesterRepository = semesterRepository;
        _schoolRepository = schoolRepository;
        _userSemesterRepository = userSemesterRepository;
        _subjectUserRepository = subjectUserRepository;
        _userInformationRepository = userInformationRepository;
    }

    public void RegisterDependencies(IServiceCollection services, WebApplicationBuilder builder)
    {
        IApiModule apiModule = new ApiModule(ConnectionString);
        apiModule.RegisterDependencies(services, builder);

        services.AddSingleton<IAuthenticationService, AllowAnonymousAuthenticationService>();

        // services.AddScoped(_ => _classesRepository);
        // services.AddScoped(_ => _subjectRepository);
    }
}