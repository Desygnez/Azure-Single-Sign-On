using Microsoft.Extensions.DependencyInjection.Extensions;
using NotenTool.API.AllGrades;
using NotenTool.API.Classes;
using NotenTool.API.Context;
using NotenTool.API.Grade;
using NotenTool.API.Semester;
using NotenTool.API.Subject;
using NotenTool.API.SubjectUser;
using NotenTool.API.UserInformation;
using NotenTool.API.School;
using NotenTool.API.UserSemester;

namespace NotenTool.API.IoC;

interface IApiModule
{
    void RegisterDependencies(IServiceCollection services, WebApplicationBuilder builder);
}

public class ApiModule : IApiModule
{
    private string _connectionString;

    public ApiModule(string connectionString)
    {
        _connectionString = connectionString;
    }

    void IApiModule.RegisterDependencies(IServiceCollection services, WebApplicationBuilder builder)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddSingleton<DapperContext>(provider => new DapperContext(_connectionString));
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<IUserInformationRepository, UserInformationRepository>();
        services.AddScoped<IAllGradesRepository, AllGradesRepository>();
        services.AddScoped<ISubjectUserRepository, SubjectUserRepository>();
        services.AddScoped<ISemesterRepository, SemesterRepository>();
        services.AddScoped<ISchoolRepository, SchoolRepository>();
        services.AddScoped<IUserSemesterRepository, UserSemesterRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGradeAuthorizationValidator, GradeAuthorizationValidator>();
        services.AddScoped<ISubjectValidator, SubjectValidator>();
        services.AddScoped<ISemesterValidator, SemesterValidator>();
        services.AddScoped<ISchoolValidator, SchoolValidator>();
        services.AddScoped<IUserSemesterValidator, UserSemesterValidator>();
        services.AddScoped<ISubjectUserValidator, SubjectUserValidator>();
        services.AddScoped<ISchoolAuthorizationValidator, SchoolAuthorizationValidator>();
        services.AddScoped<ISemesterAuthorizationValidator, SemesterAuthorizationValidator>();
        services.AddScoped<ISubjectAuthorzationValidator, SubjectAuthorzationValidator>();
        services.AddScoped<ISubjectUserAuthorizationValidator, SubjectUserAuthorizationValidator>();
        services.AddScoped<IUserInformationValidator, UserInformationValidator>();
        services.AddScoped<IUserInformationAuthorizationValidator, UserInformationAuthorizationValidator>();
        services.AddScoped<IUserSemesterAuthorizationValidator, UserSemesterAuthorizationValidator>();
        services.AddScoped<IAllGradesAuthoriationValidator, AllGradesAuthorizationValidator>();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IUserContext, UserContext>();
    }
}