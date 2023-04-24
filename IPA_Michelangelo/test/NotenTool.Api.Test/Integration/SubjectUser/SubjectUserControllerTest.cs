/*using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NotenTool.API.Role;
using NotenTool.API.School;
using NotenTool.API.Semester;
using NotenTool.API.Subject;
using NotenTool.API.SubjectUser;
using NotenTool.API.UserInformation;
using Xunit;

namespace NotenTool.Api.Test.Integration.SubjectUser;

public class SubjectUserControllerTest : IntegrationTest
{
    private Guid RoleId { get; set; }
    private Guid UserId { get; set; }
    private Guid SchoolId { get; set; }
    private Guid SubjectId { get; set; }
    private Guid SemesterId { get; set; }

    public SubjectUserControllerTest(IntegrationFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Setup()
    {
        var postRole = new DbRoles(Guid.NewGuid(), "test");
        var createdRole = await HttpClient.PostAsync("/v1/Role",
            new StringContent(JsonConvert.SerializeObject(postRole), Encoding.Default,
                "application/json"));
        createdRole.StatusCode.Should().Be(HttpStatusCode.OK);
        var role = createdRole.GetContent<DbRoles>();
        RoleId = createdRole.GetContent<DbRoles>()!.Id;

        var postUserInformation = new DbUserInformation(Guid.NewGuid(), "test", "test", "test", false, "test@kpmg.com");
        var createdUserInformation = await HttpClient.PostAsync("/v1/UserInformation",
            new StringContent(JsonConvert.SerializeObject(postUserInformation), Encoding.Default,
                "application/json"));
        createdUserInformation.StatusCode.Should().Be(HttpStatusCode.OK);
        UserId = createdUserInformation.GetContent<DbUserInformation>()!.Id;

        var postSchool = new DbSchool(Guid.NewGuid(), "test");
        var createdSchool = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(postSchool), Encoding.Default,
                "application/json"));
        createdSchool.StatusCode.Should().Be(HttpStatusCode.OK);
        SchoolId = createdSchool.GetContent<DbSchool>()!.Id;

        var postSemester = new DbSemester(Guid.NewGuid(), 3);
        var createdSemester = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(postSemester), Encoding.Default,
                "application/json"));
        createdSemester.StatusCode.Should().Be(HttpStatusCode.OK);
        SemesterId = createdSemester.GetContent<DbSemester>()!.Id;

        var postSubject = new DbSubject(Guid.NewGuid(), "French");
        var createdSubject = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(postSubject), Encoding.Default,
                "application/json"));
        createdSubject.StatusCode.Should().Be(HttpStatusCode.OK);
        SubjectId = createdSubject.GetContent<DbSubject>()!.Id;
    }

    [Fact]
    public void returns_404_when_user_subject_are_returned()
    {
        var response = HttpClient.GetAsync($"v1/SubjectUser/{Guid.NewGuid()}").Result;
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task returns_subjectUser_with_id()
    {
        await Setup();
        var postSubjectUser = new DbSubjectUser(Guid.NewGuid(), SubjectId, UserId, SchoolId, SemesterId);

        var createdSubjectUser = await HttpClient.PostAsync("v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(postSubjectUser), Encoding.Default,
                "application/json"));
        createdSubjectUser.StatusCode.Should().Be(HttpStatusCode.OK);
        var userSubject = createdSubjectUser.GetContent<DbSubjectUser>();
        var actual = HttpClient.GetAsync($"v1/SubjectUser/{userSubject!.Id}").Result.GetContent<DbSubjectUser>();


        actual!.Id.Should().Be(userSubject.Id);
        actual.School_id.Should().Be(userSubject.School_id);
        actual.UserInfo_id.Should().Be(userSubject.UserInfo_id);
        actual.Subject_id.Should().Be(userSubject.Subject_id);
        actual.Semester_id.Should().Be(userSubject.Semester_id);
    }

    [Fact]
    public async Task returns_subjectUser_with_userId()
    {
        await Setup();
        var postSubjectUser = new DbSubjectUser(Guid.NewGuid(), SubjectId, UserId, SchoolId, SemesterId);

        var createdSubjectUser = await HttpClient.PostAsync("v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(postSubjectUser), Encoding.Default,
                "application/json"));
        createdSubjectUser.StatusCode.Should().Be(HttpStatusCode.OK);
        var subjectUser = createdSubjectUser.GetContent<DbSubjectUser>();

        var actual = HttpClient.GetAsync($"v1/SubjectUser/GetByUserId/{subjectUser!.UserInfo_id}").Result
            .GetContent<List<DbSubjectUser>>();


        actual![0].Id.Should().Be(subjectUser.Id);
        actual[0].School_id.Should().Be(subjectUser.School_id);
        actual[0].UserInfo_id.Should().Be(subjectUser.UserInfo_id);
        actual[0].Semester_id.Should().Be(subjectUser.Semester_id);
        actual[0].Subject_id.Should().Be(subjectUser.Subject_id);
    }

    [Fact]
    public void returns_404_when_subject_user_are_returned_with_userId()
    {
        var response = HttpClient.GetAsync($"v1/SubjectUser/GetByUserId/{Guid.NewGuid()}").Result;
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task create_subject_user_returns_ok()
    {
        await Setup();

        var testSubjectUser = new DbSubjectUser(Guid.NewGuid(), SubjectId, UserId, SchoolId, SemesterId);

        var createdSubjectUser = await HttpClient.PostAsync("/v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(testSubjectUser), Encoding.Default, "application/json"));

        createdSubjectUser.StatusCode.Should().Be(HttpStatusCode.OK);
        var subjectUser = createdSubjectUser.GetContent<DbSubjectUser>();

        var actual = HttpClient.GetAsync($"v1/SubjectUser/{subjectUser!.Id}").Result.GetContent<DbSubjectUser>()!;

        actual.Id.Should().Be(subjectUser.Id);
        actual.UserInfo_id.Should().Be(subjectUser.UserInfo_id);
        actual.School_id.Should().Be(subjectUser.School_id);
        actual.Semester_id.Should().Be(subjectUser.Semester_id);
        actual.Subject_id.Should().Be(subjectUser.Subject_id);
    }

    [Fact]
    public async Task creates_subject_user_returns_unauthorized()
    {
        await Setup();
        var userSchool = new DbSubjectUser(Guid.NewGuid(), SubjectId, UserId, SchoolId, SemesterId);

        var actual = await HttpClient.PostAsync("/v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(userSchool), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task update_subject_user_returns_no_content()
    {
        await Setup();

        var postUserInformation = new DbUserInformation(Guid.NewGuid(), "Shansai", "test", "test", false,
            "test@kpmg.com");
        var updatedUserInformation = await HttpClient.PostAsync("/v1/UserInformation",
            new StringContent(JsonConvert.SerializeObject(postUserInformation), Encoding.Default,
                "application/json"));
        updatedUserInformation.StatusCode.Should().Be(HttpStatusCode.OK);
        var updateUserId = updatedUserInformation.GetContent<DbUserInformation>()!.Id;

        var oldSubjectUser = new DbSubjectUser(Guid.NewGuid(), SubjectId, UserId, SchoolId, SemesterId);

        var old = await HttpClient.PostAsync("/v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(oldSubjectUser), Encoding.Default, "application/json"));
        old.StatusCode.Should().Be(HttpStatusCode.OK);

        var oldContent = old.GetContent<DbSubjectUser>();

        var updatedSubjectUser = new DbSubjectUser(oldContent!.Id, SubjectId, updateUserId, SchoolId, SemesterId);

        var newSubjectUser = await HttpClient.PutAsync("/v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(updatedSubjectUser), Encoding.Default, "application/json"));

        newSubjectUser.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var actual = HttpClient.GetAsync($"v1/SubjectUser/{oldContent.Id}").Result.GetContent<DbSubjectUser>()!;
        actual.Id.Should().Be(updatedSubjectUser.Id);
        actual.School_id.Should().Be(updatedSubjectUser.School_id);
        actual.UserInfo_id.Should().Be(updatedSubjectUser.UserInfo_id);
        actual.Subject_id.Should().Be(updatedSubjectUser.Subject_id);
        actual.Semester_id.Should().Be(updatedSubjectUser.Semester_id);
    }

    [Fact]
    public async Task update_subject_user_returns_404()
    {
        await Setup();

        var fakeUpdatedSubjectUser =
            new DbSubjectUser(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        var actual = await HttpClient.PutAsync("/v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(fakeUpdatedSubjectUser), Encoding.Default,
                "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task should_delete_subject_user()
    {
        await Setup();
        var testSubjectUser = new DbSubjectUser(Guid.NewGuid(), SubjectId, UserId, SchoolId, SemesterId);

        var responseMessage = await HttpClient.PostAsync("/v1/SubjectUser",
            new StringContent(JsonConvert.SerializeObject(testSubjectUser),
                Encoding.Default,
                "application/json"));
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdSubjectUser = responseMessage.GetContent<DbSubjectUser>();

        var delete = await HttpClient.DeleteAsync($"/v1/SubjectUser/{createdSubjectUser!.Id}");
        delete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkSubjectUser = await HttpClient.GetAsync($"/v1/SubjectUser/{createdSubjectUser.Id}");

        checkSubjectUser.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task delete_subject_user_returns_not_found()
    {
        var fakeId = Guid.NewGuid();
        
        var actual = await HttpClient.DeleteAsync($"/v1/SubjectUser/{fakeId}");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}*/