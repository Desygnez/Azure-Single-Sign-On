using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using NotenTool.API;
using NotenTool.API.Classes;
using NotenTool.API.Grade;
using NotenTool.API.School;
using NotenTool.API.Semester;
using NotenTool.API.Subject;
using NotenTool.API.SubjectUser;
using NotenTool.API.UserInformation;
using NotenTool.API.UserSemester;
using NSubstitute;
using Xunit;

namespace NotenTool.Api.Test.Integration;

[Collection("Integration tests")]
public class IntegrationTest
{
    protected HttpClient HttpClient { get; }

    protected ISubjectRepository SubjectRepository { get; }
    protected ISemesterRepository SemesterRepository { get; }
    protected ISchoolRepository SchoolRepository { get; }
    protected IUserSemesterRepository UserSemesterRepository { get; }
    protected ISubjectUserRepository SubjectUserRepository { get; }
    protected IUserInformationRepository UserInformationRepository { get; }
    protected IGradeRepository GradeRepository { get; }

    protected IntegrationTest(IntegrationFixture fixture)
    {
        using var conn = new SqlConnection(TestApiModule.ConnectionString);
        conn.Execute("DELETE FROM Classes");
        conn.Execute("DELETE FROM School");
        conn.Execute("DELETE FROM Subject");
        conn.Execute("DELETE FROM Semester");
        conn.Execute("DELETE FROM UserInformation");

        HttpClient = fixture.HttpClient;
        SubjectRepository = fixture.SubjectRepository;
        SemesterRepository = fixture.SemesterRepository;
        SchoolRepository = fixture.SchoolRepository;
        UserSemesterRepository = fixture.UserSemesterRepository;
        SubjectUserRepository = fixture.SubjectUserRepository;
        UserInformationRepository = fixture.UserInformationRepository;
        GradeRepository = fixture.GradeRepository;
    }
}

public class IntegrationFixture : IDisposable
{
    private readonly WebApplicationFactory<Program> _application;
    internal HttpClient HttpClient { get; }

    internal ISubjectRepository SubjectRepository { get; }
    internal ISemesterRepository SemesterRepository { get; }
    internal ISchoolRepository SchoolRepository { get; }
    internal IUserSemesterRepository UserSemesterRepository { get; }
    internal ISubjectUserRepository SubjectUserRepository { get; }
    internal IUserInformationRepository UserInformationRepository { get; }
    internal IGradeRepository GradeRepository { get; }

    public IntegrationFixture()
    {
        SubjectRepository = Substitute.For<ISubjectRepository>();
        SemesterRepository = Substitute.For<ISemesterRepository>();
        SchoolRepository = Substitute.For<ISchoolRepository>();
        UserSemesterRepository = Substitute.For<IUserSemesterRepository>();
        SubjectUserRepository = Substitute.For<ISubjectUserRepository>();
        UserInformationRepository = Substitute.For<IUserInformationRepository>();
        GradeRepository = Substitute.For<IGradeRepository>();

        Program.ApiModule =
            new TestApiModule(SubjectRepository, SemesterRepository, SchoolRepository, UserSemesterRepository, SubjectUserRepository, UserInformationRepository,
                GradeRepository);


        _application = new WebApplicationFactory<Program>();

        HttpClient = _application.CreateClient();
    }

    void IDisposable.Dispose()
    {
        _application.Dispose();
    }
}

[CollectionDefinition("Integration tests")]
public class TestCollection : ICollectionFixture<IntegrationFixture>
{
}