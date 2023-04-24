using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NotenTool.API.Classes;
using NotenTool.API.Subject;
using NSubstitute;
using Xunit;

namespace NotenTool.Api.Test.Integration.Subject;

public class SubjectControllerTest : IntegrationTest
{
    public SubjectControllerTest(IntegrationFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void returns_404_when_subjects_are_returned()
    {
        var response = HttpClient.GetAsync($"v1/Subject/{Guid.NewGuid()}").Result;

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task returns_subject_with_id()
    {
        var postSubject = new DbSubject(Guid.NewGuid(), "test");

        var createdSubject = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(postSubject), Encoding.Default,
                "application/json"));
        createdSubject.StatusCode.Should().Be(HttpStatusCode.OK);
        var subject = createdSubject.GetContent<DbSubject>();

        var actual = HttpClient.GetAsync($"/v1/Subject/{subject!.Id}").Result.GetContent<DbSubject>();

        actual.Should().Be(subject);
    }

    [Fact]
    public async Task returns_all_subjects()
    {
        var createdSubject1 = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(new DbSubject(Guid.NewGuid(), "Italian")), Encoding.Default,
                "application/json"));
        createdSubject1.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdSubject2 = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(new DbSubject(Guid.NewGuid(), "German")), Encoding.Default,
                "application/json"));
        createdSubject2.StatusCode.Should().Be(HttpStatusCode.OK);

        var subject1 = createdSubject1.GetContent<DbSubject>()!;
        var subject2 = createdSubject2.GetContent<DbSubject>()!;
        List<DbSubject> subjects = new()
        {
            subject1, subject2
        };
        var actual = HttpClient.GetAsync("/v1/Subject").Result.GetContent<List<DbSubject>>();
        actual.Should().BeEquivalentTo(subjects);
    }

    [Fact]
    public async Task create_subject_returns_ok()
    {
        var testClass = new DbSubject(Guid.NewGuid(), "Math");

        var createdSubject = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(testClass), Encoding.Default, "application/json"));

        createdSubject.StatusCode.Should().Be(HttpStatusCode.OK);
        var subject = createdSubject.GetContent<DbSubject>();

        var actual = await HttpClient.GetAsync($"v1/Subject/{subject!.Id}");

        actual.GetContent<DbSubject>().Should().Be(subject);
    }

    [Fact]
    public async Task creates_subject_returns_unauthorized()
    {
        var id = Guid.NewGuid();
        var testClass = new DbSubject(id, "Math");

        var actual = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(testClass), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    } 
    [Fact]
    public async Task update_subject_returns_no_content()
    {
        var oldSubject = new DbSubject(Guid.NewGuid(), "French");

        var old = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(oldSubject), Encoding.Default, "application/json"));
        old.StatusCode.Should().Be(HttpStatusCode.OK);

        var oldContent = old.GetContent<DbSubject>();

        var updatedSubject = new DbSubject(oldContent!.Id, "English" );

        var updateSubject = await HttpClient.PutAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(updatedSubject), Encoding.Default, "application/json"));

        updateSubject.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var actual = await HttpClient.GetAsync($"v1/Subject/{oldContent.Id}");
        actual.GetContent<DbSubject>().Should().Be(updatedSubject);
    }
    [Fact]
    public async Task update_subject_returns_404()
    {
        var id = Guid.NewGuid();

        var fakeUpdatedSubject = new DbSubject(id, "Wirtschaft");

        var actual = await HttpClient.PutAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(fakeUpdatedSubject), Encoding.Default, "application/json"));

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task should_delete_Subject()
    {
        var testSubject = new DbSubject(Guid.NewGuid(), "German");
        
        var responseMessage = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(testSubject),
                Encoding.Default,
                "application/json"));
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdSubject = responseMessage.GetContent<DbSubject>();

        var delete = await HttpClient.DeleteAsync($"/v1/Subject/{createdSubject!.Id}");
        delete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkSubject = await HttpClient.GetAsync($"/v1/Subject/{createdSubject.Id}");

        checkSubject.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task delete_subject_returns_not_found()
    {
        var fakeId = Guid.NewGuid();
        
        var actual = await HttpClient.DeleteAsync($"/v1/Subject/{fakeId}");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}