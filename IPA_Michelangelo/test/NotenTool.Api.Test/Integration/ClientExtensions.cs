using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace NotenTool.Api.Test.Integration;

public static class ClientExtensions
{
    public static T? GetContent<T>(this HttpResponseMessage httpResponseMessage)
    {
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<T>(
            content,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }
}