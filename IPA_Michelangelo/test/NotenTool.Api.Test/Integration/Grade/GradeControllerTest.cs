/*using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NotenTool.API.Classes;
using NotenTool.API.Grade;
using NotenTool.API.Semester;
using NotenTool.API.Subject;
using NotenTool.Api.Test.Integration.EntityBuilder;
using Xunit;

namespace NotenTool.Api.Test.Integration.Grade;

public class GradeControllerTest : IntegrationTest
{
    public GradeControllerTest(IntegrationFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void returns_404_when_grades_are_returned()
    {
        var response = HttpClient.GetAsync($"/v1/Grade/{Guid.NewGuid()}").Result;

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task returns_200_when_grade_is_returned()
    {
        // Not working
        var subject = new DbSubject(Guid.NewGuid(), "Maths");
        var createdSubject = await HttpClient.PostAsync("/v1/Subject",
            new StringContent(JsonConvert.SerializeObject(subject), Encoding.Default, "application/json"));
        createdSubject.StatusCode.Should().Be(HttpStatusCode.OK);

        var semester = new DbSemester(Guid.NewGuid(), 1);
        var createdSemester = await HttpClient.PostAsync("/v1/Semester",
            new StringContent(JsonConvert.SerializeObject(semester
            ), Encoding.Default, "application/json"));
        createdSemester.StatusCode.Should().Be(HttpStatusCode.OK);


        var postGrade = new DbGrade(Guid.NewGuid(), 6, "test", DateTime.Now, 1,
            createdSubject.GetContent<DbSubject>()!.Id, 
            createdSemester.GetContent<DbSemester>()!.Id);
        var createGrade = await HttpClient.PostAsync($"/v1/Grade/{users!.Id}",
            new StringContent(JsonConvert.SerializeObject(postGrade), Encoding.Default, "application/json"));
        createGrade.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await HttpClient.GetAsync($"/v1/Grade/{createGrade.GetContent<DbGrade>()!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}*/