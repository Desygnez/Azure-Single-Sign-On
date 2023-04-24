/*using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace NotenTool.Api.Test.Integration.UserInformation;

public class UserInformationControllerTest : IntegrationTest
{
    private Guid RoleId { get; set; }

    public UserInformationControllerTest(IntegrationFixture fixture) : base(fixture)
    {
    }

    
    [Fact]
    public void returns_404_when_userinformation_are_returned()
    {
        var response = HttpClient.GetAsync($"v1/UserInformation/{Guid.NewGuid()}").Result;
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
  
}*/