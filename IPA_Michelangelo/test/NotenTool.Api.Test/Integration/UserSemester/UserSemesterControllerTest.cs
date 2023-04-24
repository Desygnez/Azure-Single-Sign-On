/*using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NotenTool.API.Semester;
using NotenTool.API.UserInformation;
using NotenTool.API.UserSemester;
using Xunit;

namespace NotenTool.Api.Test.Integration.UserSemester;

public class UserSemesterControllerTest : IntegrationTest
{
    private Guid RoleId { get; set; }
    private Guid UserId { get; set; }
    private Guid SemesterId { get; set; }

    public UserSemesterControllerTest(IntegrationFixture fixture) : base(fixture)
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

        var postSemester = new DbSemester(Guid.NewGuid(), 1);
        var createdSemester = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(postSemester), Encoding.Default,
                "application/json"));
        createdSemester.StatusCode.Should().Be(HttpStatusCode.OK);
        SemesterId = createdSemester.GetContent<DbSemester>()!.Id;
    }

    [Fact]
    public void returns_404_when_user_semesters_are_returned()
    {
        var response = HttpClient.GetAsync($"v1/UserSemesters/{Guid.NewGuid()}").Result;
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task returns_userSemester_with_id()
    {
        await Setup();
        var postUserSemester = new DbUserSemester(Guid.NewGuid(), UserId, SemesterId);

        var createdUserSemester = await HttpClient.PostAsync("v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(postUserSemester), Encoding.Default,
                "application/json"));
        createdUserSemester.StatusCode.Should().Be(HttpStatusCode.OK);
        var userSemester = createdUserSemester.GetContent<DbUserSemester>();
        var actual = HttpClient.GetAsync($"v1/UserSemester/{userSemester!.Id}").Result.GetContent<DbUserSemester>();


        actual!.Id.Should().Be(userSemester.Id);
        actual.Semester_id.Should().Be(userSemester.Semester_id);
        actual.UserInfo_id.Should().Be(userSemester.UserInfo_id);
    }

    [Fact]
    public async Task returns_userSemester_with_userId()
    {
        await Setup();
        var postUserSemester = new DbUserSemester(Guid.NewGuid(), UserId, SemesterId);

        var createdUserSemester = await HttpClient.PostAsync("v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(postUserSemester), Encoding.Default,
                "application/json"));
        createdUserSemester.StatusCode.Should().Be(HttpStatusCode.OK);
        var userSemester = createdUserSemester.GetContent<DbUserSemester>();
        var actual = HttpClient.GetAsync($"v1/UserSemester/GetByUserId/{userSemester!.UserInfo_id}").Result
            .GetContent<List<DbUserSemester>>();


        actual![0].Id.Should().Be(userSemester.Id);
        actual[0].Semester_id.Should().Be(userSemester.Semester_id);
        actual[0].UserInfo_id.Should().Be(userSemester.UserInfo_id);
    }

    [Fact]
    public void returns_404_when_userSemester_are_returned_with_userId()
    {
        var response = HttpClient.GetAsync($"v1/UserSemester/GetByUserId/{Guid.NewGuid()}").Result;
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task create_userSemester_returns_ok()
    {
        await Setup();

        var testUserSemester = new DbUserSemester(Guid.NewGuid(), UserId, SemesterId);

        var createdUserSemester = await HttpClient.PostAsync("/v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(testUserSemester), Encoding.Default, "application/json"));

        createdUserSemester.StatusCode.Should().Be(HttpStatusCode.OK);
        var userSemester = createdUserSemester.GetContent<DbUserSemester>();

        var actual = HttpClient.GetAsync($"v1/UserSemester/{userSemester!.Id}").Result.GetContent<DbUserSemester>()!;

        actual.Id.Should().Be(userSemester.Id);
        actual.UserInfo_id.Should().Be(userSemester.UserInfo_id);
        actual.Semester_id.Should().Be(userSemester.Semester_id);
    }

    [Fact]
    public async Task creates_userSemester_returns_unauthorized()
    {
        await Setup();
        var userSemester = new DbUserSemester(Guid.NewGuid(), UserId, SemesterId);

        var actual = await HttpClient.PostAsync("/v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(userSemester), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task update_userSemester_returns_no_content()
    {
        await Setup();

        var postUserInformation = new DbUserInformation(Guid.NewGuid(), "Shansai", "test", "test", false,
            "test@kpmg.com");
        var newUserInformation = await HttpClient.PostAsync("/v1/UserInformation",
            new StringContent(JsonConvert.SerializeObject(postUserInformation), Encoding.Default,
                "application/json"));
        newUserInformation.StatusCode.Should().Be(HttpStatusCode.OK);
        var newUserId = newUserInformation.GetContent<DbUserInformation>()!.Id;

        var oldUserSemester = new DbUserSemester(Guid.NewGuid(), UserId, SemesterId);

        var old = await HttpClient.PostAsync("/v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(oldUserSemester), Encoding.Default, "application/json"));
        old.StatusCode.Should().Be(HttpStatusCode.OK);

        var oldContent = old.GetContent<DbUserSemester>();

        var newUserSemester = new DbUserSemester(oldContent!.Id, newUserId, SemesterId);

        var updateUserSemester = await HttpClient.PutAsync("/v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(newUserSemester), Encoding.Default, "application/json"));

        updateUserSemester.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var actual = HttpClient.GetAsync($"v1/UserSemester/{oldContent.Id}").Result.GetContent<DbUserSemester>()!;

        actual.Id.Should().Be(newUserSemester.Id);
        actual.Semester_id.Should().Be(newUserSemester.Semester_id);
        actual.UserInfo_id.Should().Be(newUserSemester.UserInfo_id);
    }
    
    [Fact]
    public async Task update_userSemester_returns_404()
    {
        await Setup();

        var fakeUpdatedUserSemester = new DbUserSemester(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        var actual = await HttpClient.PutAsync("/v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(fakeUpdatedUserSemester), Encoding.Default,
                "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task should_delete_userSemester()
    {
        await Setup();
        var testUserSemester = new DbUserSemester(Guid.NewGuid(),UserId, SemesterId);
        
        var responseMessage = await HttpClient.PostAsync("/v1/UserSemester",
            new StringContent(JsonConvert.SerializeObject(testUserSemester),
                Encoding.Default,
                "application/json"));
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdUserSemester = responseMessage.GetContent<DbUserSemester>();

        var delete = await HttpClient.DeleteAsync($"/v1/UserSemester/{createdUserSemester!.Id}");
        delete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkUserSemester = await HttpClient.GetAsync($"/v1/UserSemester/{createdUserSemester.Id}");

        checkUserSemester.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task delete_userSemester_returns_not_found()
    {
        var fakeId = Guid.NewGuid();
        
        var actual = await HttpClient.DeleteAsync($"/v1/UserSemester/{fakeId}");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}*/