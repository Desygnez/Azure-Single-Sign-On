using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NotenTool.API.School;
using Xunit;

namespace NotenTool.Api.Test.Integration.School;

public class SchoolControllerTest : IntegrationTest
{
    public SchoolControllerTest(IntegrationFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void returns_404_when_school_are_returned()
    {
        var response = HttpClient.GetAsync($"/v1/School/{Guid.NewGuid()}").Result;

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task returns_school_with_id()
    {
        var postSchool = new DbSchool(Guid.NewGuid(), "BBW");

        var createdSchool = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(postSchool), Encoding.Default,
                "application/json"));
        createdSchool.StatusCode.Should().Be(HttpStatusCode.OK);
        var school = createdSchool.GetContent<DbSchool>();

        var actual = HttpClient.GetAsync($"/v1/School/{school!.Id}").Result.GetContent<DbSchool>();

        actual.Should().Be(school);
    }

    [Fact]
    public async Task returns_all_schools()
    {
        var createdSchool = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(new DbSchool(Guid.NewGuid(), "Bbw")), Encoding.Default,
                "application/json"));
        createdSchool.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdSchool2 = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(new DbSchool(Guid.NewGuid(), "TBZ")), Encoding.Default,
                "application/json"));
        createdSchool2.StatusCode.Should().Be(HttpStatusCode.OK);

        var school1 = createdSchool.GetContent<DbSchool>()!;
        var school2 = createdSchool2.GetContent<DbSchool>()!;
        List<DbSchool> schools = new()
        {
            school1, school2
        };
        var actual = HttpClient.GetAsync("/v1/School").Result.GetContent<List<DbSchool>>();
        actual.Should().BeEquivalentTo(schools);
    }

    [Fact]
    public async Task school_should_be_created()
    {
        var testSchool = new DbSchool(Guid.NewGuid(), "Bbw");
        var createdSchool = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(testSchool), Encoding.Default, "application/json"));

        createdSchool.StatusCode.Should().Be(HttpStatusCode.OK);
        var school = createdSchool.GetContent<DbSchool>();

        var actual = await HttpClient.GetAsync($"v1/School/{school!.Id}");

        actual.GetContent<DbSchool>().Should().Be(school);
    }

    [Fact]
    public async Task creates_school_returns_unauthorized()
    {
        var id = Guid.NewGuid();
        var testSchool = new DbSchool(id, "Bbc");

        var actual = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(testSchool), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task update_school_returns_no_content()
    {
        var oldSchool = new DbSchool(Guid.NewGuid(), "Bbw");

        var old = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(oldSchool), Encoding.Default, "application/json"));
        old.StatusCode.Should().Be(HttpStatusCode.OK);

        var oldContent = old.GetContent<DbSchool>();

        var updatedSchool = new DbSchool(oldContent!.Id, "Bbc" );

        var updateSchool = await HttpClient.PutAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(updatedSchool), Encoding.Default, "application/json"));

        updateSchool.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var actual = await HttpClient.GetAsync($"v1/School/{oldContent.Id}");
        actual.GetContent<DbSchool>().Should().Be(updatedSchool);
    }
    [Fact]
    public async Task update_school_returns_404()
    {
        var id = Guid.NewGuid();

        var fakeUpdatedSchool = new DbSchool(id, "KV Zürich");

        var actual = await HttpClient.PutAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(fakeUpdatedSchool), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task should_delete_school()
    {
        var testSchool = new DbSchool(Guid.NewGuid(), "Frauenfeld");
        
        var responseMessage = await HttpClient.PostAsync("/v1/School",
            new StringContent(JsonConvert.SerializeObject(testSchool),
                Encoding.Default,
                "application/json"));
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdSchool = responseMessage.GetContent<DbSchool>();

        var delete = await HttpClient.DeleteAsync($"/v1/School/{createdSchool!.Id}");
        delete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkSchool = await HttpClient.GetAsync($"/v1/School/{createdSchool.Id}");

        checkSchool.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task delete_school_returns_not_found()
    {
        var fakeId = Guid.NewGuid();
        
        var actual = await HttpClient.DeleteAsync($"/v1/School/{fakeId}");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}