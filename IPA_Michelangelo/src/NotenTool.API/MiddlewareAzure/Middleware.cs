using System.Net.Http.Headers;
using Microsoft.Graph;

//Middleware to Read MSUser and map to LocalUser

namespace NotenTool.API.MiddlewareAzure;

public class Middleware
{
    public static async Task<User> GetIdentity(string oauthToken, string baseUrl)
    {
        var graphClient = new GraphServiceClient(baseUrl,
            new DelegateAuthenticationProvider(async (requestMessage) =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", oauthToken);
            })
        );

        return await graphClient.Me.Request().GetAsync();
    }
}