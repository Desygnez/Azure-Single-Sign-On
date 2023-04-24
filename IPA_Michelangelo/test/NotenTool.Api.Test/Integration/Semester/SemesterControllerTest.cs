using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NotenTool.API.Semester;
using Xunit;

namespace NotenTool.Api.Test.Integration.Semester;

public class SemesterControllerTest : IntegrationTest
{
    public SemesterControllerTest(IntegrationFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void returns_404_when_semester_are_returned()
    {
        var response = HttpClient.GetAsync($"/v1/Semester/{Guid.NewGuid()}").Result;

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task returns_subject_with_id()
    {
        var postSemester = new DbSemester(Guid.NewGuid(), 1);

        var createdSemester = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(postSemester), Encoding.Default,
                "application/json"));
        createdSemester.StatusCode.Should().Be(HttpStatusCode.OK);
        var semester = createdSemester.GetContent<DbSemester>();

        var actual = HttpClient.GetAsync($"/v1/Semester/{semester!.Id}").Result.GetContent<DbSemester>();

        actual.Should().Be(semester);
    }

    [Fact]
    public async Task returns_all_semesters()
    {
        var createdSemester1 = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(new DbSemester(Guid.NewGuid(), 1)), Encoding.Default,
                "application/json"));
        createdSemester1.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdSemester2 = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(new DbSemester(Guid.NewGuid(), 2)), Encoding.Default,
                "application/json"));
        createdSemester2.StatusCode.Should().Be(HttpStatusCode.OK);

        var semester1 = createdSemester1.GetContent<DbSemester>()!;
        var semester2 = createdSemester2.GetContent<DbSemester>()!;
        List<DbSemester> semesters = new()
        {
            semester1, semester2
        };
        var actual = HttpClient.GetAsync("/v1/Semester").Result.GetContent<List<DbSemester>>();
        actual.Should().BeEquivalentTo(semesters);
    }

    [Fact]
    public async Task create_semester_returns_ok()
    {
        var testSemester = new DbSemester(Guid.NewGuid(), 1);

        var createdSemester = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(testSemester), Encoding.Default, "application/json"));

        createdSemester.StatusCode.Should().Be(HttpStatusCode.OK);
        var semester = createdSemester.GetContent<DbSemester>();

        var actual = await HttpClient.GetAsync($"v1/Semester/{semester!.Id}");

        actual.GetContent<DbSemester>().Should().Be(semester);
    }

    [Fact]
    public async Task creates_semester_returns_unauthorized()
    {
        var id = Guid.NewGuid();
        var testSemester = new DbSemester(id, 1);

        var actual = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(testSemester), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task update_semester_returns_no_content()
    {
        var oldSemester = new DbSemester(Guid.NewGuid(), 3);

        var old = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(oldSemester), Encoding.Default, "application/json"));
        old.StatusCode.Should().Be(HttpStatusCode.OK);

        var oldContent = old.GetContent<DbSemester>();

        var updatedSemester = new DbSemester(oldContent!.Id, 4);

        var updateSemester = await HttpClient.PutAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(updatedSemester), Encoding.Default, "application/json"));

        updateSemester.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var actual = await HttpClient.GetAsync($"v1/Semester/{oldContent.Id}");
        actual.GetContent<DbSemester>().Should().Be(updatedSemester);
    }

    [Fact]
    public async Task update_semester_returns_404()
    {
        var id = Guid.NewGuid();

        var fakeUpdatedSemester = new DbSemester(id, 6);

        var actual = await HttpClient.PutAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(fakeUpdatedSemester), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task should_delete_semester()
    {
        var testSemester = new DbSemester(Guid.NewGuid(), 0);

        var responseMessage = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(testSemester),
                Encoding.Default,
                "application/json"));
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdSemester = responseMessage.GetContent<DbSemester>();

        var delete = await HttpClient.DeleteAsync($"/v1/Semester/{createdSemester!.Id}");
        delete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkSemester = await HttpClient.GetAsync($"/v1/Semester/{createdSemester.Id}");

        checkSemester.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task delete_semester_returns_not_found()
    {
        var fakeId = Guid.NewGuid();

        var actual = await HttpClient.DeleteAsync($"/v1/Semester/{fakeId}");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}