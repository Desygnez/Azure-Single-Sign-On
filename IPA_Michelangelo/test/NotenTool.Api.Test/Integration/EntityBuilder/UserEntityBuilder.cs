/*using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NotenTool.API.UserInformation;

namespace NotenTool.Api.Test.Integration.EntityBuilder;

public class UserEntityBuilder
{
    HttpClient _httpClient;
    private string _roleName;
    private string _user;

    public UserEntityBuilder(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public UserEntityBuilder WithRole(string name)
    {
        _roleName = name;
        return this;
    }

    public UserEntityBuilder WithUser(string username)
    {
        _user = username;
        return this;
    }

    public async Task<(string, DbUserInformation?)> Build()
    {
        var role = await CreateRole(_roleName);
        var users = await CreateUser(role!.Id, _user);

        return (role, users);
    }

    private async Task<DbUserInformation?> CreateUser(Guid roleId, string username)
    {
        var postUserInformation = new DbUserInformation(Guid.NewGuid(), "test", "test", username, false,
            "test@kpmg.com");
        var createdUserInformation = await _httpClient.PostAsync("/UserInformation",
            new StringContent(JsonConvert.SerializeObject(postUserInformation), Encoding.Default,
                "application/json"));
        createdUserInformation.StatusCode.Should().Be(HttpStatusCode.OK);
        return createdUserInformation.GetContent<DbUserInformation>();
    }

    private async Task<string> CreateRole(string name)
    {
        /*var postRole = new DbRoles(Guid.NewGuid(), name);
        var createdRole = await _httpClient.PostAsync("/Role",
            new StringContent(JsonConvert.SerializeObject(postRole), Encoding.Default,
                "application/json"));
        createdRole.StatusCode.Should().Be(HttpStatusCode.OK);
        return createdRole.GetContent<DbRoles>();#1#
    }
}*/